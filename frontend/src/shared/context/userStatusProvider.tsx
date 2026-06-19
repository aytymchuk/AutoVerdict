import { useEffect, useState, useCallback, type ReactNode } from 'react';
import { useAuth } from '@clerk/clerk-react';
import { useUsersApi, type WhitelistStatus } from '../api/users';
import { UserStatusContext, type UserStatus } from './userStatusContext';

interface ResolvedUserStatus {
  status: UserStatus;
  email: string | null;
  whitelistStatus: WhitelistStatus;
}

export function UserStatusProvider({ children }: { children: ReactNode }) {
  const { isSignedIn, isLoaded } = useAuth();
  const usersApi = useUsersApi();
  const [status, setStatus] = useState<UserStatus>('loading');
  const [email, setEmail] = useState<string | null>(null);
  const [whitelistStatus, setWhitelistStatus] = useState<WhitelistStatus>('none');

  const resolveStatus = useCallback(async (): Promise<ResolvedUserStatus> => {
    if (!isLoaded) {
      return { status: 'loading', email: null, whitelistStatus: 'none' };
    }

    if (!isSignedIn) {
      return { status: 'unauthenticated', email: null, whitelistStatus: 'none' };
    }

    try {
      const user = await usersApi.getMe();

      if (!user) {
        return { status: 'unregistered', email: null, whitelistStatus: 'none' };
      }

      return {
        status: user.isWhitelisted ? 'registered' : 'not_whitelisted',
        email: user.email,
        whitelistStatus: user.whitelistStatus,
      };
    } catch {
      return { status: 'error', email: null, whitelistStatus: 'none' };
    }
  }, [isLoaded, isSignedIn, usersApi]);

  const applyResolved = useCallback((resolved: ResolvedUserStatus) => {
    setStatus(resolved.status);
    setEmail(resolved.email);
    setWhitelistStatus(resolved.whitelistStatus);
  }, []);

  const refetch = useCallback(async (): Promise<UserStatus> => {
    if (!isLoaded) {
      return 'loading';
    }

    if (!isSignedIn) {
      applyResolved({ status: 'unauthenticated', email: null, whitelistStatus: 'none' });
      return 'unauthenticated';
    }

    setStatus('loading');
    const resolved = await resolveStatus();
    applyResolved(resolved);
    return resolved.status;
  }, [applyResolved, isLoaded, isSignedIn, resolveStatus]);

  useEffect(() => {
    let cancelled = false;

    void resolveStatus().then(resolved => {
      if (!cancelled) {
        applyResolved(resolved);
      }
    });

    return () => {
      cancelled = true;
    };
  }, [applyResolved, resolveStatus]);

  return (
    <UserStatusContext.Provider value={{ status, email, whitelistStatus, refetch }}>
      {children}
    </UserStatusContext.Provider>
  );
}
