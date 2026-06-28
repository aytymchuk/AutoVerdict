interface ResearchEmptyStateProps {
  onStartResearch: () => void;
}

const inputMethods = [
  { icon: 'assignment', label: 'Enter data' },
  { icon: 'keyboard', label: 'Paste text' },
  { icon: 'photo_camera', label: 'Photos' },
] as const;

export function ResearchEmptyState({ onStartResearch }: ResearchEmptyStateProps) {
  return (
    <div className="mx-auto flex w-full max-w-[720px] flex-col items-center px-gutter py-16 text-center">
      <div className="mb-8 flex h-24 w-24 items-center justify-center rounded-full border border-primary/20 bg-primary/10">
        <span className="material-symbols-outlined text-[48px] text-primary" aria-hidden="true">
          directions_car
        </span>
      </div>

      <h1 className="font-headline-md text-[32px] font-semibold tracking-tight text-on-surface">
        You don&apos;t have any research yet
      </h1>
      <p className="mt-3 max-w-[28rem] text-[16px] text-on-surface-variant">
        Check your first listing in 3 ways
      </p>

      <div className="mt-10 grid w-full grid-cols-1 gap-4 sm:grid-cols-3">
        {inputMethods.map((method) => (
          <div
            key={method.label}
            className="flex flex-col items-center gap-3 rounded-2xl border border-outline-variant/20 bg-surface-container px-6 py-8 inner-glow"
          >
            <span
              className="material-symbols-outlined text-[28px] text-on-surface-variant"
              aria-hidden="true"
            >
              {method.icon}
            </span>
            <span className="text-[14px] font-medium text-on-surface">{method.label}</span>
          </div>
        ))}
      </div>

      <button
        type="button"
        onClick={onStartResearch}
        className="mt-10 inline-flex items-center gap-2 rounded-full bg-primary px-8 py-3 text-[15px] font-semibold text-on-primary transition-colors hover:bg-primary-fixed-dim focus:outline-none focus:ring-2 focus:ring-risk-medium/50"
      >
        <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
          add
        </span>
        Start your first research
      </button>

      <p className="mt-8 flex items-center gap-2 text-[14px] text-on-surface-variant">
        <span className="text-primary" aria-hidden="true">
          ✦
        </span>
        You received 50 starter credits as a beta participant
      </p>
    </div>
  );
}
