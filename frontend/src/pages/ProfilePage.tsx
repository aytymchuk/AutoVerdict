import { useUser } from '@clerk/clerk-react';
import { useState, type FormEvent } from 'react';
import { useUsersApi } from '../shared/api/users';
import { useUserStatus } from '../shared/hooks/useUserStatus';
import { useTranslation } from '../shared/lib/i18n';
import { AppLayout } from '../shared/components/AppLayout';
import { formInputClassName, selectInputClassName, selectWrapperClassName } from '../shared/styles/formInput';
import type { Language } from '../shared/lib/i18n/types';

type LanguageOption = Language | '';
type CurrencyOption = 'PLN' | 'UAH' | 'EUR' | 'USD' | '';

export function ProfilePage() {
  const { user } = useUser();
  const usersApi = useUsersApi();
  const { language: profileLanguage, defaultCurrency: profileCurrency } = useUserStatus();
  const { t, setLanguage } = useTranslation();

  const [firstName, setFirstName] = useState(user?.firstName ?? '');
  const [lastName, setLastName] = useState(user?.lastName ?? '');
  const [selectedLanguage, setSelectedLanguage] = useState<LanguageOption>(
    (profileLanguage as LanguageOption) ?? '',
  );
  const [selectedCurrency, setSelectedCurrency] = useState<CurrencyOption>(
    (profileCurrency as CurrencyOption) ?? '',
  );
  const [submitting, setSubmitting] = useState(false);
  const [saveStatus, setSaveStatus] = useState<'idle' | 'success' | 'error'>('idle');

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setSaveStatus('idle');
    setSubmitting(true);

    try {
      await Promise.all([
        user?.update({ firstName: firstName.trim(), lastName: lastName.trim() }),
        usersApi.updateProfile(selectedLanguage || null, selectedCurrency || null),
      ]);
      if (selectedLanguage) {
        setLanguage(selectedLanguage as Language);
      }
      setSaveStatus('success');
    } catch {
      setSaveStatus('error');
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <AppLayout>
      <div className="mx-auto w-full max-w-3xl px-gutter py-xl">
      {/* Heading block */}
      <div className="mb-lg md:mb-xl">
        <h1 className="heading-page mb-md">
          {t('profile_title')}
        </h1>
        <p className="font-body-lg text-[18px] text-text-secondary max-w-2xl leading-relaxed">
          {t('profile_subtitle')}
        </p>
      </div>

      {/* Settings card */}
      <form
        onSubmit={handleSubmit}
        className="bg-surface border border-border-subtle rounded-xl p-lg md:p-xl inner-glow relative overflow-hidden group"
      >
        {/* Hover left accent */}
        <div className="absolute left-0 top-0 bottom-0 w-[3px] bg-risk-medium opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

        <div className="flex flex-col gap-lg">
          {saveStatus === 'success' && (
            <p
              role="status"
              className="text-[14px] leading-snug bg-surface-variant/30 border border-outline-variant/30 rounded-lg px-md py-sm text-on-surface"
            >
              {t('profile_save_success')}
            </p>
          )}

          {saveStatus === 'error' && (
            <p
              role="alert"
              className="text-error text-[14px] leading-snug bg-error-container/20 border border-error/30 rounded-lg px-md py-sm"
            >
              {t('profile_error_generic')}
            </p>
          )}

          {/* First name */}
          <div className="flex flex-col gap-sm">
            <label
              htmlFor="profile-first-name"
              className="font-body-sm text-[14px] text-on-surface font-medium"
            >
              {t('profile_first_name_label')}
            </label>
            <input
              id="profile-first-name"
              type="text"
              value={firstName}
              onChange={e => setFirstName(e.target.value)}
              className={formInputClassName}
            />
          </div>

          {/* Last name */}
          <div className="flex flex-col gap-sm">
            <label
              htmlFor="profile-last-name"
              className="font-body-sm text-[14px] text-on-surface font-medium"
            >
              {t('profile_last_name_label')}
            </label>
            <input
              id="profile-last-name"
              type="text"
              value={lastName}
              onChange={e => setLastName(e.target.value)}
              className={formInputClassName}
            />
          </div>

          {/* Email (read-only) */}
          <div className="flex flex-col gap-sm">
            <label className="font-body-sm text-[14px] text-on-surface font-medium">
              {t('profile_email_label')}
            </label>
            <div className={`${formInputClassName} cursor-default select-all text-on-surface-variant`}>
              {user?.primaryEmailAddress?.emailAddress ?? ''}
            </div>
          </div>

          {/* Language selector */}
          <div className="flex flex-col gap-sm">
            <label
              htmlFor="profile-language"
              className="font-body-sm text-[14px] text-on-surface font-medium"
            >
              {t('profile_language_label')}
            </label>
            <div className={selectWrapperClassName}>
              <select
                id="profile-language"
                value={selectedLanguage}
                onChange={e => setSelectedLanguage(e.target.value as LanguageOption)}
                className={selectInputClassName}
              >
                <option value="">{t('profile_language_browser')}</option>
                <option value="en">{t('profile_language_en')}</option>
                <option value="pl">{t('profile_language_pl')}</option>
                <option value="uk">{t('profile_language_uk')}</option>
              </select>
              <span
                className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 material-symbols-outlined text-[20px] text-on-surface-variant"
                aria-hidden="true"
              >
                expand_more
              </span>
            </div>
          </div>

          {/* Currency selector */}
          <div className="flex flex-col gap-sm">
            <label
              htmlFor="profile-currency"
              className="font-body-sm text-[14px] text-on-surface font-medium"
            >
              {t('profile_currency_label')}
            </label>
            <div className={selectWrapperClassName}>
              <select
                id="profile-currency"
                value={selectedCurrency}
                onChange={e => setSelectedCurrency(e.target.value as CurrencyOption)}
                className={selectInputClassName}
              >
                <option value="">{t('profile_currency_none')}</option>
                <option value="PLN">PLN — Polish Złoty</option>
                <option value="UAH">UAH — Ukrainian Hryvnia</option>
                <option value="EUR">EUR — Euro</option>
                <option value="USD">USD — US Dollar</option>
              </select>
              <span
                className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 material-symbols-outlined text-[20px] text-on-surface-variant"
                aria-hidden="true"
              >
                expand_more
              </span>
            </div>
          </div>

          {/* Submit */}
          <div className="pt-sm">
            <button
              type="submit"
              disabled={submitting}
              className="w-full bg-risk-medium text-surface-container-lowest font-body-lg text-[16px] font-bold rounded-full py-md px-lg flex items-center justify-center gap-sm transition-all duration-300 hover:bg-primary-fixed-dim hover:shadow-[0_0_24px_-6px_rgba(245,158,11,0.3)] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-surface focus:ring-risk-medium disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {submitting ? t('profile_saving') : t('profile_save')}
            </button>
          </div>
        </div>
      </form>
      </div>
    </AppLayout>
  );
}
