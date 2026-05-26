import { SignIn } from '@clerk/clerk-react';

export function AuthPage() {
  return (
    <div className="min-h-screen bg-surface-container-lowest flex items-center justify-center">
      <SignIn />
    </div>
  );
}
