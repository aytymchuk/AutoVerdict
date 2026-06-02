import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '@clerk/clerk-react';
import { useUsersApi } from '../shared/api/users';
import { LoadingScreen } from '../shared/components/ProtectedRoute';

export function AuthCallbackPage() {
  const { isLoaded, isSignedIn } = useAuth();
  const usersApi = useUsersApi();
  const navigate = useNavigate();

  useEffect(() => {
    if (!isLoaded) return;

    if (!isSignedIn) {
      navigate('/auth', { replace: true });
      return;
    }

    let cancelled = false;

    usersApi
      .getMe()
      .then(user => {
        if (cancelled) return;
        navigate(user ? '/home' : '/register', { replace: true });
      })
      .catch(() => {
        if (!cancelled) navigate('/auth', { replace: true });
      });

    return () => {
      cancelled = true;
    };
  }, [isLoaded, isSignedIn, usersApi, navigate]);

  return <LoadingScreen />;
}
