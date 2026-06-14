import { createContext } from 'react';
import type { WhitelistStatus } from '../api/users';

export type UserStatus =
  | 'loading'
  | 'unauthenticated'
  | 'unregistered'
  | 'not_whitelisted'
  | 'registered';

export interface UserStatusContextValue {
  status: UserStatus;
  email: string | null;
  whitelistStatus: WhitelistStatus;
  refetch: () => void;
}

export const UserStatusContext = createContext<UserStatusContextValue | null>(null);
