import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of, tap } from 'rxjs';
import { LoginRequest, LoginResponse, MeResponse } from './auth.models';

const TOKEN_KEY = 'taskflow_access_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _token = signal<string | null>(localStorage.getItem(TOKEN_KEY));
  readonly token = computed(() => this._token());
  readonly isAuthenticated = computed(() => !!this._token());

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

