import type { Language } from '../types';
import { chat } from './chat';
import { common } from './common';
import { home } from './home';
import { landing } from './landing';
import { profile } from './profile';
import { register } from './register';
import { waiting } from './waiting';

function mergeLocale(lang: Language) {
  return {
    ...common[lang],
    ...chat[lang],
    ...home[lang],
    ...register[lang],
    ...landing[lang],
    ...waiting[lang],
    ...profile[lang],
  };
}

export const translations = {
  en: mergeLocale('en'),
  pl: mergeLocale('pl'),
  uk: mergeLocale('uk'),
} as const;

export type TranslationKey = keyof typeof translations.en;
