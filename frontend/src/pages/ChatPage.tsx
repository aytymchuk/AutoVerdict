import { AIChat } from '../features/AIChat/AIChat';
import { SignedIn, SignedOut, SignInButton } from '@clerk/clerk-react';
import { useTranslation } from '../shared/lib/i18n';
import { DashboardLayout } from '../shared/components/DashboardLayout';

export function ChatPage() {
  const { t } = useTranslation();

  return (
    <DashboardLayout>
      <div className="mx-auto max-w-4xl space-y-6 p-gutter">
        <div className="text-center">
          <h2 className="font-headline-md text-[24px] font-semibold text-on-surface">{t('title')}</h2>
          <p className="mt-2 text-on-surface-variant">{t('description')}</p>
        </div>

        <SignedIn>
          <AIChat />
        </SignedIn>

        <SignedOut>
          <div className="rounded-lg border border-outline-variant/20 bg-surface-container p-8 text-center">
            <h3 className="mb-4 text-lg font-medium text-on-surface">{t('signInToContinue')}</h3>
            <SignInButton mode="modal">
              <button className="rounded-md bg-primary px-6 py-2 text-on-primary transition-colors hover:bg-primary/90">
                {t('signIn')}
              </button>
            </SignInButton>
          </div>
        </SignedOut>
      </div>
    </DashboardLayout>
  );
}
