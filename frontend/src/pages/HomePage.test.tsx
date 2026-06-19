import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { HomePage } from './HomePage';

const mockSignOut = vi.fn();
const mockUseUser = vi.fn();

vi.mock('@clerk/clerk-react', () => ({
  useUser: () => mockUseUser(),
  useClerk: () => ({ signOut: mockSignOut }),
}));

function renderPage() {
  return render(
    <MemoryRouter initialEntries={['/home']}>
      <Routes>
        <Route path="/home" element={<HomePage />} />
        <Route path="/" element={<div>landing page</div>} />
      </Routes>
    </MemoryRouter>
  );
}

describe('HomePage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockUseUser.mockReturnValue({
      user: {
        firstName: 'Jane',
        primaryEmailAddress: { emailAddress: 'jane@example.com' },
      },
    });
  });

  it('renders the dashboard navigation links', () => {
    renderPage();
    expect(screen.getByRole('link', { name: 'My Research' })).toHaveAttribute('href', '/home');
    expect(screen.getByRole('link', { name: 'Credits' })).toHaveAttribute('href', '/credits');
  });

  it('shows a personalised welcome message with the first name', () => {
    renderPage();
    expect(screen.getByText('Welcome, Jane! Your dashboard is coming soon.')).toBeInTheDocument();
  });

  it('shows a generic welcome when no first name is available', () => {
    mockUseUser.mockReturnValue({
      user: { firstName: null, primaryEmailAddress: { emailAddress: 'x@x.com' } },
    });
    renderPage();
    expect(screen.getByText('Welcome! Your dashboard is coming soon.')).toBeInTheDocument();
  });

  it('shows the user initial in the account menu trigger', () => {
    renderPage();
    expect(screen.getByRole('button', { name: 'Open account menu' })).toHaveTextContent('J');
  });

  it('calls signOut with redirectUrl "/" when Sign out is clicked', async () => {
    mockSignOut.mockResolvedValue(undefined);
    renderPage();

    fireEvent.click(screen.getByRole('button', { name: 'Open account menu' }));
    fireEvent.click(screen.getByRole('menuitem', { name: 'Sign out' }));

    await waitFor(() =>
      expect(mockSignOut).toHaveBeenCalledWith({ redirectUrl: '/' })
    );
  });
});
