import { useContext } from 'react';
import { UserStatusContext, type UserStatusContextValue } from '../context/userStatusContext';

export function useUserStatus(): UserStatusContextValue {
  const ctx = useContext(UserStatusContext);
  if (!ctx) throw new Error('useUserStatus must be used inside UserStatusProvider');
  return ctx;
}
