export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresAtUtc: string;
  email: string;
  roles: string[];
}

export interface MeResponse {
  id: string | null;
  email: string | null;
  roles: string[];
}

