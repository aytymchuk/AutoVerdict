import { useEffect, useId, useRef, useState, type FormEvent } from 'react';
import type { CreateCarDataDto } from '../../shared/api/research';
import { useResearchApi } from '../../shared/api/research';
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

const tabs: { id: InputTab; icon: string; label: string; enabled: boolean }[] = [
  { id: 'form', icon: 'assignment', label: 'Enter data', enabled: true },
  { id: 'text', icon: 'content_paste', label: 'Paste text', enabled: false },
  { id: 'photos', icon: 'photo_camera', label: 'Photos', enabled: false },
];

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

function buildCarPayload(form: FormState): CreateCarDataDto {
  return {
    make: form.make.trim() || null,
    model: form.model.trim() || null,
    year: parseOptionalInt(form.year),
    mileageKm: parseOptionalInt(form.mileageKm),
    price: parseOptionalDecimal(form.price),
    currency: 'PLN',
    vin: form.vin.trim() || null,
  };
}

function validateForm(form: FormState): string | null {
  if (!form.make.trim()) {
    return 'Make is required.';
  }
  if (!form.model.trim()) {
    return 'Model is required.';
  }
  if (!form.year.trim() || parseOptionalInt(form.year) === null) {
    return 'Year is required.';
  }
  if (!form.mileageKm.trim() || parseOptionalInt(form.mileageKm) === null) {
    return 'Mileage is required.';
  }
  if (!form.price.trim() || parseOptionalDecimal(form.price) === null) {
    return 'Price is required.';
  }

  return null;
}

export function NewResearchDialog({ open, onClose, onCreated }: NewResearchDialogProps) {
  const dialogRef = useRef<HTMLDialogElement>(null);
  const titleId = useId();
  const researchApi = useResearchApi();
  const [activeTab, setActiveTab] = useState<InputTab>('form');
  const [form, setForm] = useState<FormState>(emptyFormState);
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
        car: buildCarPayload(form),
      });
      resetState();
      onCreated();
      onClose();
    } catch (submitError) {
      setError(submitError instanceof Error ? submitError.message : 'Failed to create research.');
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
            aria-label="Close dialog"
            className="absolute right-5 top-5 flex h-9 w-9 items-center justify-center rounded-full text-on-surface-variant transition-colors hover:bg-surface-container-high hover:text-on-surface"
          >
            <span className="material-symbols-outlined text-[22px]" aria-hidden="true">
              close
            </span>
          </button>

          <div className="border-b border-outline-variant/20 px-6 pb-0 pt-8 sm:px-8">
            <h2 id={titleId} className="font-headline-md text-[28px] font-semibold text-on-surface">
              New Research
            </h2>
            <p className="mt-2 text-[14px] text-on-surface-variant">
              Provide vehicle details to start an AI-powered listing analysis.
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
                    <span className="text-[13px] font-medium text-on-surface-variant">Make</span>
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
                    <span className="text-[13px] font-medium text-on-surface-variant">Model</span>
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
                    <span className="text-[13px] font-medium text-on-surface-variant">Year</span>
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
                    <span className="text-[13px] font-medium text-on-surface-variant">Mileage</span>
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
                    <span className="text-[13px] font-medium text-on-surface-variant">Price</span>
                    <div className="relative">
                      <input
                        type="number"
                        inputMode="decimal"
                        value={form.price}
                        onChange={(event) => updateField('price', event.target.value)}
                        className={formInputClassName}
                        disabled={submitting}
                      />
                      <span className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[13px] text-on-surface-variant">
                        PLN
                      </span>
                    </div>
                  </label>
                  <label className="block space-y-2">
                    <span className="text-[13px] font-medium text-on-surface-variant">
                      VIN <span className="text-on-surface-variant/70">Optional</span>
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
                    Listing Description
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
                    Our AI will extract relevant specs, history, and potential red flags automatically.
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
                  {submitting ? 'Starting analysis...' : 'Start AI Analysis'}
                </button>
              </form>
            )}

            {activeTab === 'text' && (
              <div className="rounded-2xl border border-outline-variant/20 bg-surface-container px-6 py-10 text-center text-on-surface-variant">
                Paste text analysis is coming soon.
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
                <p className="mt-4 text-[15px] font-medium text-on-surface">Vehicle Imagery</p>
                <p className="mt-2 text-[14px] text-on-surface-variant">
                  Drag and drop photos here or click to browse from your device
                </p>
                <p className="mt-2 text-[12px] text-on-surface-variant/70">
                  JPG, PNG, HEIC up to 10MB each
                </p>
                <p className="mt-6 text-[13px] text-on-surface-variant">Photo uploads are coming soon.</p>
              </div>
            )}
          </div>
        </div>
      </div>
    </dialog>
  );
}
