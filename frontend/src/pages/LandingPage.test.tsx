import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { LandingPage } from './LandingPage';

function renderPage() {
  return render(
    <MemoryRouter initialEntries={['/']}>
      <Routes>
        <Route path="/" element={<LandingPage />} />
      </Routes>
    </MemoryRouter>
  );
}

describe('LandingPage', () => {
  let languageGetter: ReturnType<typeof vi.spyOn>;

  beforeEach(() => {
    vi.clearAllMocks();
    languageGetter = vi.spyOn(navigator, 'language', 'get');
    languageGetter.mockReturnValue('en-US');
  });

  afterEach(() => {
    languageGetter.mockRestore();
  });

  it('renders the landing page for unauthenticated visitors', () => {
    renderPage();
    expect(screen.getAllByText('AutoVerdikt').length).toBeGreaterThan(0);
  });

  it('renders the landing page without redirecting', () => {
    renderPage();
    expect(screen.getAllByText('AutoVerdikt').length).toBeGreaterThan(0);
  });
});
