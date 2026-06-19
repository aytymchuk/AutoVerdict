import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { AppClerkProvider } from './providers/ClerkProvider';
import { ChatPage } from '../pages/ChatPage';
import { LandingPage } from '../pages/LandingPage';
import { AuthPage } from '../pages/AuthPage';
import { AuthCallbackPage } from '../pages/AuthCallbackPage';
import { RegisterPage } from '../pages/RegisterPage';
import { HomePage } from '../pages/HomePage';
import { WaitingPage } from '../pages/WaitingPage';
import { I18nProvider } from '../shared/lib/i18n/index';
import { UserStatusProvider } from '../shared/context/userStatusProvider';
import { ProtectedRoute, RegisterRoute, WhitelistRoute } from '../shared/components/ProtectedRoute';

export function App() {
  return (
    <BrowserRouter>
      <AppClerkProvider>
        <I18nProvider>
          <UserStatusProvider>
            <Routes>
              <Route path="/" element={<LandingPage />} />
              <Route path="/auth" element={<AuthPage />} />
              <Route path="/auth/callback" element={<AuthCallbackPage />} />
              <Route
                path="/register"
                element={
                  <RegisterRoute>
                    <RegisterPage />
                  </RegisterRoute>
                }
              />
              <Route
                path="/waiting"
                element={
                  <WhitelistRoute>
                    <WaitingPage />
                  </WhitelistRoute>
                }
              />
              <Route
                path="/home"
                element={
                  <ProtectedRoute>
                    <HomePage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/chat"
                element={
                  <ProtectedRoute>
                    <ChatPage />
                  </ProtectedRoute>
                }
              />
            </Routes>
          </UserStatusProvider>
        </I18nProvider>
      </AppClerkProvider>
    </BrowserRouter>
  );
}
