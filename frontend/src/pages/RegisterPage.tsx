import { useState, type FormEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useUser } from '@clerk/clerk-react';
import { useUsersApi } from '../shared/api/users';
import { useUserStatus } from '../shared/hooks/useUserStatus';
import { useTranslation, type Language } from '../shared/lib/i18n';

interface ApiError {
  errors?: Record<string, string[]>;
  error?: string;
}

const LANGUAGES: Language[] = ['en', 'pl', 'uk'];

const inputClassName =
  'w-full min-w-0 bg-surface-container-high border border-outline-variant rounded-xl px-4 py-3 text-on-surface text-[15px] placeholder:text-on-surface-variant/50 focus:outline-none focus:border-primary transition-colors';

export function RegisterPage() {
  const { user } = useUser();
  const usersApi = useUsersApi();
  const { refetch } = useUserStatus();
  const navigate = useNavigate();
  const { t, language, setLanguage } = useTranslation();

  const [name, setName] = useState(user?.fullName ?? '');
  const [email, setEmail] = useState(user?.primaryEmailAddress?.emailAddress ?? '');
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [submitting, setSubmitting] = useState(false);
  const [globalError, setGlobalError] = useState('');

  function cycleLanguage() {
    const idx = LANGUAGES.indexOf(language);
    setLanguage(LANGUAGES[(idx + 1) % LANGUAGES.length]);
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setErrors({});
    setGlobalError('');
    setSubmitting(true);

    try {
      await usersApi.register(name, email);
      refetch();
      navigate('/home', { replace: true });
    } catch (err) {
      const text = err instanceof Error ? err.message : String(err);
      try {
        const parsed: ApiError = JSON.parse(text);
        if (parsed.errors) {
          const flat: Record<string, string> = {};
          for (const [k, v] of Object.entries(parsed.errors)) {
            flat[k.toLowerCase()] = v.join(' ');
          }
          setErrors(flat);
        } else if (parsed.error) {
          setGlobalError(parsed.error);
        } else {
          setGlobalError(t('register_error_generic'));
        }
      } catch {
        setGlobalError(t('register_error_generic'));
      }
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div className="min-h-screen w-full bg-surface-container-lowest px-4 py-10 sm:px-6 sm:py-14">
      <div className="mx-auto w-full max-w-form min-w-0">
        <div className="mb-8 flex items-start justify-between gap-4">
          <Link
            to="/"
            className="font-headline-md text-[20px] font-semibold text-on-surface tracking-tight hover:text-primary transition-colors shrink-0"
          >
            AutoVerdikt
          </Link>
          <button
            type="button"
            aria-label={t('register_switch_language')}
            title={t('landing_footer_language')}
            onClick={cycleLanguage}
            className="text-on-surface-variant hover:text-primary transition-colors flex items-center justify-center gap-1.5 px-3 h-10 rounded-full hover:bg-surface-container-highest text-[13px] font-medium shrink-0"
          >
            <span className="material-symbols-outlined text-[20px]">language</span>
            <span className="uppercase tracking-wider text-[11px]">{language}</span>
          </button>
        </div>

        <header className="mb-8">
          <h1 className="font-headline-md text-[28px] sm:text-[32px] leading-tight font-semibold text-on-surface tracking-tight text-balance">
            {t('register_title')}
          </h1>
          <p className="mt-2 text-on-surface-variant text-[15px] leading-relaxed">
            {t('register_subtitle')}
          </p>
        </header>

        <form
          onSubmit={handleSubmit}
          className="w-full bg-surface-container rounded-2xl border border-outline-variant/30 p-6 sm:p-8 flex flex-col gap-5"
        >
          {globalError && (
            <p
              role="alert"
              className="text-error text-[14px] leading-snug bg-error-container/20 border border-error/30 rounded-lg px-4 py-3"
            >
              {globalError}
            </p>
          )}

          <div className="flex flex-col gap-1.5">
            <label className="text-on-surface-variant text-[13px] font-medium" htmlFor="reg-name">
              {t('register_name_label')}
            </label>
            <input
              id="reg-name"
              type="text"
              value={name}
              onChange={e => setName(e.target.value)}
              placeholder={t('register_name_placeholder')}
              required
              autoComplete="name"
              className={inputClassName}
            />
            {errors.name && <p className="text-error text-[13px]">{errors.name}</p>}
          </div>

          <div className="flex flex-col gap-1.5">
            <label className="text-on-surface-variant text-[13px] font-medium" htmlFor="reg-email">
              {t('register_email_label')}
            </label>
            <input
              id="reg-email"
              type="email"
              value={email}
              onChange={e => setEmail(e.target.value)}
              placeholder={t('register_email_placeholder')}
              required
              autoComplete="email"
              className={inputClassName}
            />
            {errors.email && <p className="text-error text-[13px]">{errors.email}</p>}
          </div>

          <button
            type="submit"
            disabled={submitting}
            className="mt-1 w-full bg-primary text-on-primary font-medium text-[15px] rounded-xl py-3.5 px-6 text-center whitespace-normal hover:opacity-90 active:opacity-80 transition-opacity disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {submitting ? t('register_submitting') : t('register_submit')}
          </button>
        </form>
      </div>
    </div>
  );
}
