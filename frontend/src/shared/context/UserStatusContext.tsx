import { createContext, useContext, useEffect, useState, useCallback, type ReactNode } from 'react';
import { useAuth } from '@clerk/clerk-react';

type UserStatus = 'loading' | 'unauthenticated' | 'unregistered' | 'registered';

interface UserStatusContextValue {
  status: UserStatus;
  refetch: () => void;
}

const UserStatusContext = createContext<UserStatusContextValue | null>(null);

export function UserStatusProvider({ children }: { children: ReactNode }) {
  const { getToken, isSignedIn, isLoaded } = useAuth();
  const [status, setStatus] = useState<UserStatus>('loading');
  const [trigger, setTrigger] = useState(0);

  const refetch = useCallback(() => setTrigger(n => n + 1), []);

  useEffect(() => {
    if (!isLoaded) return;

    if (!isSignedIn) {
      setStatus('unauthenticated');
      return;
    }

    let cancelled = false;
    setStatus('loading');

    (async () => {
      try {
        const token = await getToken();
        const res = await fetch('/api/users/me', {
          headers: token ? { Authorization: `Bearer ${token}` } : {},
        });
        if (!cancelled) {
          setStatus(res.ok ? 'registered' : res.status === 404 ? 'unregistered' : 'loading');
        }
      } catch {
        if (!cancelled) setStatus('loading');
      }
    })();

    return () => { cancelled = true; };
  }, [isLoaded, isSignedIn, getToken, trigger]);

  return (
    <UserStatusContext.Provider value={{ status, refetch }}>
      {children}
    </UserStatusContext.Provider>
  );
}

export function useUserStatus(): UserStatusContextValue {
  const ctx = useContext(UserStatusContext);
  if (!ctx) throw new Error('useUserStatus must be used inside UserStatusProvider');
  return ctx;
}
