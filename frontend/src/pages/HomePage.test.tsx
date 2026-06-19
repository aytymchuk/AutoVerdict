import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { HomePage } from './HomePage';
import type { IResearchApi } from '../shared/api/research';

const mockSignOut = vi.fn();
const mockUseUser = vi.fn();
const mockList = vi.fn<IResearchApi['list']>();

vi.mock('@clerk/clerk-react', () => ({
  useUser: () => mockUseUser(),
  useClerk: () => ({ signOut: mockSignOut }),
  useAuth: () => ({ getToken: vi.fn().mockResolvedValue('token') }),
}));

vi.mock('../shared/api/research', () => ({
  useResearchApi: () => ({
    list: mockList,
    create: vi.fn(),
    get: vi.fn(),
    rename: vi.fn(),
    delete: vi.fn(),
  }),
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
    mockList.mockResolvedValue({ items: [], total: 0 });
  });

  it('renders the dashboard navigation links', async () => {
    renderPage();
    expect(screen.getByRole('link', { name: 'My Research' })).toHaveAttribute('href', '/home');
    expect(screen.getByRole('link', { name: 'Credits' })).toHaveAttribute('href', '/credits');
  });

  it('shows the empty state when the user has no research', async () => {
    renderPage();

    await waitFor(() => {
      expect(screen.getByText("You don't have any research yet")).toBeInTheDocument();
    });
    expect(screen.getByRole('button', { name: /Start your first research/i })).toBeInTheDocument();
  });

  it('shows the research list when the user has research', async () => {
    mockList.mockResolvedValue({
      items: [
        {
          id: 'research-1',
          name: '2019 Volvo XC60',
          status: 'analyzed',
          riskLevel: 'low',
          car: {
            make: 'Volvo',
            model: 'XC60',
            year: 2019,
            mileageKm: 45000,
            price: 120000,
          },
          creditsSpent: 1,
          updatedAt: '2023-10-24T12:00:00.000Z',
        },
      ],
      total: 1,
    });

    renderPage();

    await waitFor(() => {
      expect(screen.getByText('Recent Research')).toBeInTheDocument();
    });
    expect(screen.getByText('2019 Volvo XC60')).toBeInTheDocument();
    expect(screen.getByText('Low Risk')).toBeInTheDocument();
  });

  it('shows the user initial in the account menu trigger', async () => {
    renderPage();
    await waitFor(() => {
      expect(screen.getByRole('button', { name: 'Open account menu' })).toHaveTextContent('J');
    });
  });

  it('calls signOut with redirectUrl "/" when Sign out is clicked', async () => {
    mockSignOut.mockResolvedValue(undefined);
    renderPage();

    await waitFor(() => {
      expect(screen.getByText("You don't have any research yet")).toBeInTheDocument();
    });

    fireEvent.click(screen.getByRole('button', { name: 'Open account menu' }));
    fireEvent.click(screen.getByRole('menuitem', { name: 'Sign out' }));

    await waitFor(() => expect(mockSignOut).toHaveBeenCalledWith({ redirectUrl: '/' }));
  });
});
