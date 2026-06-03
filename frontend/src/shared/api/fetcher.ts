import { useAuth } from '@clerk/clerk-react';
import { useCallback } from 'react';
import { ApiError, type ProblemDetails } from './problemDetails';

export function useApi() {
  const { getToken } = useAuth();

  const fetchWithAuth = useCallback(
    async (url: string, options: RequestInit = {}) => {
      const token = await getToken();

      const headers = new Headers(options.headers);
      if (token) {
        headers.set('Authorization', `Bearer ${token}`);
      }
      if (options.body && !(options.body instanceof FormData) && !headers.has('Content-Type')) {
        headers.set('Content-Type', 'application/json');
      }

      const response = await fetch(url, {
        ...options,
        headers,
      });

      if (!response.ok) {
        const contentType = response.headers.get('Content-Type') ?? '';
        if (contentType.includes('application/problem+json')) {
          const rawBody = await response.text();
          let problemDetails: ProblemDetails | undefined;
          if (rawBody) {
            try {
              problemDetails = JSON.parse(rawBody) as ProblemDetails;
            } catch {
              problemDetails = undefined;
            }
          }
          throw new ApiError(
            response.status,
            problemDetails?.title ?? `API error: ${response.status}`,
            problemDetails
          );
        }
        throw new ApiError(response.status, `API error: ${response.statusText}`);
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
