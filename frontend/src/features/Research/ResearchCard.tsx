import type { ResearchListItemDto } from '../../shared/api/research';
import {
  formatAnalysisDate,
  formatResearchSubtitle,
  formatResearchTitle,
  getRiskBadgeStyle,
} from './researchDisplay';

interface ResearchCardProps {
  research: ResearchListItemDto;
}

export function ResearchCard({ research }: ResearchCardProps) {
  const riskBadge = getRiskBadgeStyle(research.riskLevel);

  return (
    <article className="rounded-2xl border border-outline-variant/20 bg-surface-container p-6 inner-glow transition-colors hover:border-outline-variant/40">
      <div className="flex flex-col gap-6 lg:flex-row lg:items-center lg:justify-between">
        <div className="min-w-0 flex-1">
          <h3 className="truncate font-headline-md text-[20px] font-semibold text-on-surface">
            {formatResearchTitle(research)}
          </h3>
          <p className="mt-1 font-mono-sm text-[13px] text-on-surface-variant">
            {formatResearchSubtitle(research)}
          </p>
          <p className="mt-4 text-[13px] text-on-surface-variant">
            <span className="font-medium text-on-surface">Analysis Date</span>{' '}
            {formatAnalysisDate(research.updatedAt)}
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
              {riskBadge.label}
            </span>
          ) : (
            <span className="inline-flex items-center gap-2 rounded-full border border-outline-variant/30 bg-surface-container-high px-3 py-1.5 text-[13px] font-medium text-on-surface-variant">
              <span className="material-symbols-outlined text-[18px]" aria-hidden="true">
                hourglass_empty
              </span>
              {research.status === 'analyzing' ? 'Analyzing' : 'Pending'}
            </span>
          )}

          <button
            type="button"
            disabled
            className="inline-flex items-center gap-2 text-[14px] font-medium text-primary/50 transition-colors"
            aria-disabled="true"
            title="Report view coming soon"
          >
            View Report
            <span className="material-symbols-outlined text-[18px]" aria-hidden="true">
              arrow_forward
            </span>
          </button>
        </div>
      </div>
    </article>
  );
}
