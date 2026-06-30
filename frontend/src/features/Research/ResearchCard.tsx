import { useTranslation } from '../../shared/lib/i18n';
import { useUserStatus } from '../../shared/hooks/useUserStatus';
import type { ResearchListItemDto } from '../../shared/api/research';
import {
  formatAnalysisDate,
  formatResearchSubtitle,
  formatResearchTitle,
  getRiskBadgeStyle,
} from './researchDisplay';

const KNOWN_CURRENCIES = ['PLN', 'EUR', 'USD', 'UAH'] as const;

interface ResearchCardProps {
  research: ResearchListItemDto;
}

export function ResearchCard({ research }: ResearchCardProps) {
  const { t, language } = useTranslation();
  const { defaultCurrency } = useUserStatus();
  const riskBadge = getRiskBadgeStyle(research.riskLevel);
  const currency =
    defaultCurrency && (KNOWN_CURRENCIES as readonly string[]).includes(defaultCurrency)
      ? defaultCurrency
      : 'PLN';
  const formatCurrency = (value: number) => `${value.toLocaleString()} ${currency}`;

  return (
    <article className="rounded-2xl border border-outline-variant/20 bg-surface-container p-6 inner-glow transition-colors hover:border-outline-variant/40">
      <div className="flex flex-col gap-6 lg:flex-row lg:items-center lg:justify-between">
        <div className="min-w-0 flex-1">
          <h3 className="truncate font-headline-md text-[20px] font-semibold text-on-surface">
            {formatResearchTitle(research, t('research_untitled'))}
          </h3>
          <p className="mt-1 font-mono-sm text-[13px] text-on-surface-variant">
            {formatResearchSubtitle(research, t('research_no_details'), t('research_details_pending'), formatCurrency)}
          </p>
          <p className="mt-4 text-[13px] text-on-surface-variant">
            <span className="font-medium text-on-surface">{t('research_analysis_date')}</span>{' '}
            {formatAnalysisDate(research.updatedAt, language)}
          </p>
        </div>

        <div className="flex flex-col items-start gap-4 sm:flex-row sm:items-center lg:flex-col lg:items-end">
          {riskBadge ? (
            <span
              className={`inline-flex items-center gap-2 rounded-full border px-3 py-1.5 text-[13px] font-medium ${riskBadge.className}`}
            >
              <span className="material-symbols-outlined text-[18px]" aria-hidden="true">
                {riskBadge.icon}
              </span>
              {t(riskBadge.labelKey)}
            </span>
          ) : (
            <span className="inline-flex items-center gap-2 rounded-full border border-outline-variant/30 bg-surface-container-high px-3 py-1.5 text-[13px] font-medium text-on-surface-variant">
              <span className="material-symbols-outlined text-[18px]" aria-hidden="true">
                hourglass_empty
              </span>
              {research.status === 'analyzing'
                ? t('research_status_analyzing')
                : t('research_status_pending')}
            </span>
          )}

          <button
            type="button"
            disabled
            className="inline-flex items-center gap-2 text-[14px] font-medium text-primary/50 transition-colors"
            aria-disabled="true"
            title="Report view coming soon"
          >
            {t('research_view_report')}
            <span className="material-symbols-outlined text-[18px]" aria-hidden="true">
              arrow_forward
            </span>
          </button>
        </div>
      </div>
    </article>
  );
}
