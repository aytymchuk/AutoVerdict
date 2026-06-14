import { useState, type FormEvent } from 'react';
import { Link } from 'react-router-dom';
import { useTranslation, type Language } from '../shared/lib/i18n';
import { useUserStatus } from '../shared/hooks/useUserStatus';
import { useWaitlistApi } from '../shared/api/waitlist';

const LANGUAGES: Language[] = ['en', 'pl', 'uk'];

const inputClassName =
  'w-full min-w-0 bg-surface-container-high border border-outline-variant rounded-xl px-4 py-3 text-on-surface text-[15px] placeholder:text-on-surface-variant/50 focus:outline-none focus:border-primary transition-colors';

type FormState = 'idle' | 'success' | 'duplicate' | 'review';

export function WaitingPage() {
  const { t, language, setLanguage } = useTranslation();
  const { email, whitelistStatus } = useUserStatus();
  const waitlistApi = useWaitlistApi();

  const [about, setAbout] = useState('');
  const [formState, setFormState] = useState<FormState>('idle');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  function cycleLanguage() {
    const idx = LANGUAGES.indexOf(language);
    setLanguage(LANGUAGES[(idx + 1) % LANGUAGES.length]);
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    if (formState === 'success' || formState === 'duplicate' || formState === 'review' || whitelistStatus === 'requested') return;

    setError('');
    setSubmitting(true);

    try {
      const result = await waitlistApi.submitRequest(about.trim() || undefined);
      setFormState(result === 'already_submitted' ? 'duplicate' : 'success');
    } catch {
      setError(t('waiting_error_generic'));
    } finally {
      setSubmitting(false);
    }
  }

  const showReview =
    formState === 'review' || formState === 'duplicate' || whitelistStatus === 'requested';
  const showSuccess = formState === 'success';

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
            onClick={cycleLanguage}
            className="text-on-surface-variant text-sm uppercase tracking-wide hover:text-primary transition-colors"
          >
            {language}
          </button>
        </div>

        {showReview ? (
          <>
            <h1 className="font-headline-lg text-[28px] font-semibold text-on-surface mb-4">
              {t('waiting_review_heading')}
            </h1>
            <p className="text-on-surface-variant text-[15px] leading-relaxed mb-8">
              {t('waiting_review_body')}
            </p>
          </>
        ) : (
          <>
            <h1 className="font-headline-lg text-[28px] font-semibold text-on-surface mb-4">
              {t('waiting_heading')}
            </h1>
            <p className="text-on-surface-variant text-[15px] leading-relaxed mb-8">
              {t('waiting_body')}
            </p>
          </>
        )}

        {showSuccess && (
          <p className="mb-6 rounded-xl bg-primary-container/30 border border-primary/20 px-4 py-3 text-on-surface text-[15px]" role="status">
            {t('waiting_success')}
          </p>
        )}

        {formState === 'idle' && whitelistStatus !== 'requested' && (
          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label htmlFor="waiting-email" className="block text-on-surface text-sm font-medium mb-2">
                {t('waiting_email_label')}
              </label>
              <input
                id="waiting-email"
                type="email"
                readOnly
                value={email ?? ''}
                className={`${inputClassName} opacity-80 cursor-not-allowed`}
              />
            </div>

            <div>
              <label htmlFor="waiting-about" className="block text-on-surface text-sm font-medium mb-2">
                {t('waiting_about_label')}
              </label>
              <textarea
                id="waiting-about"
                rows={4}
                value={about}
                onChange={e => setAbout(e.target.value)}
                placeholder={t('waiting_about_placeholder')}
                className={inputClassName}
              />
            </div>

            {error && (
              <p className="text-error text-sm" role="alert">
                {error}
              </p>
            )}

            <button
              type="submit"
              disabled={submitting}
              className="w-full rounded-xl bg-primary text-on-primary px-4 py-3 text-[15px] font-medium hover:opacity-90 transition-opacity disabled:opacity-50"
            >
              {t('waiting_submit')}
            </button>
          </form>
        )}
      </div>
    </div>
  );
}
