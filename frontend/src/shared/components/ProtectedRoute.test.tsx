import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { ProtectedRoute, RegisterRoute, WhitelistRoute } from './ProtectedRoute';
import type { UserStatus } from '../context/userStatusContext';

let mockStatus: UserStatus = 'loading';

vi.mock('../hooks/useUserStatus', () => ({
  useUserStatus: () => ({ status: mockStatus, whitelistStatus: 'none', refetch: vi.fn() }),
}));

function renderInRouter(element: React.ReactNode) {
  return render(
    <MemoryRouter initialEntries={['/current']}>
      <Routes>
        <Route path="/current" element={element} />
        <Route path="/" element={<div>landing page</div>} />
        <Route path="/auth" element={<div>auth page</div>} />
        <Route path="/register" element={<div>register page</div>} />
        <Route path="/home" element={<div>home page</div>} />
        <Route path="/waiting" element={<div>waiting page</div>} />
      </Routes>
    </MemoryRouter>
  );
}

describe('ProtectedRoute', () => {
  beforeEach(() => vi.clearAllMocks());

  it('shows loading spinner while status is loading', () => {
    mockStatus = 'loading';
    renderInRouter(<ProtectedRoute><div>protected content</div></ProtectedRoute>);
    expect(screen.queryByText('protected content')).not.toBeInTheDocument();
    expect(screen.getByText('progress_activity')).toBeInTheDocument();
  });

  it('redirects to / when unauthenticated', () => {
    mockStatus = 'unauthenticated';
    renderInRouter(<ProtectedRoute><div>protected content</div></ProtectedRoute>);
    expect(screen.getByText('landing page')).toBeInTheDocument();
    expect(screen.queryByText('protected content')).not.toBeInTheDocument();
  });

  it('redirects to /register when unregistered', () => {
    mockStatus = 'unregistered';
    renderInRouter(<ProtectedRoute><div>protected content</div></ProtectedRoute>);
    expect(screen.getByText('register page')).toBeInTheDocument();
    expect(screen.queryByText('protected content')).not.toBeInTheDocument();
  });

  it('redirects to /waiting when not_whitelisted', () => {
    mockStatus = 'not_whitelisted';
    renderInRouter(<ProtectedRoute><div>protected content</div></ProtectedRoute>);
    expect(screen.getByText('waiting page')).toBeInTheDocument();
    expect(screen.queryByText('protected content')).not.toBeInTheDocument();
  });

  it('renders children when registered', () => {
    mockStatus = 'registered';
    renderInRouter(<ProtectedRoute><div>protected content</div></ProtectedRoute>);
    expect(screen.getByText('protected content')).toBeInTheDocument();
  });
});

describe('WhitelistRoute', () => {
  beforeEach(() => vi.clearAllMocks());

  it('redirects to /auth when unauthenticated', () => {
    mockStatus = 'unauthenticated';
    renderInRouter(<WhitelistRoute><div>waiting form</div></WhitelistRoute>);
    expect(screen.getByText('auth page')).toBeInTheDocument();
  });

  it('redirects to /home when registered', () => {
    mockStatus = 'registered';
    renderInRouter(<WhitelistRoute><div>waiting form</div></WhitelistRoute>);
    expect(screen.getByText('home page')).toBeInTheDocument();
  });

  it('renders children when not_whitelisted', () => {
    mockStatus = 'not_whitelisted';
    renderInRouter(<WhitelistRoute><div>waiting form</div></WhitelistRoute>);
    expect(screen.getByText('waiting form')).toBeInTheDocument();
  });
});

describe('RegisterRoute', () => {
  beforeEach(() => vi.clearAllMocks());

  it('shows loading spinner while status is loading', () => {
    mockStatus = 'loading';
    renderInRouter(<RegisterRoute><div>register form</div></RegisterRoute>);
    expect(screen.queryByText('register form')).not.toBeInTheDocument();
    expect(screen.getByText('progress_activity')).toBeInTheDocument();
  });

  it('redirects to /auth when unauthenticated', () => {
    mockStatus = 'unauthenticated';
    renderInRouter(<RegisterRoute><div>register form</div></RegisterRoute>);
    expect(screen.getByText('auth page')).toBeInTheDocument();
    expect(screen.queryByText('register form')).not.toBeInTheDocument();
  });

  it('redirects to /home when already registered', () => {
    mockStatus = 'registered';
    renderInRouter(<RegisterRoute><div>register form</div></RegisterRoute>);
    expect(screen.getByText('home page')).toBeInTheDocument();
    expect(screen.queryByText('register form')).not.toBeInTheDocument();
  });

  it('renders children when unregistered', () => {
    mockStatus = 'unregistered';
    renderInRouter(<RegisterRoute><div>register form</div></RegisterRoute>);
    expect(screen.getByText('register form')).toBeInTheDocument();
  });
});
