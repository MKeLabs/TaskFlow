import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import {
  TaskItemsApi,
  TaskItemDto,
  CreateTaskItemRequest,
  TASK_STATUS_LABELS,
  TASK_CATEGORY_LABELS,
} from '../../core/api/task-items.api';
import { ProjectsApi, ProjectDto } from '../../core/api/projects.api';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss'],
})
export class TasksComponent {
  readonly busy = signal(true);
  readonly creating = signal(false);
  readonly error = signal<string | null>(null);
  readonly tasks = signal<TaskItemDto[]>([]);

  readonly newForm = signal<CreateTaskItemRequest>({
    projectId: 0,
    name: '',
    description: null,
    status: 0,
    category: 0,
    dueDate: null,
    tagIds: [],
  });

  readonly projects = signal<ProjectDto[]>([]);

  readonly statusLabels = TASK_STATUS_LABELS;
  readonly categoryLabels = TASK_CATEGORY_LABELS;
  readonly statusOptions = Object.entries(TASK_STATUS_LABELS).map(([v, l]) => ({ value: +v, label: l }));
  readonly categoryOptions = Object.entries(TASK_CATEGORY_LABELS).map(([v, l]) => ({ value: +v, label: l }));

  constructor(
    private readonly api: TaskItemsApi,
    private readonly projectsApi: ProjectsApi,
  ) {
    this.loadProjects();
    this.load();
  }

  loadProjects() {
    this.projectsApi.getAll().subscribe({
      next: (items) => this.projects.set(items),
    });
  }

  load() {
    this.error.set(null);
    this.busy.set(true);

    this.api.getAll().subscribe({
      next: (items) => this.tasks.set(items),
      error: () => this.error.set('Could not load tasks. Are you logged in?'),
      complete: () => this.busy.set(false),
    });
  }

  onProjectIdChange(value: string | number) {
    this.newForm.update((f) => ({
      ...f,
      projectId: Number(value) || 0,
    }));
  }

  onNameChange(value: string) {
    this.newForm.update((f) => ({ ...f, name: value || '' }));
  }

  onDescriptionChange(value: string) {
    this.newForm.update((f) => ({ ...f, description: value || null }));
  }

  onStatusChange(value: string | number) {
    this.newForm.update((f) => ({ ...f, status: Number(value) || 0 }));
  }

  onCategoryChange(value: string | number) {
    this.newForm.update((f) => ({ ...f, category: Number(value) || 0 }));
  }

  createTask() {
    this.error.set(null);
    const form = this.newForm();

    if (!form.projectId || form.projectId <= 0) {
      this.error.set('Please select a project.');
      return;
    }

    if (!form.name.trim()) {
      this.error.set('Task name is required.');
      return;
    }

    const request: CreateTaskItemRequest = {
      ...form,
      name: form.name.trim(),
      description: form.description?.trim() || null,
    };

    this.creating.set(true);
    this.api.create(request).subscribe({
      next: (created) => {
        this.tasks.update((items) => [created, ...items]);
        this.newForm.set({ projectId: 0, name: '', description: null, status: 0, category: 0, dueDate: null, tagIds: [] });
      },
      error: () => this.error.set('Could not create task.'),
      complete: () => this.creating.set(false),
    });
  }
}
