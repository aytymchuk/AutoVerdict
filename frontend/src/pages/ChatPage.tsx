import { AIChat } from '../features/AIChat/AIChat';
import { SignedIn, SignedOut, SignInButton } from '@clerk/clerk-react';
import { useTranslation } from '../shared/lib/i18n';
import type { Language } from '../shared/lib/i18n';

export function ChatPage() {
  const { t, language, setLanguage } = useTranslation();

  return (
    <div className="max-w-4xl mx-auto space-y-6">
      <div className="flex justify-end gap-2 text-xs">
        {(['en', 'pl', 'uk'] as Language[]).map((lang) => (
          <button
            key={lang}
            onClick={() => setLanguage(lang)}
            className={`px-3 py-1 rounded-full font-medium transition-all duration-200 hover:scale-105 active:scale-95 ${
              language === lang
                ? 'bg-blue-600 text-white shadow-sm'
                : 'bg-gray-100 text-gray-600 hover:bg-gray-200'
            }`}
          >
            {lang.toUpperCase()}
          </button>
        ))}
      </div>

      <div className="text-center">
        <h2 className="text-2xl font-bold text-gray-800">{t('title')}</h2>
        <p className="text-gray-600 mt-2">{t('description')}</p>
      </div>

      <SignedIn>
        <AIChat />
      </SignedIn>

      <SignedOut>
        <div className="bg-white p-8 rounded-lg shadow-sm text-center border border-gray-200">
          <h3 className="text-lg font-medium text-gray-900 mb-4">{t('signInToContinue')}</h3>
          <SignInButton mode="modal">
            <button className="px-6 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors">
              {t('signIn')}
            </button>
          </SignInButton>
        </div>
      </SignedOut>
    </div>
  );
}
