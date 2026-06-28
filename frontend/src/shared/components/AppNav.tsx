import { useClerk, useUser } from '@clerk/clerk-react';
import { useEffect, useRef, useState } from 'react';
import { Link, NavLink } from 'react-router-dom';

const navLinkClassName = ({ isActive }: { isActive: boolean }) =>
  [
    'h-16 flex items-center px-4 text-[14px] font-medium transition-colors border-b-2',
    isActive
      ? 'text-primary border-primary'
      : 'text-on-surface-variant border-transparent hover:text-on-surface',
  ].join(' ');

function getUserInitial(user: ReturnType<typeof useUser>['user']) {
  if (!user) {
    return '?';
  }

  const fromName = user.firstName?.charAt(0) ?? user.lastName?.charAt(0);
  if (fromName) {
    return fromName.toUpperCase();
  }

  return user.primaryEmailAddress?.emailAddress?.charAt(0).toUpperCase() ?? '?';
}

export function AppNav() {
  const { user } = useUser();
  const { signOut } = useClerk();
  const [menuOpen, setMenuOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!menuOpen) {
      return;
    }

    function handlePointerDown(event: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setMenuOpen(false);
      }
    }

    document.addEventListener('mousedown', handlePointerDown);
    return () => document.removeEventListener('mousedown', handlePointerDown);
  }, [menuOpen]);

  async function handleSignOut() {
    setMenuOpen(false);
    await signOut({ redirectUrl: '/' });
  }

  return (
    <header className="fixed top-0 left-0 right-0 z-50 border-b border-outline-variant/20 bg-background/80 backdrop-blur-md">
      <div className="mx-auto flex h-16 max-w-[1280px] items-center justify-between px-gutter">
        <div className="flex items-center gap-8">
          <Link
            to="/"
            className="font-headline-md text-[24px] font-semibold tracking-tight text-on-surface transition-colors hover:text-primary"
          >
            AutoVerdikt
          </Link>

          <nav aria-label="Main navigation" className="hidden items-center gap-1 md:flex">
            <NavLink to="/home" className={navLinkClassName} end>
              My Research
            </NavLink>
            <NavLink to="/credits" className={navLinkClassName}>
              Credits
            </NavLink>
          </nav>
        </div>

        <div className="flex items-center gap-4">
          <div className="flex items-center gap-2 rounded-full border border-outline-variant/30 bg-surface-container px-3 py-1.5">
            <span className="h-2 w-2 rounded-full bg-primary" aria-hidden="true" />
            <span className="font-mono-sm text-[14px] text-on-surface">50</span>
            <span className="text-on-surface-variant text-[12px]">Add Credit</span>
          </div>

          <div className="relative" ref={menuRef}>
            <button
              type="button"
              onClick={() => setMenuOpen((open) => !open)}
              aria-expanded={menuOpen}
              aria-haspopup="menu"
              aria-label="Open account menu"
              className="flex items-center gap-1 rounded-full p-1 text-on-surface-variant transition-colors hover:text-on-surface focus:outline-none focus:ring-2 focus:ring-risk-medium"
            >
              <span className="flex h-8 w-8 items-center justify-center rounded-full bg-surface-container-high text-[14px] font-medium text-on-surface">
                {getUserInitial(user)}
              </span>
              <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
                expand_more
              </span>
            </button>

            {menuOpen && (
              <div
                role="menu"
                className="absolute right-0 top-full mt-2 min-w-[12rem] rounded-xl border border-outline-variant/20 bg-surface-container-high py-2 shadow-lg"
              >
                <Link
                  to="/profile"
                  role="menuitem"
                  onClick={() => setMenuOpen(false)}
                  className="flex w-full items-center gap-3 px-4 py-2.5 text-left text-[14px] text-on-surface transition-colors hover:bg-surface-variant/50"
                >
                  <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
                    person
                  </span>
                  Profile
                </Link>
                <button
                  type="button"
                  role="menuitem"
                  onClick={() => setMenuOpen(false)}
                  className="flex w-full items-center gap-3 px-4 py-2.5 text-left text-[14px] text-on-surface transition-colors hover:bg-surface-variant/50"
                >
                  <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
                    person_add
                  </span>
                  Invite a friend
                </button>
                <button
                  type="button"
                  role="menuitem"
                  onClick={() => setMenuOpen(false)}
                  className="flex w-full items-center gap-3 px-4 py-2.5 text-left text-[14px] text-on-surface transition-colors hover:bg-surface-variant/50"
                >
                  <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
                    help
                  </span>
                  Help
                </button>
                <div className="my-2 border-t border-outline-variant/20" />
                <button
                  type="button"
                  role="menuitem"
                  onClick={handleSignOut}
                  className="flex w-full items-center gap-3 px-4 py-2.5 text-left text-[14px] text-risk-high transition-colors hover:bg-surface-variant/50"
                >
                  <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
                    logout
                  </span>
                  Sign out
                </button>
              </div>
            )}
          </div>
        </div>
      </div>
    </header>
  );
}
