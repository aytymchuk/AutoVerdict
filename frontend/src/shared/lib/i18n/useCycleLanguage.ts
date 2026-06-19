import { useCallback } from 'react';
import { useTranslation } from './useTranslation';
import { SUPPORTED_LANGUAGES } from './types';

export function useCycleLanguage() {
  const { language, setLanguage } = useTranslation();

  const cycleLanguage = useCallback(() => {
    const idx = SUPPORTED_LANGUAGES.indexOf(language);
    const next = SUPPORTED_LANGUAGES[(idx + 1) % SUPPORTED_LANGUAGES.length];
    setLanguage(next);
  }, [language, setLanguage]);

  return { language, setLanguage, cycleLanguage };
}
