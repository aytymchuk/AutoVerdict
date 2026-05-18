import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, act } from '@testing-library/react';
import { AIChat } from './AIChat';
import '@testing-library/jest-dom';

const mockGetToken = vi.fn();

vi.mock('@clerk/clerk-react', () => {
  return {
    useAuth: () => ({
      getToken: mockGetToken,
    }),
  };
});

describe('AIChat', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('calls getToken on mount to fetch authentication token', async () => {
    mockGetToken.mockResolvedValue('mocked-clerk-token-123');

    await act(async () => {
      render(<AIChat />);
    });

    expect(mockGetToken).toHaveBeenCalled();
  });

  it('handles getToken rejection gracefully', async () => {
    const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
    mockGetToken.mockRejectedValue(new Error('Auth failed'));

    await act(async () => {
      render(<AIChat />);
    });

    expect(mockGetToken).toHaveBeenCalled();
    expect(consoleSpy).toHaveBeenCalledWith(
      'Failed to retrieve Clerk authentication token:',
      expect.any(Error)
    );
    consoleSpy.mockRestore();
  });

  it('renders chat input and send button', async () => {
    mockGetToken.mockResolvedValue('token');

    await act(async () => {
      render(<AIChat />);
    });

    expect(screen.getByPlaceholderText('Ask AutoVerdikt...')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Send' })).toBeInTheDocument();
  });
});
