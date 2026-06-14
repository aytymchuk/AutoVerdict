import { Navigate } from 'react-router-dom';
import type { ReactNode } from 'react';
import { useUserStatus } from '../hooks/useUserStatus';

export function LoadingScreen() {
  return (
    <div className="min-h-screen bg-surface-container-lowest flex items-center justify-center">
      <span className="material-symbols-outlined text-primary animate-spin text-[48px]">progress_activity</span>
    </div>
  );
}

/** Requires auth + completed registration + whitelist access. */
export function ProtectedRoute({ children }: { children: ReactNode }) {
  const { status } = useUserStatus();

  if (status === 'loading') return <LoadingScreen />;
  if (status === 'unauthenticated') return <Navigate to="/" replace />;
  if (status === 'unregistered') return <Navigate to="/register" replace />;
  if (status === 'not_whitelisted') return <Navigate to="/waiting" replace />;

  return <>{children}</>;
}

/** Requires auth but NOT registration (for the /register route itself). */
export function RegisterRoute({ children }: { children: ReactNode }) {
  const { status } = useUserStatus();

  if (status === 'loading') return <LoadingScreen />;
  if (status === 'unauthenticated') return <Navigate to="/auth" replace />;
  if (status === 'registered') return <Navigate to="/home" replace />;
  if (status === 'not_whitelisted') return <Navigate to="/waiting" replace />;

  return <>{children}</>;
}

/** Waiting page — authenticated users without whitelist access. */
export function WhitelistRoute({ children }: { children: ReactNode }) {
  const { status } = useUserStatus();

  if (status === 'loading') return <LoadingScreen />;
  if (status === 'unauthenticated') return <Navigate to="/auth" replace />;
  if (status === 'unregistered') return <Navigate to="/register" replace />;
  if (status === 'registered') return <Navigate to="/home" replace />;

  return <>{children}</>;
}
