import { useContext } from 'react';
import { I18nContext } from './context';
import { translations, type TranslationKey } from './locales';
import { getInitialLanguage } from './language';

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
