import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (_route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.isAuthenticated()) {
    return true;
  }

  // Direct visits to /projects show the "unauthorized" page instead of login.
  if (state.url.startsWith('/projects')) {
    return router.createUrlTree(['/unauthorized']);
  }

  return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
};

