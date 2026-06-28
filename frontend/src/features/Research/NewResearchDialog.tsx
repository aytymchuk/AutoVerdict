import { useEffect, useId, useRef, useState, type FormEvent } from 'react';
import type { CreateCarDataDto } from '../../shared/api/research';
import { useResearchApi } from '../../shared/api/research';
import { useUserStatus } from '../../shared/hooks/useUserStatus';
import { useTranslation } from '../../shared/lib/i18n';
import { formInputClassName } from '../../shared/styles/formInput';

type InputTab = 'form' | 'text' | 'photos';

interface NewResearchDialogProps {
  open: boolean;
  onClose: () => void;
  onCreated: () => void;
}

interface FormState {
  make: string;
  model: string;
  year: string;
  mileageKm: string;
  price: string;
  vin: string;
  description: string;
}

const emptyFormState: FormState = {
  make: '',
  model: '',
  year: '',
  mileageKm: '',
  price: '',
  vin: '',
  description: '',
};


function parseOptionalInt(value: string): number | null {
  const trimmed = value.trim();
  if (!trimmed) {
    return null;
  }

  const parsed = Number.parseInt(trimmed, 10);
  return Number.isNaN(parsed) ? null : parsed;
}

function parseOptionalDecimal(value: string): number | null {
  const trimmed = value.trim();
  if (!trimmed) {
    return null;
  }

  const parsed = Number.parseFloat(trimmed);
  return Number.isNaN(parsed) ? null : parsed;
}

const CURRENCIES = ['PLN', 'EUR', 'USD', 'UAH'] as const;
type Currency = (typeof CURRENCIES)[number];

function buildCarPayload(form: FormState, currency: Currency): CreateCarDataDto {
  return {
    make: form.make.trim() || null,
    model: form.model.trim() || null,
    year: parseOptionalInt(form.year),
    mileageKm: parseOptionalInt(form.mileageKm),
    price: parseOptionalDecimal(form.price),
    currency,
    vin: form.vin.trim() || null,
  };
}

