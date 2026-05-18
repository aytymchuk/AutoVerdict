import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import { ChatPage } from './ChatPage';
import '@testing-library/jest-dom';

// Mock Clerk components
vi.mock('@clerk/clerk-react', () => {
  return {
    SignedIn: ({ children }: { children: React.ReactNode }) => <div data-testid="signed-in">{children}</div>,
    SignedOut: ({ children }: { children: React.ReactNode }) => <div data-testid="signed-out">{children}</div>,
    SignInButton: ({ children }: { children: React.ReactNode }) => <div data-testid="signin-button">{children}</div>,
  };
});

// Mock AIChat component to avoid auth token resolution inside the ChatPage test
vi.mock('../features/AIChat/AIChat', () => {
  return {
    AIChat: () => <div data-testid="ai-chat">Mocked AIChat</div>,
  };
});

describe('ChatPage', () => {
  let languageGetter: {
    mockRestore: () => void;
    mockReturnValue: (value: string) => unknown;
  };

  beforeEach(() => {
    languageGetter = vi.spyOn(navigator, 'language', 'get');
    languageGetter.mockReturnValue('en-US');
  });

  afterEach(() => {
    languageGetter.mockRestore();
  });

  it('renders page headers and structure correctly', () => {
    render(<ChatPage />);
    expect(screen.getByText('Vehicle Investigation Assistant')).toBeInTheDocument();
    expect(
      screen.getByText('Ask questions about vehicle history, CEPiK records, or upload documents for analysis.')
    ).toBeInTheDocument();
  });

  it('renders SignedIn and SignedOut wrappers', () => {
    render(<ChatPage />);
    expect(screen.getByTestId('signed-in')).toBeInTheDocument();
    expect(screen.getByTestId('signed-out')).toBeInTheDocument();
  });
});
