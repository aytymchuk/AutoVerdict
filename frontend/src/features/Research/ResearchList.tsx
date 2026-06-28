import { useTranslation } from '../../shared/lib/i18n';
import type { ResearchListItemDto } from '../../shared/api/research';
import { ResearchCard } from './ResearchCard';

interface ResearchListProps {
  items: ResearchListItemDto[];
  total: number;
  hasMore: boolean;
  loadingMore: boolean;
  onNewResearch: () => void;
  onLoadMore: () => void;
}

export function ResearchList({
  items,
  total,
  hasMore,
  loadingMore,
  onNewResearch,
  onLoadMore,
}: ResearchListProps) {
  const { t } = useTranslation();

  const countLabel = (
    total === 1 ? t('research_list_count_one') : t('research_list_count_many')
  ).replace('{count}', String(total));

  return (
    <div className="mx-auto w-full max-w-[960px] px-gutter py-10">
      <div className="mb-8 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="heading-page">
            {t('research_list_heading')}
          </h1>
          <p className="mt-1 text-[14px] text-on-surface-variant">{countLabel}</p>
        </div>

        <div className="flex items-center gap-3">
          <button
            type="button"
            onClick={onNewResearch}
            className="inline-flex items-center gap-2 rounded-full bg-primary px-5 py-2.5 text-[14px] font-semibold text-on-primary transition-colors hover:bg-primary-fixed-dim focus:outline-none focus:ring-2 focus:ring-risk-medium/50"
          >
            <span className="material-symbols-outlined text-[18px]" aria-hidden="true">
              add
            </span>
            {t('research_new_analysis')}
          </button>
          <button
            type="button"
            disabled
            aria-label="Filter research"
            className="flex h-10 w-10 items-center justify-center rounded-full border border-outline-variant/30 bg-surface-container text-on-surface-variant opacity-50"
          >
            <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
              filter_list
            </span>
          </button>
          <button
            type="button"
            disabled
            aria-label="Search research"
            className="flex h-10 w-10 items-center justify-center rounded-full border border-outline-variant/30 bg-surface-container text-on-surface-variant opacity-50"
          >
            <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
              search
            </span>
          </button>
        </div>
      </div>

      <div className="flex flex-col gap-4">
        {items.map((research) => (
          <ResearchCard key={research.id} research={research} />
        ))}
      </div>

      {hasMore && (
        <div className="mt-8 flex justify-center">
          <button
            type="button"
            onClick={onLoadMore}
            disabled={loadingMore}
            className="rounded-full border border-outline-variant/30 bg-surface-container px-6 py-2.5 text-[14px] font-medium text-on-surface transition-colors hover:bg-surface-container-high disabled:opacity-50"
          >
            {loadingMore ? t('research_loading_more') : t('research_load_more')}
          </button>
        </div>
      )}
    </div>
  );
}
