import { Navigate } from 'react-router-dom';
import type { ReactNode } from 'react';
import { useUserStatus } from '../hooks/useUserStatus';
import { useTranslation } from '../lib/i18n';

export function LoadingScreen() {
  return (
    <div className="min-h-screen bg-surface-container-lowest flex items-center justify-center">
      <span className="material-symbols-outlined text-primary animate-spin text-[48px]">progress_activity</span>
    </div>
  );
}

function StatusErrorScreen({ onRetry }: { onRetry: () => void }) {
  const { t } = useTranslation();

  return (
    <div className="min-h-screen bg-surface-container-lowest flex flex-col items-center justify-center gap-md px-gutter text-center">
      <p className="font-body-md text-on-surface max-w-md">{t('common_status_error')}</p>
      <button
        type="button"
        onClick={onRetry}
        className="bg-primary-container text-surface-container-lowest font-body-md font-medium rounded-full py-sm px-lg hover:bg-primary transition-colors"
      >
        {t('common_retry')}
      </button>
    </div>
  );
}

function useRouteGuard() {
  const { status, refetch } = useUserStatus();

  if (status === 'loading') return { element: <LoadingScreen /> as ReactNode };
  if (status === 'error') {
    return {
      element: <StatusErrorScreen onRetry={() => void refetch()} /> as ReactNode,
    };
  }

  return { status };
}

/** Requires auth + completed registration + whitelist access. */
export function ProtectedRoute({ children }: { children: ReactNode }) {
  const guard = useRouteGuard();
  if ('element' in guard) return guard.element;

  const { status } = guard;
  if (status === 'unauthenticated') return <Navigate to="/" replace />;
  if (status === 'unregistered') return <Navigate to="/register" replace />;
  if (status === 'not_whitelisted') return <Navigate to="/waiting" replace />;

  return <>{children}</>;
}

/** Requires auth but NOT registration (for the /register route itself). */
export function RegisterRoute({ children }: { children: ReactNode }) {
  const guard = useRouteGuard();
  if ('element' in guard) return guard.element;

  const { status } = guard;
  if (status === 'unauthenticated') return <Navigate to="/auth" replace />;
  if (status === 'registered') return <Navigate to="/home" replace />;
  if (status === 'not_whitelisted') return <Navigate to="/waiting" replace />;

  return <>{children}</>;
}

/** Waiting page — authenticated users without whitelist access. */
export function WhitelistRoute({ children }: { children: ReactNode }) {
  const guard = useRouteGuard();
  if ('element' in guard) return guard.element;

  const { status } = guard;
  if (status === 'unauthenticated') return <Navigate to="/auth" replace />;
  if (status === 'unregistered') return <Navigate to="/register" replace />;
  if (status === 'registered') return <Navigate to="/home" replace />;

  return <>{children}</>;
}
