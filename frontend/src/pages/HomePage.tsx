import { useClerk, useUser } from '@clerk/clerk-react';
import { Link } from 'react-router-dom';

export function HomePage() {
  const { user } = useUser();
  const { signOut } = useClerk();

  async function handleSignOut() {
    await signOut({ redirectUrl: '/' });
  }

  return (
    <div className="min-h-screen bg-surface-container-lowest text-on-surface flex flex-col">
      <header className="border-b border-outline-variant/20 bg-background/80 backdrop-blur-md px-6 py-4 flex items-center justify-between">
        <Link
          to="/"
          className="font-headline-md text-[24px] font-semibold text-on-surface tracking-tight hover:text-primary transition-colors"
        >
          AutoVerdikt
        </Link>
        <div className="flex items-center gap-4">
          {user && (
            <span className="text-on-surface-variant text-[14px]">
              {user.primaryEmailAddress?.emailAddress}
            </span>
          )}
          <button
            onClick={handleSignOut}
            className="text-on-surface-variant hover:text-primary text-[14px] transition-colors"
          >
            Sign out
          </button>
        </div>
      </header>

      <main className="flex-1 flex flex-col items-center justify-center gap-4 px-4">
        <span className="material-symbols-outlined text-primary text-[64px]">dashboard</span>
        <h1 className="font-headline-md text-[32px] font-semibold text-on-surface tracking-tight">
          Dashboard
        </h1>
        <p className="text-on-surface-variant text-[16px] text-center max-w-[24rem]">
          Welcome{user?.firstName ? `, ${user.firstName}` : ''}! Your dashboard is coming soon.
        </p>
      </main>
    </div>
  );
}
