import { useCallback, useEffect, useState } from 'react';
import { NewResearchDialog } from '../features/Research/NewResearchDialog';
import { ResearchEmptyState } from '../features/Research/ResearchEmptyState';
import { ResearchList } from '../features/Research/ResearchList';
import { AppLayout } from '../shared/components/AppLayout';
import type { ResearchListItemDto } from '../shared/api/research';
import { useResearchApi } from '../shared/api/research';
import { useTranslation } from '../shared/lib/i18n';

const PAGE_SIZE = 20;

export function HomePage() {
  const { t } = useTranslation();
  const researchApi = useResearchApi();
  const [researches, setResearches] = useState<ResearchListItemDto[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [loadingMore, setLoadingMore] = useState(false);
  const [initialLoadFailed, setInitialLoadFailed] = useState(false);
  const [showCreate, setShowCreate] = useState(false);

  const applyListResult = useCallback(
    (result: { items: ResearchListItemDto[]; total: number }, pageNumber: number, append: boolean) => {
      setTotal(result.total);
      setPage(pageNumber);
      setResearches((current) => (append ? [...current, ...result.items] : result.items));
    },
    []
  );

  useEffect(() => {
    let cancelled = false;

    researchApi
      .list(1, PAGE_SIZE)
      .then((result) => {
        if (!cancelled) {
          applyListResult(result, 1, false);
          setInitialLoadFailed(false);
        }
      })
      .catch(() => {
        if (!cancelled) {
          setInitialLoadFailed(true);
        }
      })
      .finally(() => {
        if (!cancelled) {
          setLoading(false);
        }
      });

    return () => {
      cancelled = true;
    };
  }, [researchApi, applyListResult]);

  const refreshResearches = useCallback(async () => {
    setLoading(true);

    try {
      const result = await researchApi.list(1, PAGE_SIZE);
      applyListResult(result, 1, false);
    } catch {
      // silently fail: existing data stays visible
    } finally {
      setLoading(false);
    }
  }, [researchApi, applyListResult]);

  const hasMore = researches.length < total;


  async function handleLoadMore() {
    if (!hasMore || loadingMore) {
      return;
    }

    setLoadingMore(true);

    try {
      const result = await researchApi.list(page + 1, PAGE_SIZE);
      applyListResult(result, page + 1, true);
    } catch {
      // silently fail: existing data stays visible
    } finally {
      setLoadingMore(false);
    }
  }

  function handleCreated() {
    void refreshResearches();
  }

  return (
    <AppLayout>
      {loading ? (
        <div className="mx-auto flex w-full max-w-[960px] flex-col gap-4 px-gutter py-10">
          <div className="h-10 w-48 animate-pulse rounded-lg bg-surface-container" />
          <div className="h-32 animate-pulse rounded-2xl bg-surface-container" />
          <div className="h-32 animate-pulse rounded-2xl bg-surface-container" />
        </div>
      ) : initialLoadFailed ? (
        <div className="mx-auto flex w-full max-w-[720px] flex-col items-center gap-4 px-gutter py-16 text-center">
          <span className="material-symbols-outlined text-[48px] text-risk-high" aria-hidden="true">
            error
          </span>
          <p className="text-[16px] text-on-surface-variant">{t('home_load_error')}</p>
          <button
            type="button"
            onClick={() => void refreshResearches()}
            className="rounded-full border border-outline-variant/30 bg-surface-container px-5 py-2.5 text-[14px] font-medium text-on-surface"
          >
            {t('common_retry')}
          </button>
        </div>
      ) : total === 0 ? (
        <ResearchEmptyState onStartResearch={() => setShowCreate(true)} />
      ) : (
        <ResearchList
          items={researches}
          total={total}
          hasMore={hasMore}
          loadingMore={loadingMore}
          onNewResearch={() => setShowCreate(true)}
          onLoadMore={() => void handleLoadMore()}
        />
      )}

      <NewResearchDialog
        open={showCreate}
        onClose={() => setShowCreate(false)}
        onCreated={handleCreated}
      />
    </AppLayout>
  );
}
