import { createContext, useEffect, useState, useCallback, type ReactNode } from 'react';
import { useAuth } from '@clerk/clerk-react';

export type UserStatus = 'loading' | 'unauthenticated' | 'unregistered' | 'registered';

export interface UserStatusContextValue {
  status: UserStatus;
  refetch: () => void;
}

export const UserStatusContext = createContext<UserStatusContextValue | null>(null);

export function UserStatusProvider({ children }: { children: ReactNode }) {
  const { getToken, isSignedIn, isLoaded } = useAuth();
  const [status, setStatus] = useState<UserStatus>('loading');
  const [trigger, setTrigger] = useState(0);

  const refetch = useCallback(() => setTrigger(n => n + 1), []);

  useEffect(() => {
    if (!isLoaded) return;

    let cancelled = false;

    (async () => {
      if (!isSignedIn) {
        if (!cancelled) setStatus('unauthenticated');
        return;
      }

      if (!cancelled) setStatus('loading');

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
