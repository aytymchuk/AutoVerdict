import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { RegisterPage } from './RegisterPage';

const mockRegister = vi.hoisted(() => vi.fn());
const mockRefetch = vi.fn().mockResolvedValue('registered');
const mockUseUser = vi.fn();

vi.mock('@clerk/clerk-react', () => ({
  useUser: () => mockUseUser(),
  useAuth: () => ({ getToken: vi.fn().mockResolvedValue('token') }),
}));

vi.mock('../shared/api/users', () => ({
  useUsersApi: () => ({ getMe: vi.fn(), register: mockRegister }),
}));

vi.mock('../shared/hooks/useUserStatus', () => ({
  useUserStatus: () => ({ status: 'unregistered', refetch: mockRefetch }),
}));

function renderPage() {
  return render(
    <MemoryRouter initialEntries={['/register']}>
      <Routes>
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/home" element={<div>home page</div>} />
        <Route path="/waiting" element={<div>waiting page</div>} />
      </Routes>
    </MemoryRouter>
  );
}

describe('RegisterPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockUseUser.mockReturnValue({
      user: {
        fullName: 'Jane Doe',
        primaryEmailAddress: { emailAddress: 'jane@example.com' },
      },
    });
  });

  it('pre-fills name and email from Clerk user data', () => {
    renderPage();
    expect(screen.getByLabelText('Full name')).toHaveValue('Jane Doe');
    expect(screen.getByLabelText('Email address')).toHaveValue('jane@example.com');
  });

  it('shows empty fields when no Clerk user is available', () => {
    mockUseUser.mockReturnValue({ user: null });
    renderPage();
    expect(screen.getByLabelText('Full name')).toHaveValue('');
    expect(screen.getByLabelText('Email address')).toHaveValue('');
  });

  it('submits name and email via usersApi.register', async () => {
    mockRegister.mockResolvedValue({ id: '1', name: 'Jane Doe', email: 'jane@example.com', registeredAt: '2024-01-01' });
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Create account' }));

    await waitFor(() =>
      expect(mockRegister).toHaveBeenCalledWith('Jane Doe', 'jane@example.com')
    );
  });

  it('calls refetch and navigates to /home on success when whitelisted', async () => {
    mockRefetch.mockResolvedValue('registered');
    mockRegister.mockResolvedValue({ id: '1', name: 'Jane Doe', email: 'jane@example.com', registeredAt: '2024-01-01' });
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Create account' }));

    await waitFor(() =>
      expect(screen.getByText('home page')).toBeInTheDocument()
    );
    expect(mockRefetch).toHaveBeenCalled();
  });

  it('navigates to /waiting when user is not whitelisted after register', async () => {
    mockRefetch.mockResolvedValue('not_whitelisted');
    mockRegister.mockResolvedValue({
      id: '1',
      name: 'Jane Doe',
      email: 'jane@example.com',
      registeredAt: '2024-01-01',
      isWhitelisted: false,
    });
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Create account' }));

    await waitFor(() =>
      expect(screen.getByText('waiting page')).toBeInTheDocument()
    );
  });

  it('displays field-level validation errors from API 400 response', async () => {
    mockRegister.mockRejectedValue(
      new Error(JSON.stringify({
        errors: {
          Name: ['Name is required.'],
          Email: ['A valid email address is required.'],
        },
      }))
    );
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Create account' }));

    await waitFor(() => {
      expect(screen.getByText('Name is required.')).toBeInTheDocument();
      expect(screen.getByText('A valid email address is required.')).toBeInTheDocument();
    });
  });

  it('displays global error when API returns an error string', async () => {
    mockRegister.mockRejectedValue(
      new Error(JSON.stringify({ error: 'User is already registered.' }))
    );
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Create account' }));

    await waitFor(() =>
      expect(screen.getByText('User is already registered.')).toBeInTheDocument()
    );
  });

  it('displays global error when API returns a ProblemDetails title', async () => {
    mockRegister.mockRejectedValue(
      new Error(JSON.stringify({ title: 'User is already registered.', status: 409 }))
    );
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Create account' }));

    await waitFor(() =>
      expect(screen.getByText('User is already registered.')).toBeInTheDocument()
    );
  });

  it('shows generic error message when response body is not JSON', async () => {
    mockRegister.mockRejectedValue(new Error('Internal Server Error'));
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Create account' }));

    await waitFor(() =>
      expect(screen.getByText('Registration failed. Please try again.')).toBeInTheDocument()
    );
  });

  it('disables the submit button and shows loading text while submitting', async () => {
    let resolveSubmit!: (v: unknown) => void;
    mockRegister.mockReturnValue(new Promise(resolve => { resolveSubmit = resolve; }));
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Create account' }));

    await waitFor(() =>
      expect(screen.getByRole('button', { name: 'Creating account…' })).toBeDisabled()
    );

    resolveSubmit({ id: '1' });
  });
});
