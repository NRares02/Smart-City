import type { ReactNode } from 'react';
import { Navigate } from 'react-router-dom';
import { isAuthenticated } from '../utils/storage';

interface ProtectedRouteProps {
  children: ReactNode;
}

function ProtectedRoute({ children }: ProtectedRouteProps) {
  if (!isAuthenticated()) {
    return <Navigate to="/auth" replace />;
  }

  return <>{children}</>;
}

export default ProtectedRoute;
