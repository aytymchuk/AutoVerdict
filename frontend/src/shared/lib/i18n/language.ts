import { LANGUAGE_STORAGE_KEY, SUPPORTED_LANGUAGES, type Language } from './types';

function resolveLanguageTag(tag: string): Language | null {
  const primary = tag.split('-')[0].toLowerCase();

  // Russian is not supported; map to Ukrainian.
  if (primary === 'ru') return 'uk';
  if (primary === 'pl') return 'pl';
  if (primary === 'uk') return 'uk';
  if (primary === 'en') return 'en';

  return null;
}

/** Resolve language from browser/OS preferences only, ignoring localStorage. */
export function resolveBrowserLanguage(): Language {
  if (typeof window === 'undefined') return 'en';

  for (const tag of navigator.languages ?? []) {
    const resolved = resolveLanguageTag(tag);
    if (resolved) return resolved;
  }

  return resolveLanguageTag(navigator.language) ?? 'en';
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

  return resolveBrowserLanguage();
}
