// Request payloads use camelCase; ASP.NET Core model binding matches case-insensitively
// against SmartCity.Api/DTOs/Auth/RegisterRequest.cs and LoginRequest.cs.
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  phoneNumber?: string;
}

// Response shapes mirror the anonymous objects returned by AuthController exactly (snake_case keys).
export interface AuthUser {
  id: string;
  name: string;
  email: string;
  phone_number: string | null;
  role_id: string;
  is_active: boolean;
}

export interface LoginResponse {
  token: string;
  user: AuthUser;
}

export interface RegisterResponse {
  id: string;
  name: string;
  email: string;
  phone_number: string | null;
  role_id: string;
  is_active: boolean;
  inserted_at: string;
}

export interface ApiErrorResponse {
  message: string;
}
