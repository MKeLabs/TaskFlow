import { Component, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsApi, ProjectDto, ProjectsDto } from '../../core/api/projects.api';
import { AuthService } from '../../core/auth/auth.service';
import { ModalComponent } from '../../shared/ui/modal/modal.component';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, ModalComponent],
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent {
  readonly busy = signal(true);
  readonly error = signal<string | null>(null);
  readonly projects = signal<ProjectDto[]>([]);
  readonly modalOpen = signal(false);
  readonly modalBusy = signal(false);
  readonly modalError = signal<string | null>(null);
  readonly modalForm = signal<ProjectsDto>({ id: null, name: '' });
  readonly tokenPreview = computed(() => (this.auth.token() ?? '').slice(0, 18));
  readonly modalTitle = computed(() =>
    this.modalForm().id == null ? 'Create project' : 'Edit project'
  );
  readonly modalDescription = computed(() =>
    this.modalForm().id == null
      ? 'Add a project and save it to the API.'
      : 'Update the project name and save changes.'
  );
  readonly modalSaveLabel = computed(() =>
    this.modalForm().id == null ? 'Save' : 'Save changes'
  );

  constructor(
    private readonly api: ProjectsApi,
    private readonly auth: AuthService
  ) {
    this.loadProjects();
  }

  load() {
    this.loadProjects();
  }

  openCreateModal() {
    this.modalError.set(null);
    this.modalForm.set({ id: null, name: '' });
    this.modalOpen.set(true);
  }

  openEditModal(project: ProjectDto) {
    this.modalError.set(null);
    this.modalForm.set({ id: project.id, name: project.name });
    this.modalOpen.set(true);
  }

  closeModal() {
    if (this.modalBusy()) {
      return;
    }
    this.modalOpen.set(false);
  }

  setModalName(name: string) {
    this.modalForm.update((form) => ({ ...form, name }));
  }

  saveModal() {
    this.modalError.set(null);
    const form = this.modalForm();
    const name = form.name.trim();

    if (!name) {
      this.modalError.set('Project name is required.');
      return;
    }

    this.modalBusy.set(true);
    if (form.id == null) {
      this.api.create({ id: null, name }).subscribe({
        next: (created) => {
          this.projects.update((items) => [created, ...items]);
          this.modalOpen.set(false);
        },
        error: () => this.modalError.set('Could not create project.'),
        complete: () => this.modalBusy.set(false)
      });
      return;
    }

    this.api.update({ id: form.id, name }).subscribe({
      next: (updated) => {
        this.projects.update((items) =>
          items.map((item) => (item.id === updated.id ? updated : item))
        );
        this.modalOpen.set(false);
      },
      error: () => this.modalError.set('Could not update project.'),
      complete: () => this.modalBusy.set(false)
    });
  }

  delete(project: ProjectDto) {
    this.error.set(null);

    const confirmed = confirm(`Delete project "${project.name}"?`);
    if (!confirmed) {
      return;
    }

    this.api.delete(project.id).subscribe({
      next: () => {
        this.projects.update((items) => items.filter((p) => p.id !== project.id));
      },
      error: () => this.error.set('Could not delete project.')
    });
  }

  private loadProjects() {
    this.error.set(null);
    this.busy.set(true);

    this.api.getAll().subscribe({
      next: (items) => this.projects.set(items),
      error: () => this.error.set('Could not load projects (are you logged in?).'),
      complete: () => this.busy.set(false)
    });
  }
}

