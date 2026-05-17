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

      return response.json();
    },
    [getToken]
  );

  return { fetchWithAuth };
}
