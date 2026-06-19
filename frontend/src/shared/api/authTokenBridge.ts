import type { GetToken } from './client';

let getTokenImpl: GetToken = async () => null;

export function registerGetToken(getter: GetToken): void {
  getTokenImpl = getter;
}

export const resolveAuthToken: GetToken = () => getTokenImpl();
