import { useEffect, useState, useCallback, useContext, type ReactNode } from 'react';
import { useAuth } from '@clerk/clerk-react';
import { useUsersApi, type WhitelistStatus } from '../api/users';
import { UserStatusContext, type UserStatus } from './userStatusContext';
import { I18nContext } from '../lib/i18n/context';
import type { Language } from '../lib/i18n/types';

interface ResolvedUserStatus {
  status: UserStatus;
  email: string | null;
  whitelistStatus: WhitelistStatus;
  language: string | null;
  defaultCurrency: string | null;
}

export function UserStatusProvider({ children }: { children: ReactNode }) {
  const { isSignedIn, isLoaded } = useAuth();
  const usersApi = useUsersApi();
  const i18n = useContext(I18nContext);
  const [status, setStatus] = useState<UserStatus>('loading');
  const [email, setEmail] = useState<string | null>(null);
  const [whitelistStatus, setWhitelistStatus] = useState<WhitelistStatus>('none');
  const [language, setLanguage] = useState<string | null>(null);
  const [defaultCurrency, setDefaultCurrency] = useState<string | null>(null);

  const resolveStatus = useCallback(async (): Promise<ResolvedUserStatus> => {
    if (!isLoaded) {
      return { status: 'loading', email: null, whitelistStatus: 'none', language: null, defaultCurrency: null };
    }

    if (!isSignedIn) {
      return { status: 'unauthenticated', email: null, whitelistStatus: 'none', language: null, defaultCurrency: null };
    }

    try {
      const user = await usersApi.getMe();

      if (!user) {
        return { status: 'unregistered', email: null, whitelistStatus: 'none', language: null, defaultCurrency: null };
      }

      return {
        status: user.isWhitelisted ? 'registered' : 'not_whitelisted',
        email: user.email,
        whitelistStatus: user.whitelistStatus,
        language: user.language ?? null,
        defaultCurrency: user.defaultCurrency ?? null,
      };
    } catch {
      return { status: 'error', email: null, whitelistStatus: 'none', language: null, defaultCurrency: null };
    }
  }, [isLoaded, isSignedIn, usersApi]);

  const applyResolved = useCallback((resolved: ResolvedUserStatus) => {
    setStatus(resolved.status);
    setEmail(resolved.email);
    setWhitelistStatus(resolved.whitelistStatus);
    setLanguage(resolved.language);
    setDefaultCurrency(resolved.defaultCurrency);
    if (resolved.language && i18n) {
      i18n.setLanguage(resolved.language as Language);
    }
  }, [i18n]);

  const refetch = useCallback(async (): Promise<UserStatus> => {
    if (!isLoaded) {
      return 'loading';
    }

    if (!isSignedIn) {
      applyResolved({ status: 'unauthenticated', email: null, whitelistStatus: 'none', language: null, defaultCurrency: null });
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
    <UserStatusContext.Provider value={{ status, email, whitelistStatus, language, defaultCurrency, refetch }}>
      {children}
    </UserStatusContext.Provider>
  );
}
