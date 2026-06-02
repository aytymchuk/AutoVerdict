import { useEffect, useState, useCallback, type ReactNode } from 'react';
import { useAuth } from '@clerk/clerk-react';
import { useUsersApi } from '../api/users';
import { UserStatusContext, type UserStatus } from './userStatusContext';

export function UserStatusProvider({ children }: { children: ReactNode }) {
  const { isSignedIn, isLoaded } = useAuth();
  const usersApi = useUsersApi();
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
        const user = await usersApi.getMe();
        if (!cancelled) {
          setStatus(user ? 'registered' : 'unregistered');
        }
      } catch {
        if (!cancelled) setStatus('loading');
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [isLoaded, isSignedIn, usersApi, trigger]);

  return (
    <UserStatusContext.Provider value={{ status, refetch }}>
      {children}
    </UserStatusContext.Provider>
  );
}
