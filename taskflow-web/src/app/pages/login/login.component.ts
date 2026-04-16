import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
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
    this.error.set(null);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.busy.set(true);
    this.auth.login(this.form.getRawValue()).subscribe({
      next: async () => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/projects';
        await this.router.navigateByUrl(returnUrl);
      },
      error: () => {
        this.error.set('Login failed. Check your email/password.');
        this.busy.set(false);
      },
      complete: () => this.busy.set(false)
    });
  }

  register() {
    this.error.set(null);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.busy.set(true);
    this.auth.register(this.form.getRawValue()).subscribe({
      next: async () => {
        await this.router.navigateByUrl('/projects');
      },
      error: () => {
        this.error.set('Register failed. The user may already exist.');
        this.busy.set(false);
      },
      complete: () => this.busy.set(false)
    });
  }
}

