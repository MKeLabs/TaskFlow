import { Component, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsApi, ProjectDto } from '../../core/api/projects.api';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.scss'
})
export class ProjectsComponent {
  readonly busy = signal(true);
  readonly error = signal<string | null>(null);
  readonly projects = signal<ProjectDto[]>([]);
  readonly tokenPreview = computed(() => (this.auth.token() ?? '').slice(0, 18));

  constructor(
    private readonly api: ProjectsApi,
    private readonly auth: AuthService
  ) {
    this.load();
  }

  load() {
    this.error.set(null);
    this.busy.set(true);

    this.api.getAll().subscribe({
      next: (items) => this.projects.set(items),
      error: () => this.error.set('Could not load projects (are you logged in?).'),
      complete: () => this.busy.set(false)
    });
  }

  logout() {
    this.auth.logout();
    this.projects.set([]);
  }
}

