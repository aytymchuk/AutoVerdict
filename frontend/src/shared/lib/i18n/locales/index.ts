import type { Language } from '../types';
import { chat } from './chat';
import { common } from './common';
import { landing } from './landing';
import { register } from './register';
import { waiting } from './waiting';

function mergeLocale(lang: Language) {
  return {
    ...common[lang],
    ...chat[lang],
    ...register[lang],
    ...landing[lang],
    ...waiting[lang],
  };
}

export const translations = {
  en: mergeLocale('en'),
  pl: mergeLocale('pl'),
  uk: mergeLocale('uk'),
} as const;

export type TranslationKey = keyof typeof translations.en;
