import type { ResearchCarSummaryDto, ResearchListItemDto, RiskLevel } from '../../shared/api/research';
import type { TranslationKey } from '../../shared/lib/i18n/locales';

export function formatResearchTitle(
  research: ResearchListItemDto,
  untitledLabel: string,
): string {
  if (research.name?.trim()) {
    return research.name;
  }

  const { car } = research;
  if (!car) {
    return untitledLabel;
  }

  const parts = [car.make, car.model, car.year?.toString()].filter(Boolean);
  return parts.length > 0 ? parts.join(' ') : untitledLabel;
}

export function formatResearchSubtitle(
  research: ResearchListItemDto,
  noDetailsLabel: string,
  detailsPendingLabel: string,
  formatCurrency?: (value: number) => string,
): string {
  const { car } = research;
  if (!car) {
    return noDetailsLabel;
  }

  const parts: string[] = [];
  if (car.mileageKm != null) {
    parts.push(`${car.mileageKm.toLocaleString()} km`);
  }
  if (car.price != null) {
    parts.push(formatCurrency ? formatCurrency(car.price) : `${car.price.toLocaleString()} PLN`);
  }

  return parts.length > 0 ? parts.join(' · ') : detailsPendingLabel;
}

export function formatAnalysisDate(updatedAt: string, language: string): string {
  const locale = language === 'pl' ? 'pl-PL' : language === 'uk' ? 'uk-UA' : 'en-US';
  return new Date(updatedAt).toLocaleDateString(locale, {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  });
}

export interface RiskBadgeStyle {
  labelKey: TranslationKey;
  icon: string;
  className: string;
}

export function getRiskBadgeStyle(riskLevel: RiskLevel | null): RiskBadgeStyle | null {
  if (!riskLevel) {
    return null;
  }

  switch (riskLevel) {
    case 'low':
      return {
        labelKey: 'research_risk_low',
        icon: 'check_circle',
        className: 'text-risk-low bg-risk-low/10 border-risk-low/20',
      };
    case 'medium':
      return {
        labelKey: 'research_risk_medium',
        icon: 'report_problem',
        className: 'text-risk-medium bg-risk-medium/10 border-risk-medium/20',
      };
    case 'high':
      return {
        labelKey: 'research_risk_high',
        icon: 'dangerous',
        className: 'text-risk-high bg-risk-high/10 border-risk-high/20',
      };
    default: {
      const _exhaustive: never = riskLevel;
      return _exhaustive;
    }
  }
}

export function buildCarTitle(car: ResearchCarSummaryDto | null | undefined): string {
  if (!car) {
    return '';
  }

  return [car.make, car.model, car.year?.toString()].filter(Boolean).join(' ');
}
