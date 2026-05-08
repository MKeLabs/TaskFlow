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

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.scss',
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

  readonly statusLabels = TASK_STATUS_LABELS;
  readonly categoryLabels = TASK_CATEGORY_LABELS;
  readonly statusOptions = Object.entries(TASK_STATUS_LABELS).map(([v, l]) => ({ value: +v, label: l }));
  readonly categoryOptions = Object.entries(TASK_CATEGORY_LABELS).map(([v, l]) => ({ value: +v, label: l }));

  constructor(private readonly api: TaskItemsApi) {
    this.load();
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

  createTask() {
    this.error.set(null);
    const form = this.newForm();

    if (!form.projectId || form.projectId <= 0) {
      this.error.set('Project ID is required.');
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
