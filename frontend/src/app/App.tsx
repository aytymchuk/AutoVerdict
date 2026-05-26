import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { AppClerkProvider } from './providers/ClerkProvider';
import { ChatPage } from '../pages/ChatPage';
import { LandingPage } from '../pages/LandingPage';
import { AuthPage } from '../pages/AuthPage';
import { I18nProvider } from '../shared/lib/i18n';

export function App() {
  return (
    <BrowserRouter>
      <AppClerkProvider>
        <I18nProvider>
          <Routes>
            <Route path="/" element={<LandingPage />} />
            <Route path="/auth" element={<AuthPage />} />
            <Route
              path="/chat"
              element={
                <div className="min-h-screen bg-gray-50 text-gray-900 font-sans">
                  <header className="bg-white shadow-sm px-6 py-4 flex justify-between items-center">
                    <h1 className="text-xl font-semibold text-blue-600">AutoVerdikt</h1>
                  </header>
                  <main className="container mx-auto p-4">
                    <ChatPage />
                  </main>
                </div>
              }
            />
          </Routes>
        </I18nProvider>
      </AppClerkProvider>
    </BrowserRouter>
  );
}
