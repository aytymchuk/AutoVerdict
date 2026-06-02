import { useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '@clerk/clerk-react';
import { useApi } from '../shared/api/fetcher';
import { useUserStatus } from '../shared/hooks/useUserStatus';

interface ApiError {
  errors?: Record<string, string[]>;
  error?: string;
}

export function RegisterPage() {
  const { user } = useUser();
  const { fetchWithAuth } = useApi();
  const { refetch } = useUserStatus();
  const navigate = useNavigate();

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
      await fetchWithAuth('/api/users/register', {
        method: 'POST',
        body: JSON.stringify({ name, email }),
      });
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
          setGlobalError('Registration failed. Please try again.');
        }
      } catch {
        setGlobalError('Registration failed. Please try again.');
      }
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div className="min-h-screen bg-surface-container-lowest flex items-center justify-center px-4">
      <div className="w-full max-w-md">
        <div className="mb-8 text-center">
          <h1 className="font-headline-md text-[32px] font-semibold text-on-surface tracking-tight mb-2">
            Complete your profile
          </h1>
          <p className="text-on-surface-variant text-[15px]">
            Just a couple of details to get you started.
          </p>
        </div>

        <form
          onSubmit={handleSubmit}
          className="bg-surface-container rounded-2xl border border-outline-variant/30 p-8 flex flex-col gap-5"
        >
          {globalError && (
            <p className="text-error text-[14px] bg-error-container/20 border border-error/30 rounded-lg px-4 py-3">
              {globalError}
            </p>
          )}

          <div className="flex flex-col gap-1.5">
            <label className="text-on-surface-variant text-[13px] font-medium" htmlFor="reg-name">
              Full name
            </label>
            <input
              id="reg-name"
              type="text"
              value={name}
              onChange={e => setName(e.target.value)}
              placeholder="Your name"
              required
              className="bg-surface-container-high border border-outline-variant rounded-xl px-4 py-3 text-on-surface text-[15px] placeholder:text-on-surface-variant/50 focus:outline-none focus:border-primary transition-colors"
            />
            {errors.name && (
              <p className="text-error text-[13px]">{errors.name}</p>
            )}
          </div>

          <div className="flex flex-col gap-1.5">
            <label className="text-on-surface-variant text-[13px] font-medium" htmlFor="reg-email">
              Email address
            </label>
            <input
              id="reg-email"
              type="email"
              value={email}
              onChange={e => setEmail(e.target.value)}
              placeholder="you@example.com"
              required
              className="bg-surface-container-high border border-outline-variant rounded-xl px-4 py-3 text-on-surface text-[15px] placeholder:text-on-surface-variant/50 focus:outline-none focus:border-primary transition-colors"
            />
            {errors.email && (
              <p className="text-error text-[13px]">{errors.email}</p>
            )}
          </div>

          <button
            type="submit"
            disabled={submitting}
            className="mt-2 bg-primary text-on-primary font-medium text-[15px] rounded-xl py-3 px-6 hover:opacity-90 active:opacity-80 transition-opacity disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {submitting ? 'Creating account…' : 'Create account'}
          </button>
        </form>
      </div>
    </div>
  );
}
