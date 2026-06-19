import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import type { ReactNode } from 'react';
import { UserStatusProvider } from './userStatusProvider';
import { useUserStatus } from '../hooks/useUserStatus';

let mockIsSignedIn = false;
let mockIsLoaded = true;

vi.mock('@clerk/clerk-react', () => ({
  useAuth: () => ({
    isSignedIn: mockIsSignedIn,
    isLoaded: mockIsLoaded,
    getToken: vi.fn(),
  }),
}));

const mockGetMe = vi.hoisted(() => vi.fn());
const mockUsersApi = vi.hoisted(() => ({ getMe: mockGetMe, register: vi.fn() }));

vi.mock('../api/users', () => ({
  useUsersApi: () => mockUsersApi,
}));

function StatusConsumer() {
  const { status, refetch } = useUserStatus();
  return (
    <>
      <div data-testid="status">{status}</div>
      <button data-testid="refetch" onClick={refetch}>refetch</button>
    </>
  );
}

function Wrapper({ children }: { children: ReactNode }) {
  return <UserStatusProvider>{children}</UserStatusProvider>;
}

describe('UserStatusProvider', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockIsSignedIn = false;
    mockIsLoaded = true;
  });

  it('stays loading while Clerk has not finished initializing', () => {
    mockIsLoaded = false;
    render(<StatusConsumer />, { wrapper: Wrapper });
    expect(screen.getByTestId('status')).toHaveTextContent('loading');
    expect(mockGetMe).not.toHaveBeenCalled();
  });

  it('sets status to unauthenticated when user is not signed in', async () => {
    mockIsSignedIn = false;
    render(<StatusConsumer />, { wrapper: Wrapper });
    await waitFor(() =>
      expect(screen.getByTestId('status')).toHaveTextContent('unauthenticated')
    );
    expect(mockGetMe).not.toHaveBeenCalled();
  });

  it('sets status to not_whitelisted when getMe returns user without whitelist', async () => {
    mockIsSignedIn = true;
    mockGetMe.mockResolvedValue({
      id: '1',
      name: 'Test',
      email: 'test@test.com',
      registeredAt: '2024-01-01',
      isWhitelisted: false,
      whitelistStatus: 'none',
    });

    render(<StatusConsumer />, { wrapper: Wrapper });

    await waitFor(() =>
      expect(screen.getByTestId('status')).toHaveTextContent('not_whitelisted')
    );
  });

  it('sets status to registered when getMe returns a whitelisted user', async () => {
    mockIsSignedIn = true;
    mockGetMe.mockResolvedValue({
      id: '1',
      name: 'Test',
      email: 'test@test.com',
      registeredAt: '2024-01-01',
      isWhitelisted: true,
      whitelistStatus: 'none',
    });

    render(<StatusConsumer />, { wrapper: Wrapper });

    await waitFor(() =>
      expect(screen.getByTestId('status')).toHaveTextContent('registered')
    );
    expect(mockGetMe).toHaveBeenCalled();
  });

  it('sets status to unregistered when getMe returns null (404)', async () => {
    mockIsSignedIn = true;
    mockGetMe.mockResolvedValue(null);

    render(<StatusConsumer />, { wrapper: Wrapper });

    await waitFor(() =>
      expect(screen.getByTestId('status')).toHaveTextContent('unregistered')
    );
  });

  it('sets status to error when the API call throws (network error)', async () => {
    mockIsSignedIn = true;
    mockGetMe.mockRejectedValue(new Error('Network error'));

    render(<StatusConsumer />, { wrapper: Wrapper });

    await waitFor(() =>
      expect(screen.getByTestId('status')).toHaveTextContent('error')
    );
  });

  it('retriggers the API call when refetch is invoked', async () => {
    mockIsSignedIn = true;
    mockGetMe.mockResolvedValue({
      id: '1',
      name: 'Test',
      email: 'test@test.com',
      registeredAt: '2024-01-01',
      isWhitelisted: true,
      whitelistStatus: 'none',
    });

    render(<StatusConsumer />, { wrapper: Wrapper });
    await waitFor(() =>
      expect(screen.getByTestId('status')).toHaveTextContent('registered')
    );
    expect(mockGetMe).toHaveBeenCalledTimes(1);

    fireEvent.click(screen.getByTestId('refetch'));

    await waitFor(() => expect(mockGetMe).toHaveBeenCalledTimes(2));
  });
});

describe('useUserStatus', () => {
  it('throws when used outside UserStatusProvider', () => {
    const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
    expect(() => render(<StatusConsumer />)).toThrow(
      'useUserStatus must be used inside UserStatusProvider'
    );
    consoleSpy.mockRestore();
  });
});
