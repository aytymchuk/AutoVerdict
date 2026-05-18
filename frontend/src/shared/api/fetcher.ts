import { useAuth } from '@clerk/clerk-react';
import { useCallback } from 'react';

export function useApi() {
  const { getToken } = useAuth();

  const fetchWithAuth = useCallback(
    async (url: string, options: RequestInit = {}) => {
      const token = await getToken();

      const headers = new Headers(options.headers);
      if (token) {
        headers.set('Authorization', `Bearer ${token}`);
      }
      headers.set('Content-Type', 'application/json');

      const response = await fetch(url, {
        ...options,
        headers,
      });

      if (!response.ok) {
        throw new Error(`API error: ${response.statusText}`);
      }

      if (response.status === 204 || response.status === 205) {
        return null;
      }

      const text = await response.text();
      return text ? JSON.parse(text) : null;
    },
    [getToken]
  );

  return { fetchWithAuth };
}
