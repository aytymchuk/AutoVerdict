import { createContext } from 'react';

export type UserStatus = 'loading' | 'unauthenticated' | 'unregistered' | 'registered';

export interface UserStatusContextValue {
  status: UserStatus;
  refetch: () => void;
}

export const UserStatusContext = createContext<UserStatusContextValue | null>(null);
