import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import {
  TaskItemsApi,
  TaskItemDto,
  CreateTaskItemRequest,
  UpdateTaskItemRequest,
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
  readonly saving = signal(false);
  readonly error = signal<string | null>(null);
  readonly tasks = signal<TaskItemDto[]>([]);
  readonly editing = signal<TaskItemDto | null>(null);
  readonly deletingId = signal<number | null>(null);

  readonly newForm = signal<CreateTaskItemRequest>({
    projectId: 0,
    name: '',
    description: null,
    status: 0,
    category: 0,
    dueDate: null,
    tagIds: [],
  });

  readonly editForm = signal<UpdateTaskItemRequest>({
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

  startEdit(task: TaskItemDto) {
    this.editForm.set({
      projectId: task.projectId,
      name: task.name,
      description: task.description,
      status: task.status,
      category: task.category,
      dueDate: task.dueDate,
      tagIds: task.tags.map(t => t.id),
    });
    this.editing.set(task);
  }

  cancelEdit() {
    this.editing.set(null);
  }

  saveEdit() {
    const task = this.editing();
    if (!task) return;
    this.error.set(null);

    if (!this.editForm().name.trim()) {
      this.error.set('Task name is required.');
      return;
    }

    const request: UpdateTaskItemRequest = {
      ...this.editForm(),
      name: this.editForm().name.trim(),
      description: this.editForm().description?.trim() || null,
    };

    this.saving.set(true);
    this.api.update(task.id, request).subscribe({
      next: () => {
        this.tasks.update(items =>
          items.map(t => t.id === task.id ? { ...t, ...request } : t)
        );
        this.editing.set(null);
      },
      error: () => this.error.set('Could not update task.'),
      complete: () => this.saving.set(false),
    });
  }

  deleteTask(id: number) {
    this.error.set(null);
    this.deletingId.set(id);
    this.api.delete(id).subscribe({
      next: () => this.tasks.update(items => items.filter(t => t.id !== id)),
      error: () => { this.error.set('Could not delete task.'); this.deletingId.set(null); },
      complete: () => this.deletingId.set(null),
    });
  }

  onEditProjectIdChange(value: string | number) {
    this.editForm.update(f => ({ ...f, projectId: Number(value) || 0 }));
  }

  onEditNameChange(value: string) {
    this.editForm.update(f => ({ ...f, name: value || '' }));
  }

  onEditDescriptionChange(value: string) {
    this.editForm.update(f => ({ ...f, description: value || null }));
  }

  onEditStatusChange(value: string | number) {
    this.editForm.update(f => ({ ...f, status: Number(value) || 0 }));
  }

  onEditCategoryChange(value: string | number) {
    this.editForm.update(f => ({ ...f, category: Number(value) || 0 }));
  }
}
