import type { ResearchCarSummaryDto, ResearchListItemDto, RiskLevel } from '../../shared/api/research';

export function formatResearchTitle(research: ResearchListItemDto): string {
  if (research.name?.trim()) {
    return research.name;
  }

  const { car } = research;
  if (!car) {
    return 'Untitled research';
  }

  const parts = [car.make, car.model, car.year?.toString()].filter(Boolean);
  return parts.length > 0 ? parts.join(' ') : 'Untitled research';
}

export function formatResearchSubtitle(research: ResearchListItemDto): string {
  const { car } = research;
  if (!car) {
    return 'No vehicle details';
  }

  const parts: string[] = [];
  if (car.mileageKm != null) {
    parts.push(`${car.mileageKm.toLocaleString()} km`);
  }
  if (car.price != null) {
    parts.push(`${car.price.toLocaleString()} PLN`);
  }

  return parts.length > 0 ? parts.join(' · ') : 'Vehicle details pending';
}

export function formatAnalysisDate(updatedAt: string): string {
  return new Date(updatedAt).toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  });
}

export interface RiskBadgeStyle {
  label: string;
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
        label: 'Low Risk',
        icon: 'check_circle',
        className: 'text-risk-low bg-risk-low/10 border-risk-low/20',
      };
    case 'medium':
      return {
        label: 'Medium Risk',
        icon: 'report_problem',
        className: 'text-risk-medium bg-risk-medium/10 border-risk-medium/20',
      };
    case 'high':
      return {
        label: 'High Risk',
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
