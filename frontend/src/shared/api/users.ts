import { useAuth } from '@clerk/clerk-react';
import { useMemo } from 'react';
import { ApiClient } from './client';

export interface UserAccountDto {
  id: string;
  name: string;
  email: string;
  registeredAt: string;
}

export interface IUsersApi {
  /** Returns null when the user is authenticated but not registered (404). */
  getMe(): Promise<UserAccountDto | null>;
  register(name: string, email: string): Promise<UserAccountDto>;
}

export class UsersApiClient extends ApiClient implements IUsersApi {
  async getMe(): Promise<UserAccountDto | null> {
    const res = await this.request('users/me');
    if (res.status === 404) {
      return null;
    }
    if (!res.ok) {
      const body = await res.text();
      throw new Error(body || `API error: ${res.statusText}`);
    }
    return res.json() as Promise<UserAccountDto>;
  }

  async register(name: string, email: string): Promise<UserAccountDto> {
    const res = await this.request('users/register', {
      method: 'post',
      json: { name, email },
    });
    if (!res.ok) {
      const body = await res.text();
      throw new Error(body || `API error: ${res.statusText}`);
    }
    return res.json() as Promise<UserAccountDto>;
  }
}

export function useUsersApi(): IUsersApi {
  const { getToken } = useAuth();
  return useMemo(() => new UsersApiClient(() => getToken()), [getToken]);
}
