import { useEffect, useState } from 'react';
import type { ReactNode } from 'react';
import { I18nContext } from './context';
import { translations, type TranslationKey } from './locales';
import { type Language } from './types';
import { getInitialLanguage } from './language';

interface I18nProviderProps {
  children: ReactNode;
}

export function I18nProvider({ children }: I18nProviderProps) {
  const [language, setLanguageState] = useState<Language>(getInitialLanguage);

  const setLanguage = (lang: Language) => {
    const normalized: Language = (lang as string) === 'ru' ? 'uk' : lang;
    setLanguageState(normalized);
    try {
      localStorage.setItem('av-language', normalized);
    } catch {
      // ignore
    }
  };

  const t = (key: TranslationKey): string => {
    return translations[language][key] || translations.en[key];
  };

  useEffect(() => {
    document.documentElement.lang = language;
  }, [language]);

  return (
    <I18nContext.Provider value={{ language, setLanguage, t }}>
      {children}
    </I18nContext.Provider>
  );
}
