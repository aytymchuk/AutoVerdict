import { useEffect, useState, useCallback, type ReactNode } from 'react';
import { useAuth } from '@clerk/clerk-react';
import { useUsersApi, type WhitelistStatus } from '../api/users';
import { UserStatusContext, type UserStatus } from './userStatusContext';

export function UserStatusProvider({ children }: { children: ReactNode }) {
  const { isSignedIn, isLoaded } = useAuth();
  const usersApi = useUsersApi();
  const [status, setStatus] = useState<UserStatus>('loading');
  const [email, setEmail] = useState<string | null>(null);
  const [whitelistStatus, setWhitelistStatus] = useState<WhitelistStatus>('none');
  const [trigger, setTrigger] = useState(0);

  const refetch = useCallback(() => setTrigger(n => n + 1), []);

  useEffect(() => {
    if (!isLoaded) return;

    let cancelled = false;

    (async () => {
      if (!isSignedIn) {
        if (!cancelled) {
          setEmail(null);
          setWhitelistStatus('none');
          setStatus('unauthenticated');
        }
        return;
      }

      if (!cancelled) setStatus('loading');

      try {
        const user = await usersApi.getMe();
        if (cancelled) return;

        if (!user) {
          setEmail(null);
          setWhitelistStatus('none');
          setStatus('unregistered');
          return;
        }

        setEmail(user.email);
        setWhitelistStatus(user.whitelistStatus);
        setStatus(user.isWhitelisted ? 'registered' : 'not_whitelisted');
      } catch {
        if (!cancelled) setStatus('loading');
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [isLoaded, isSignedIn, usersApi, trigger]);

  return (
    <UserStatusContext.Provider value={{ status, email, whitelistStatus, refetch }}>
      {children}
    </UserStatusContext.Provider>
  );
}
