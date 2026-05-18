import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, act } from '@testing-library/react';
import { AIChat } from './AIChat';
import React from 'react';
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

  it('shows loading state initially while token is unresolved', async () => {
    let resolveToken: (value: string) => void = () => {};
    const tokenPromise = new Promise<string>((resolve) => {
      resolveToken = resolve;
    });
    mockGetToken.mockReturnValue(tokenPromise);

    render(<AIChat />);

    expect(screen.getByText('Loading...')).toBeInTheDocument();

    // Resolve the token to clean up
    await act(async () => {
      resolveToken('test-token-xyz');
    });
  });

  it('renders token when successfully fetched', async () => {
    mockGetToken.mockResolvedValue('mocked-clerk-token-123');

    await act(async () => {
      render(<AIChat />);
    });

    expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
    expect(screen.getByText('mocked-clerk-token-123')).toBeInTheDocument();
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
