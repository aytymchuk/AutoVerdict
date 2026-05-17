import { ClerkProvider as BaseClerkProvider } from '@clerk/clerk-react';
import type { ReactNode } from 'react';

const PUBLISHABLE_KEY = import.meta.env.VITE_CLERK_PUBLISHABLE_KEY;

if (!PUBLISHABLE_KEY) {
  console.warn("Missing Publishable Key. Ensure VITE_CLERK_PUBLISHABLE_KEY is set in your .env file.");
}

interface Props {
  children: ReactNode;
}

export function AppClerkProvider({ children }: Props) {
  return (
    <BaseClerkProvider publishableKey={PUBLISHABLE_KEY || 'missing_key'}>
      {children}
    </BaseClerkProvider>
  );
}
