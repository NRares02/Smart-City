import { AxiosError } from 'axios';
import axiosClient from '../api/axiosClient';
import { ENDPOINTS } from '../api/endpoints';
import { clearStoredToken, clearStoredUser, setStoredToken, setStoredUser } from '../utils/storage';
import type {
  ApiErrorResponse,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
} from '../types/auth';

export async function login(payload: LoginRequest): Promise<LoginResponse> {
  const { data } = await axiosClient.post<LoginResponse>(ENDPOINTS.auth.login, payload);
  return data;
}

export async function register(payload: RegisterRequest): Promise<RegisterResponse> {
  const { data } = await axiosClient.post<RegisterResponse>(ENDPOINTS.auth.register, payload);
  return data;
}

export function persistSession(response: LoginResponse): void {
  setStoredToken(response.token);
  setStoredUser(response.user);
}

export function clearSession(): void {
  clearStoredToken();
  clearStoredUser();
}

// Extracts the backend's { message } payload, falling back to a friendly generic message.
export function getAuthErrorMessage(error: unknown, fallback: string): string {
  if (error instanceof AxiosError) {
    if (!error.response) {
      return 'Unable to reach the server. Please try again later.';
    }

    const data = error.response.data as ApiErrorResponse | undefined;
    if (data?.message) {
      return data.message;
    }

    if (error.response.status === 401) {
      return 'Invalid email or password.';
    }

    if (error.response.status === 409) {
      return 'An account with this email already exists.';
    }
  }

  return fallback;
}
