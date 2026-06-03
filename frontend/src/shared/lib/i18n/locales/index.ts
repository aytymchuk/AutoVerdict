import type { Language } from '../types';
import { chat } from './chat';
import { landing } from './landing';
import { register } from './register';

function mergeLocale(lang: Language) {
  return {
    ...chat[lang],
    ...register[lang],
    ...landing[lang],
  };
}

export const translations = {
  en: mergeLocale('en'),
  pl: mergeLocale('pl'),
  uk: mergeLocale('uk'),
} as const;

export type TranslationKey = keyof typeof translations.en;
