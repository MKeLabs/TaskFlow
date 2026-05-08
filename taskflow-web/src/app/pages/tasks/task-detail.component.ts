import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import {
  TaskItemsApi,
  TaskItemDto,
  TASK_STATUS_LABELS,
  TASK_CATEGORY_LABELS,
} from '../../core/api/task-items.api';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.scss'],
})
export class TaskDetailComponent {
  readonly busy = signal(true);
  readonly error = signal<string | null>(null);
  readonly task = signal<TaskItemDto | null>(null);

  readonly statusLabels = TASK_STATUS_LABELS;
  readonly categoryLabels = TASK_CATEGORY_LABELS;

  constructor(
    private readonly api: TaskItemsApi,
    private readonly route: ActivatedRoute
  ) {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.load(id);
  }

  private load(id: number) {
    this.api.getById(id).subscribe({
      next: (item) => this.task.set(item),
      error: () => this.error.set('Task not found or you do not have access.'),
      complete: () => this.busy.set(false),
    });
  }

  getTagNames(task: TaskItemDto): string {
    return task.tags?.length ? task.tags.map((tag) => tag.name).join(', ') : '—';
  }
}
