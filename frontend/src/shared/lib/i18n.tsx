/* eslint-disable react-refresh/only-export-components */
import { useState, createContext, useContext } from 'react';
import type { ReactNode } from 'react';

export type Language = 'en' | 'pl' | 'uk';

export const translations = {
  en: {
    title: "Vehicle Investigation Assistant",
    description: "Ask questions about vehicle history, CEPiK records, or upload documents for analysis.",
    signInToContinue: "Please sign in to continue",
    signIn: "Sign In",
    chatPlaceholder: "Ask AutoVerdikt...",
    send: "Send",
  },
  pl: {
    title: "Asystent ds. Badania Pojazdów",
    description: "Zadawaj pytania dotyczące historii pojazdu, rekordów CEPiK lub przesyłaj dokumenty do analizy.",
    signInToContinue: "Zaloguj się, aby kontynuować",
    signIn: "Zaloguj Się",
    chatPlaceholder: "Zapytaj AutoVerdikt...",
    send: "Wyślij",
  },
  uk: {
    title: "Асистент з розслідування транспортних засобів",
    description: "Ставте запитання про історію автомобіля, записи CEPiK або завантажуйте документи для аналізу.",
    signInToContinue: "Будь ласка, увійдіть, щоб продовжити",
    signIn: "Увійти",
    chatPlaceholder: "Запитати AutoVerdikt...",
    send: "Надіслати",
  }
} as const;

export type TranslationKey = keyof typeof translations['en'];

function getInitialLanguage(): Language {
  if (typeof window === 'undefined') return 'en';
  
  const browserLang = navigator.language.split('-')[0].toLowerCase();
  
  // Russian is NEVER supported. If browser language is 'ru', fallback to 'en'.
  if (browserLang === 'ru') {
    return 'en';
  }
  
  if (browserLang === 'pl') return 'pl';
  if (browserLang === 'uk') return 'uk';
  
  return 'en';
}

interface I18nContextType {
  language: Language;
  setLanguage: (lang: Language) => void;
  t: (key: TranslationKey) => string;
}

export const I18nContext = createContext<I18nContextType | null>(null);

export function useTranslation() {
  const context = useContext(I18nContext);
  if (!context) {
    const lang = getInitialLanguage();
    return {
      language: lang,
      setLanguage: () => {},
      t: (key: TranslationKey) => translations[lang][key] || translations['en'][key],
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
    // Safeguard to ensure Russian is never selected
    if ((lang as string) === 'ru') {
      setLanguageState('en');
    } else {
      setLanguageState(lang);
    }
  };

  const t = (key: TranslationKey): string => {
    return translations[language][key] || translations['en'][key];
  };

  return (
    <I18nContext.Provider value={{ language, setLanguage, t }}>
      {children}
    </I18nContext.Provider>
  );
}
