import type { ReactNode } from 'react';
import { AppNav } from './AppNav';

interface DashboardLayoutProps {
  children: ReactNode;
}

export function DashboardLayout({ children }: DashboardLayoutProps) {
  return (
    <div className="flex min-h-screen flex-col bg-surface-container-lowest text-on-surface">
      <AppNav />
      <main className="flex-1 pt-16">{children}</main>
    </div>
  );
}
