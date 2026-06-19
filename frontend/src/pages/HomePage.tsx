import { useUser } from '@clerk/clerk-react';
import { DashboardLayout } from '../shared/components/DashboardLayout';

export function HomePage() {
  const { user } = useUser();

  return (
    <DashboardLayout>
      <div className="flex flex-1 flex-col items-center justify-center gap-4 px-4 py-12">
        <span className="material-symbols-outlined text-primary text-[64px]">dashboard</span>
        <h1 className="font-headline-md text-[32px] font-semibold text-on-surface tracking-tight">
          Dashboard
        </h1>
        <p className="max-w-[24rem] text-center text-[16px] text-on-surface-variant">
          Welcome{user?.firstName ? `, ${user.firstName}` : ''}! Your dashboard is coming soon.
        </p>
      </div>
    </DashboardLayout>
  );
}
