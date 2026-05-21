import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of, tap } from 'rxjs';
import { LoginRequest, LoginResponse, MeResponse } from './auth.models';

const TOKEN_KEY = 'taskflow_access_token';

// ASP.NET Identity encodes ClaimTypes.Role under this URI in the JWT payload.
const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

function parseRoles(token: string | null): string[] {
  if (!token) return [];
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const raw = payload[ROLE_CLAIM];
    if (!raw) return [];
    return Array.isArray(raw) ? raw : [raw];
  } catch {
    return [];
  }
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _token = signal<string | null>(localStorage.getItem(TOKEN_KEY));
  readonly token = computed(() => this._token());
  readonly isAuthenticated = computed(() => !!this._token());
  readonly isAdmin = computed(() => parseRoles(this._token()).includes('Admin'));

  constructor(private readonly http: HttpClient) {}

  login(request: LoginRequest) {
    return this.http.post<LoginResponse>('/api/auth/login', request).pipe(
      tap((res) => this.setToken(res.accessToken))
    );
  }

  register(request: LoginRequest) {
    return this.http.post<LoginResponse>('/api/auth/register', request).pipe(
      tap((res) => this.setToken(res.accessToken))
    );
  }

  logout() {
    this.clearToken();
  }

  me() {
    return this.http.get<MeResponse>('/api/auth/me').pipe(
      catchError(() => of({ id: null, email: null, roles: [] } satisfies MeResponse))
    );
  }

  private setToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
    this._token.set(token);
  }

  private clearToken() {
    localStorage.removeItem(TOKEN_KEY);
    this._token.set(null);
  }
}

