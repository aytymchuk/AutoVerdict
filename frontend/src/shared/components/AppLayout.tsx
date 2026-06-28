import type { ReactNode } from 'react';
import { AppNav } from './AppNav';

interface AppLayoutProps {
  children: ReactNode;
}

export function AppLayout({ children }: AppLayoutProps) {
  return (
    <div className="flex min-h-screen flex-col bg-surface-container-lowest text-on-surface">
      <AppNav />
      <main className="flex-1 pt-16">{children}</main>
    </div>
  );
}
