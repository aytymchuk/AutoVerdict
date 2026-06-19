import { useState, type FormEvent } from 'react';
import { useTranslation } from '../shared/lib/i18n';
import { useUserStatus } from '../shared/hooks/useUserStatus';
import { useWaitlistApi } from '../shared/api/waitlist';
import { PageShell } from '../shared/components/PageShell';
import { formInputClassName } from '../shared/styles/formInput';

type FormState = 'idle' | 'success' | 'duplicate';

export function WaitingPage() {
  const { t } = useTranslation();
  const { email, whitelistStatus, refetch } = useUserStatus();
  const waitlistApi = useWaitlistApi();

  const [about, setAbout] = useState('');
  const [formState, setFormState] = useState<FormState>('idle');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    if (
      formState === 'success' ||
      formState === 'duplicate' ||
      whitelistStatus === 'requested' ||
      whitelistStatus === 'declined'
    )
      return;

    setError('');
    setSubmitting(true);

    try {
      const result = await waitlistApi.submitRequest(about.trim() || undefined);
      setFormState(result === 'already_submitted' ? 'duplicate' : 'success');
      await refetch();
    } catch {
      setError(t('waiting_error_generic'));
    } finally {
      setSubmitting(false);
    }
  }

  const showDeclined = whitelistStatus === 'declined';
  const showReview =
    formState === 'duplicate' || whitelistStatus === 'requested';
  const showSuccess = formState === 'success';
  const showForm =
    formState === 'idle' &&
    whitelistStatus !== 'requested' &&
    whitelistStatus !== 'declined';

  const heading = showDeclined
    ? t('waiting_declined_heading')
    : showReview
      ? t('waiting_review_heading')
      : t('waiting_heading');

  const body = showDeclined
    ? t('waiting_declined_body')
    : showReview
      ? t('waiting_review_body')
      : t('waiting_body');

  return (
    <PageShell>
      {/* Heading block */}
      <div className="mb-lg md:mb-xl">
        <h1 className="font-headline-lg text-[36px] md:text-[48px] leading-tight font-semibold text-on-surface tracking-tight mb-md">
          {heading}
        </h1>
        <p className="font-body-lg text-[18px] text-text-secondary max-w-2xl leading-relaxed">
          {body}
        </p>
      </div>

      {/* Success banner */}
      {showSuccess && (
        <p
          className="mb-lg rounded-xl bg-primary-container/20 border border-primary/20 px-md py-sm text-on-surface font-body-md"
          role="status"
        >
          {t('waiting_success')}
        </p>
      )}

      {/* Form card */}
      {showForm && (
        <form
          onSubmit={handleSubmit}
          className="bg-surface border border-border-subtle rounded-xl p-lg md:p-xl inner-glow relative overflow-hidden group"
        >
          {/* Hover left accent */}
          <div className="absolute left-0 top-0 bottom-0 w-[3px] bg-risk-medium opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

          <div className="flex flex-col gap-lg">
            {/* Email field */}
            <div className="flex flex-col gap-sm">
              <label
                htmlFor="waiting-email"
                className="font-body-sm text-[14px] text-on-surface font-medium"
              >
                {t('waiting_email_label')}
              </label>
              <input
                id="waiting-email"
                type="email"
                readOnly
                value={email ?? ''}
                className={`${formInputClassName} opacity-70 cursor-not-allowed`}
              />
            </div>

            {/* About textarea */}
            <div className="flex flex-col gap-sm">
              <label
                htmlFor="waiting-about"
                className="font-body-sm text-[14px] text-on-surface font-medium"
              >
                {t('waiting_about_label')}
              </label>
              <textarea
                id="waiting-about"
                rows={4}
                value={about}
                onChange={e => setAbout(e.target.value)}
                placeholder={t('waiting_about_placeholder')}
                className={`${formInputClassName} resize-y min-h-[100px]`}
              />
            </div>

            {error && (
              <p className="text-error text-[14px]" role="alert">
                {error}
              </p>
            )}

            {/* Submit */}
            <div className="pt-sm">
              <button
                type="submit"
                disabled={submitting}
                className="w-full bg-risk-medium text-surface-container-lowest font-body-lg text-[16px] font-bold rounded-full py-md px-lg flex items-center justify-center gap-sm transition-all duration-300 hover:bg-primary-fixed-dim hover:shadow-[0_0_24px_-6px_rgba(245,158,11,0.3)] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-surface focus:ring-risk-medium disabled:opacity-50"
              >
                {t('waiting_submit')}
              </button>
            </div>
          </div>
        </form>
      )}

      {/* Trust indicator */}
      <div className="mt-lg flex items-center justify-center gap-sm text-text-secondary">
        <span className="material-symbols-outlined text-[16px]">lock</span>
        <span className="font-body-sm text-[14px]">{t('waiting_trust')}</span>
      </div>
    </PageShell>
  );
}
