import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);

  readonly error = signal<string | null>(null);
  readonly busy = signal(false);

  readonly form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  constructor(
    private readonly auth: AuthService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {}

  submit() {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/projects';
    this.authenticate(
      this.auth.login(this.form.getRawValue()),
      'Login failed. Check your email/password.',
      returnUrl
    );
  }

  register() {
    this.authenticate(
      this.auth.register(this.form.getRawValue()),
      'Register failed. The user may already exist.',
      '/projects'
    );
  }

  private authenticate(
    request$: Observable<unknown>,
    failureMessage: string,
    redirectTo: string
  ) {
    this.error.set(null);

    if (!this.ensureValidForm()) {
      return;
    }

    this.busy.set(true);
    request$.subscribe({
      next: async () => {
        await this.router.navigateByUrl(redirectTo);
      },
      error: () => {
        this.error.set(failureMessage);
        this.busy.set(false);
      },
      complete: () => this.busy.set(false)
    });
  }

  private ensureValidForm(): boolean {
    if (this.form.valid) {
      return true;
    }

    this.form.markAllAsTouched();
    return false;
  }
}

