import { HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const token = auth.token();

  if (shouldSkipAuthorization(req.url) || !token) {
    return next(req);
  }

  return next(withAuthorizationHeader(req, token));
};

function shouldSkipAuthorization(url: string): boolean {
  return url.startsWith('/assets');
}

function withAuthorizationHeader(req: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
  return req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });
}

