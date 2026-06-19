import { useAuth } from '@clerk/clerk-react';
import { useMemo } from 'react';
import { ApiClient } from './client';

export type WaitlistSubmitResult = 'created' | 'already_submitted';

export interface IWaitlistApi {
  submitRequest(about?: string): Promise<WaitlistSubmitResult>;
}

export class WaitlistApiClient extends ApiClient implements IWaitlistApi {
  async submitRequest(about?: string): Promise<WaitlistSubmitResult> {
    const res = await this.request('waitlist-requests', {
      method: 'post',
      json: { about: about ?? null },
    });

    if (res.status === 409) {
      return 'already_submitted';
    }

    if (!res.ok) {
      const body = await res.text();
      throw new Error(body || `API error: ${res.statusText}`);
    }

    return 'created';
  }
}

export function useWaitlistApi(): IWaitlistApi {
  const { getToken } = useAuth();
  return useMemo(() => new WaitlistApiClient(() => getToken()), [getToken]);
}
