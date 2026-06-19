import { useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '@clerk/clerk-react';
import { useUsersApi } from '../shared/api/users';
import { useUserStatus } from '../shared/hooks/useUserStatus';
import { useTranslation } from '../shared/lib/i18n';
import { PageShell } from '../shared/components/PageShell';
import { formInputClassName } from '../shared/styles/formInput';

interface ApiError {
  errors?: Record<string, string[]>;
  error?: string;
  title?: string;
}

export function RegisterPage() {
  const { user } = useUser();
  const usersApi = useUsersApi();
  const { refetch } = useUserStatus();
  const navigate = useNavigate();
  const { t } = useTranslation();

  const [name, setName] = useState(user?.fullName ?? '');
  const [email, setEmail] = useState(user?.primaryEmailAddress?.emailAddress ?? '');
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [submitting, setSubmitting] = useState(false);
  const [globalError, setGlobalError] = useState('');

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setErrors({});
    setGlobalError('');
    setSubmitting(true);

    try {
      await usersApi.register(name, email);
      const status = await refetch();
      navigate(status === 'registered' ? '/home' : '/waiting', { replace: true });
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
        } else if (parsed.error ?? parsed.title) {
          setGlobalError(parsed.error ?? parsed.title!);
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
    <PageShell>
      {/* Heading block */}
      <div className="mb-lg md:mb-xl">
        <h1 className="font-headline-lg text-[36px] md:text-[48px] leading-tight font-semibold text-on-surface tracking-tight mb-md">
          {t('register_title')}
        </h1>
        <p className="font-body-lg text-[18px] text-text-secondary max-w-2xl leading-relaxed">
          {t('register_subtitle')}
        </p>
      </div>

      {/* Form card */}
      <form
        onSubmit={handleSubmit}
        className="bg-surface border border-border-subtle rounded-xl p-lg md:p-xl inner-glow relative overflow-hidden group"
      >
        {/* Hover left accent */}
        <div className="absolute left-0 top-0 bottom-0 w-[3px] bg-risk-medium opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

        <div className="flex flex-col gap-lg">
          {globalError && (
            <p
              role="alert"
              className="text-error text-[14px] leading-snug bg-error-container/20 border border-error/30 rounded-lg px-md py-sm"
            >
              {globalError}
            </p>
          )}

          {/* Name field */}
          <div className="flex flex-col gap-sm">
            <label
              htmlFor="reg-name"
              className="font-body-sm text-[14px] text-on-surface font-medium"
            >
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
              className={formInputClassName}
            />
            {errors.name && <p className="text-error text-[13px]">{errors.name}</p>}
          </div>

          {/* Email field */}
          <div className="flex flex-col gap-sm">
            <label
              htmlFor="reg-email"
              className="font-body-sm text-[14px] text-on-surface font-medium"
            >
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
              className={formInputClassName}
            />
            {errors.email && <p className="text-error text-[13px]">{errors.email}</p>}
          </div>

          {/* Submit */}
          <div className="pt-sm">
            <button
              type="submit"
              disabled={submitting}
              className="w-full bg-risk-medium text-surface-container-lowest font-body-lg text-[16px] font-bold rounded-full py-md px-lg flex items-center justify-center gap-sm transition-all duration-300 hover:bg-primary-fixed-dim hover:shadow-[0_0_24px_-6px_rgba(245,158,11,0.3)] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-surface focus:ring-risk-medium disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {submitting ? t('register_submitting') : t('register_submit')}
            </button>
          </div>
        </div>
      </form>
    </PageShell>
  );
}
