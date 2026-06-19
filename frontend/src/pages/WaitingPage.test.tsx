import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter } from 'react-router-dom';
import { WaitingPage } from './WaitingPage';
import { waiting } from '../shared/lib/i18n/locales/waiting';
import { common } from '../shared/lib/i18n/locales/common';
import type { Language } from '../shared/lib/i18n/types';

let mockStatus = 'not_whitelisted';
let mockEmail: string | null = 'user@example.com';
let mockWhitelistStatus: 'none' | 'requested' | 'approved' | 'declined' = 'none';
let mockLanguage: Language = 'en';
const mockSubmit = vi.fn();
const mockRefetch = vi.fn().mockResolvedValue('not_whitelisted');

vi.mock('../shared/hooks/useUserStatus', () => ({
  useUserStatus: () => ({
    status: mockStatus,
    email: mockEmail,
    whitelistStatus: mockWhitelistStatus,
    refetch: mockRefetch,
  }),
}));

vi.mock('../shared/api/waitlist', () => ({
  useWaitlistApi: () => ({ submitRequest: mockSubmit }),
}));

vi.mock('../shared/lib/i18n', async importOriginal => {
  const actual = await importOriginal<typeof import('../shared/lib/i18n')>();
  return {
    ...actual,
    useTranslation: () => ({
      language: mockLanguage,
      setLanguage: vi.fn(),
      t: (key: keyof typeof waiting.en | keyof typeof common.en) =>
        waiting[mockLanguage][key as keyof typeof waiting.en] ??
        common[mockLanguage][key as keyof typeof common.en] ??
        waiting.en[key as keyof typeof waiting.en] ??
        common.en[key as keyof typeof common.en],
    }),
  };
});

function renderPage(lang: Language = 'en') {
  mockLanguage = lang;
  return render(
    <MemoryRouter>
      <WaitingPage />
    </MemoryRouter>
  );
}

describe('WaitingPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockStatus = 'not_whitelisted';
    mockEmail = 'user@example.com';
    mockWhitelistStatus = 'none';
    mockLanguage = 'en';
    mockSubmit.mockResolvedValue('created');
  });

  it('renders heading and explanation', () => {
    renderPage();
    expect(screen.getByRole('heading', { name: waiting.en.waiting_heading })).toBeInTheDocument();
    expect(screen.getByText(waiting.en.waiting_body)).toBeInTheDocument();
  });

  it('renders email field pre-filled and read-only', () => {
    renderPage();
    const email = screen.getByLabelText(waiting.en.waiting_email_label) as HTMLInputElement;
    expect(email.value).toBe('user@example.com');
    expect(email).toHaveAttribute('readonly');
  });

  it('shows success message after submission without reload', async () => {
    renderPage();
    fireEvent.click(screen.getByRole('button', { name: waiting.en.waiting_submit }));

    await waitFor(() =>
      expect(screen.getByText(waiting.en.waiting_success)).toBeInTheDocument()
    );
    expect(mockSubmit).toHaveBeenCalled();
  });

  it('shows duplicate message when already submitted', async () => {
    mockSubmit.mockResolvedValue('already_submitted');
    renderPage();
    fireEvent.click(screen.getByRole('button', { name: waiting.en.waiting_submit }));

    await waitFor(() =>
      expect(screen.getByText(waiting.en.waiting_review_body)).toBeInTheDocument()
    );
  });

  it('shows under-review view when whitelist status is requested', () => {
    mockWhitelistStatus = 'requested';
    renderPage();
    expect(screen.getByRole('heading', { name: waiting.en.waiting_review_heading })).toBeInTheDocument();
    expect(screen.getByText(waiting.en.waiting_review_body)).toBeInTheDocument();
    expect(screen.queryByRole('button', { name: waiting.en.waiting_submit })).not.toBeInTheDocument();
  });

  it('shows declined view when whitelist status is declined', () => {
    mockWhitelistStatus = 'declined';
    renderPage();
    expect(screen.getByRole('heading', { name: waiting.en.waiting_declined_heading })).toBeInTheDocument();
    expect(screen.getByText(waiting.en.waiting_declined_body)).toBeInTheDocument();
    expect(screen.queryByRole('button', { name: waiting.en.waiting_submit })).not.toBeInTheDocument();
  });

  it('refetches user status after successful submission', async () => {
    renderPage();
    fireEvent.click(screen.getByRole('button', { name: waiting.en.waiting_submit }));

    await waitFor(() => expect(mockRefetch).toHaveBeenCalled());
  });

  it('renders Polish copy when locale is pl', () => {
    renderPage('pl');
    expect(screen.getByRole('heading', { name: waiting.pl.waiting_heading })).toBeInTheDocument();
  });

  it('renders Ukrainian copy when locale is uk', () => {
    renderPage('uk');
    expect(screen.getByRole('heading', { name: waiting.uk.waiting_heading })).toBeInTheDocument();
  });
});
