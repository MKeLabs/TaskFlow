import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import {
  TaskItemsApi,
  TaskItemDto,
  TASK_STATUS_LABELS,
  TASK_CATEGORY_LABELS,
} from '../../core/api/task-items.api';
import { TaskCommentsApi } from '../../core/api/task-comments.api';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.scss'],
})
export class TaskDetailComponent {
  readonly busy = signal(true);
  readonly error = signal<string | null>(null);
  readonly task = signal<TaskItemDto | null>(null);
  readonly newCommentText = signal('');
  readonly addingComment = signal(false);
  readonly deletingCommentId = signal<number | null>(null);

  readonly statusLabels = TASK_STATUS_LABELS;
  readonly categoryLabels = TASK_CATEGORY_LABELS;

  constructor(
    private readonly api: TaskItemsApi,
    private readonly commentsApi: TaskCommentsApi,
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

  addComment() {
    const task = this.task();
    if (!task) return;
    const text = this.newCommentText().trim();
    if (!text) return;

    this.addingComment.set(true);
    this.commentsApi.create({ taskItemId: task.id, text }).subscribe({
      next: (comment) => {
        this.task.update(t => t ? { ...t, comments: [...t.comments, comment] } : t);
        this.newCommentText.set('');
      },
      error: () => this.error.set('Could not add comment.'),
      complete: () => this.addingComment.set(false),
    });
  }

  deleteComment(commentId: number) {
    this.deletingCommentId.set(commentId);
    this.commentsApi.delete(commentId).subscribe({
      next: () => {
        this.task.update(t => t
          ? { ...t, comments: t.comments.filter(c => c.id !== commentId) }
          : t
        );
      },
      error: () => this.error.set('Could not delete comment.'),
      complete: () => this.deletingCommentId.set(null),
    });
  }

  getTagNames(task: TaskItemDto): string {
    return task.tags?.length ? task.tags.map((tag) => tag.name).join(', ') : '—';
  }
}