export function NewResearchDialog({ open, onClose, onCreated }: NewResearchDialogProps) {
  const dialogRef = useRef<HTMLDialogElement>(null);
  const titleId = useId();
  const researchApi = useResearchApi();
  const { t } = useTranslation();
  const { defaultCurrency } = useUserStatus();

  const tabs = [
    { id: 'form' as InputTab, icon: 'assignment', label: t('research_input_form'), enabled: true },
    { id: 'text' as InputTab, icon: 'content_paste', label: t('research_input_text'), enabled: false },
    { id: 'photos' as InputTab, icon: 'photo_camera', label: t('research_input_photos'), enabled: false },
  ];

  function validateForm(form: FormState): string | null {
    if (!form.make.trim()) return t('dialog_error_make_required');
    if (!form.model.trim()) return t('dialog_error_model_required');
    if (!form.year.trim() || parseOptionalInt(form.year) === null) return t('dialog_error_year_required');
    if (!form.mileageKm.trim() || parseOptionalInt(form.mileageKm) === null) return t('dialog_error_mileage_required');
    if (!form.price.trim() || parseOptionalDecimal(form.price) === null) return t('dialog_error_price_required');
    return null;
  }
  const defaultCurrencyValue: Currency =
    CURRENCIES.includes(defaultCurrency as Currency) ? (defaultCurrency as Currency) : 'PLN';
  const [activeTab, setActiveTab] = useState<InputTab>('form');
  const [form, setForm] = useState<FormState>(emptyFormState);
  // null means "follow profile default"; set to a specific value when the user picks one
  const [currencyOverride, setCurrencyOverride] = useState<Currency | null>(null);
  const currency = currencyOverride ?? defaultCurrencyValue;
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const dialog = dialogRef.current;
    if (!dialog) {
      return;
    }

    if (open && !dialog.open) {
      dialog.showModal();
      return;
    }

    if (!open && dialog.open) {
      dialog.close();
    }
  }, [open]);

  function resetState() {
    setActiveTab('form');
    setForm(emptyFormState);
    setCurrencyOverride(null);
    setSubmitting(false);
    setError(null);
  }

  function handleClose() {
    if (submitting) {
      return;
    }

    resetState();
    onClose();
  }

  function updateField<K extends keyof FormState>(field: K, value: FormState[K]) {
    setForm((current) => ({ ...current, [field]: value }));
    setError(null);
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const validationError = validateForm(form);
    if (validationError) {
      setError(validationError);
      return;
    }

    setSubmitting(true);
    setError(null);

    try {
      await researchApi.create({
        inputMethod: 'form',
        car: buildCarPayload(form, currency),
      });
      resetState();
      onCreated();
      onClose();
    } catch {
      setError(t('dialog_error_create'));
      setSubmitting(false);
    }
  }

  return (
    <dialog
      ref={dialogRef}
      aria-labelledby={titleId}
      className="m-0 h-full max-h-none w-full max-w-none border-0 bg-transparent p-0 backdrop:bg-black/70"
      onCancel={(event) => {
        event.preventDefault();
        handleClose();
      }}
      onClose={handleClose}
    >
      <div className="flex min-h-full items-center justify-center px-gutter py-10">
        <div className="relative w-full max-w-[720px] rounded-3xl border border-outline-variant/20 bg-surface-container-low inner-glow">
          <button
            type="button"
            onClick={handleClose}
            disabled={submitting}
            aria-label={t('dialog_close')}
            className="absolute right-5 top-5 flex h-9 w-9 items-center justify-center rounded-full text-on-surface-variant transition-colors hover:bg-surface-container-high hover:text-on-surface"
          >
            <span className="material-symbols-outlined text-[22px]" aria-hidden="true">
              close
            </span>
          </button>

          <div className="border-b border-outline-variant/20 px-6 pb-0 pt-8 sm:px-8">
            <h2 id={titleId} className="font-headline-md text-[28px] font-semibold text-on-surface">
              {t('dialog_new_research_title')}
            </h2>
            <p className="mt-2 text-[14px] text-on-surface-variant">
              {t('dialog_new_research_subtitle')}
            </p>

            <div className="mt-6 flex gap-2 overflow-x-auto">
              {tabs.map((tab) => (
                <button
                  key={tab.id}
                  type="button"
                  disabled={!tab.enabled}
                  onClick={() => tab.enabled && setActiveTab(tab.id)}
                  className={[
                    'inline-flex items-center gap-2 rounded-t-xl border-b-2 px-4 py-3 text-[14px] font-medium transition-colors',
                    activeTab === tab.id
                      ? 'border-primary text-primary'
                      : 'border-transparent text-on-surface-variant',
                    tab.enabled ? 'hover:text-on-surface' : 'cursor-not-allowed opacity-50',
                  ].join(' ')}
                >
                  <span className="material-symbols-outlined text-[18px]" aria-hidden="true">
                    {tab.icon}
                  </span>
                  {tab.label}
                </button>
              ))}
            </div>
          </div>

          <div className="px-6 py-8 sm:px-8">
            {activeTab === 'form' && (
              <form className="space-y-6" onSubmit={handleSubmit}>
                <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                  <label className="block space-y-2">
                    <span className="text-[13px] font-medium text-on-surface-variant">{t('dialog_field_make')}</span>
                    <input
                      type="text"
                      value={form.make}
                      onChange={(event) => updateField('make', event.target.value)}
                      className={formInputClassName}
                      autoComplete="off"
                      disabled={submitting}
                    />
                  </label>
                  <label className="block space-y-2">
                    <span className="text-[13px] font-medium text-on-surface-variant">{t('dialog_field_model')}</span>
                    <input
                      type="text"
                      value={form.model}
                      onChange={(event) => updateField('model', event.target.value)}
                      className={formInputClassName}
                      autoComplete="off"
                      disabled={submitting}
                    />
                  </label>
                  <label className="block space-y-2">
                    <span className="text-[13px] font-medium text-on-surface-variant">{t('dialog_field_year')}</span>
                    <input
                      type="number"
                      inputMode="numeric"
                      value={form.year}
                      onChange={(event) => updateField('year', event.target.value)}
                      className={formInputClassName}
                      disabled={submitting}
                    />
                  </label>
                  <label className="block space-y-2">
                    <span className="text-[13px] font-medium text-on-surface-variant">{t('dialog_field_mileage')}</span>
                    <div className="relative">
                      <input
                        type="number"
                        inputMode="numeric"
                        value={form.mileageKm}
                        onChange={(event) => updateField('mileageKm', event.target.value)}
                        className={formInputClassName}
                        disabled={submitting}
                      />
                      <span className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[13px] text-on-surface-variant">
                        km
                      </span>
                    </div>
                  </label>
                  <label className="block space-y-2">
                    <span className="text-[13px] font-medium text-on-surface-variant">{t('dialog_field_price')}</span>
                    <div className="flex gap-2">
                      <input
                        type="number"
                        inputMode="decimal"
                        value={form.price}
                        onChange={(event) => updateField('price', event.target.value)}
                        className={`${formInputClassName} min-w-0 flex-1`}
                        disabled={submitting}
                      />
                      <select
                        value={currency}
                        onChange={(e) => setCurrencyOverride(e.target.value as Currency)}
                        disabled={submitting}
                        aria-label={t('dialog_field_currency')}
                        className="appearance-none rounded-lg border border-surface-variant bg-surface-alt px-3 py-[14px] text-[13px] font-medium text-on-surface focus:outline-none focus:ring-2 focus:ring-risk-medium/50 focus:border-risk-medium transition-all shadow-sm cursor-pointer"
                      >
                        {CURRENCIES.map((c) => (
                          <option key={c} value={c}>{c}</option>
                        ))}
                      </select>
                    </div>
                  </label>
                  <label className="block space-y-2">
                    <span className="text-[13px] font-medium text-on-surface-variant">
                      {t('dialog_field_vin')} <span className="text-on-surface-variant/70">{t('dialog_field_vin_optional')}</span>
                    </span>
                    <input
                      type="text"
                      value={form.vin}
                      onChange={(event) => updateField('vin', event.target.value)}
                      className={formInputClassName}
                      autoComplete="off"
                      disabled={submitting}
                    />
                  </label>
                </div>

                <label className="block space-y-2">
                  <span className="text-[13px] font-medium text-on-surface-variant">
                    {t('dialog_field_description')}
                  </span>
                  <textarea
                    value={form.description}
                    onChange={(event) => updateField('description', event.target.value)}
                    rows={4}
                    className={`${formInputClassName} resize-y`}
                    disabled={submitting}
                  />
                  <p className="flex items-start gap-2 text-[13px] text-on-surface-variant">
                    <span className="material-symbols-outlined mt-0.5 text-[16px]" aria-hidden="true">
                      info
                    </span>
                    {t('dialog_field_description_hint')}
                  </p>
                </label>

                {error && (
                  <p className="rounded-lg border border-risk-high/30 bg-risk-high/10 px-4 py-3 text-[14px] text-risk-high">
                    {error}
                  </p>
                )}

                <button
                  type="submit"
                  disabled={submitting}
                  className="inline-flex w-full items-center justify-center gap-2 rounded-full bg-primary px-6 py-3 text-[15px] font-semibold text-on-primary transition-colors hover:bg-primary-fixed-dim disabled:opacity-50 sm:w-auto"
                >
                  <span className="material-symbols-outlined text-[20px]" aria-hidden="true">
                    analytics
                  </span>
                  {submitting ? t('dialog_submitting') : t('dialog_submit')}
                </button>
              </form>
            )}

            {activeTab === 'text' && (
              <div className="rounded-2xl border border-outline-variant/20 bg-surface-container px-6 py-10 text-center text-on-surface-variant">
                {t('dialog_text_soon')}
              </div>
            )}

            {activeTab === 'photos' && (
              <div className="rounded-2xl border border-dashed border-outline-variant/30 bg-surface-container px-6 py-12 text-center">
                <span
                  className="material-symbols-outlined text-[40px] text-on-surface-variant"
                  aria-hidden="true"
                >
                  cloud_upload
                </span>
                <p className="mt-4 text-[15px] font-medium text-on-surface">{t('dialog_photos_title')}</p>
                <p className="mt-2 text-[14px] text-on-surface-variant">
                  {t('dialog_photos_drag')}
                </p>
                <p className="mt-2 text-[12px] text-on-surface-variant/70">
                  {t('dialog_photos_format')}
                </p>
                <p className="mt-6 text-[13px] text-on-surface-variant">{t('dialog_photos_soon')}</p>
              </div>
            )}
          </div>
        </div>
      </div>
    </dialog>
  );
}
