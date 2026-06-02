import { useEffect, useState, useContext } from 'react';
import type { ReactNode } from 'react';
import { I18nContext } from './context';
import { translations, type TranslationKey } from './locales';
import {
  LANGUAGE_STORAGE_KEY,
  SUPPORTED_LANGUAGES,
  type Language,
} from './types';

export type { Language, TranslationKey };
export { translations };

function resolveLanguageTag(tag: string): Language | null {
  const primary = tag.split('-')[0].toLowerCase();

  // Russian is not supported; map to Ukrainian.
  if (primary === 'ru') return 'uk';
  if (primary === 'pl') return 'pl';
  if (primary === 'uk') return 'uk';
  if (primary === 'en') return 'en';

  return null;
}

/** Prefer saved choice, then browser/OS language list, then primary browser locale. */
export function getInitialLanguage(): Language {
  if (typeof window === 'undefined') return 'en';

  try {
    const stored = localStorage.getItem(LANGUAGE_STORAGE_KEY);
    if (stored && SUPPORTED_LANGUAGES.includes(stored as Language)) {
      return stored as Language;
    }
  } catch {
    // localStorage unavailable (private mode, etc.)
  }

  for (const tag of navigator.languages ?? []) {
    const resolved = resolveLanguageTag(tag);
    if (resolved) return resolved;
  }

  return resolveLanguageTag(navigator.language) ?? 'en';
}

export function useTranslation() {
  const context = useContext(I18nContext);
  if (!context) {
    const lang = getInitialLanguage();
    return {
      language: lang,
      setLanguage: () => {},
      t: (key: TranslationKey) => translations[lang][key] || translations.en[key],
    };
  }
  return context;
}

interface I18nProviderProps {
  children: ReactNode;
}

export function I18nProvider({ children }: I18nProviderProps) {
  const [language, setLanguageState] = useState<Language>(getInitialLanguage);

  const setLanguage = (lang: Language) => {
    const normalized: Language = (lang as string) === 'ru' ? 'uk' : lang;
    setLanguageState(normalized);
    try {
      localStorage.setItem(LANGUAGE_STORAGE_KEY, normalized);
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
