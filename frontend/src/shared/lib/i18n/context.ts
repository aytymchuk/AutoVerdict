import { createContext } from 'react';
import type { TranslationKey } from './locales';
import type { Language } from './types';

export interface I18nContextType {
  language: Language;
  setLanguage: (lang: Language) => void;
  t: (key: TranslationKey) => string;
}

export const I18nContext = createContext<I18nContextType | null>(null);
