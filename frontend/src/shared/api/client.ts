import ky, { type KyInstance, type Options } from 'ky';
import { fetchEventSource, type EventSourceMessage } from '@microsoft/fetch-event-source';

export type GetToken = () => Promise<string | null>;

export interface StreamOptions {
  signal?: AbortSignal;
  onMessage: (event: EventSourceMessage) => void;
  onError?: (err: unknown) => number | null | undefined;
}

export class ApiClient {
  private readonly http: KyInstance;
  private readonly getToken: GetToken;

  constructor(getToken: GetToken) {
    this.getToken = getToken;
    this.http = ky.create({
      baseUrl: '/api/',
      throwHttpErrors: false,
      retry: {
        limit: 3,
        methods: ['get'],
        statusCodes: [408, 429, 502, 503, 504],
        backoffLimit: 10_000,
      },
      hooks: {
        beforeRequest: [
          async ({ request }) => {
            const token = await getToken();
            if (token) {
              request.headers.set('Authorization', `Bearer ${token}`);
            }
          },
        ],
      },
    });
  }

  protected request(path: string, options?: Options): Promise<Response> {
    return this.http(path, options);
  }

  protected async stream(path: string, options: StreamOptions): Promise<void> {
    const token = await this.getToken();
    await fetchEventSource(`/api/${path}`, {
      signal: options.signal,
      headers: token ? { Authorization: `Bearer ${token}` } : undefined,
      onmessage: options.onMessage,
      onerror: options.onError,
    });
  }
}
