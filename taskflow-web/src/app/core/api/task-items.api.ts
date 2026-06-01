import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface TaskItemDto {
  id: number;
  projectId: number;
  projectName: string;
  name: string;
  description: string | null;
  status: number;
  category: number;
  dueDate: string | null;
  tags: { id: number; name: string }[];
  comments: { id: number; taskItemId: number; text: string; createdByUserId: string | null }[];
}

export interface CreateTaskItemRequest {
  projectId: number;
  name: string;
  description: string | null;
  status: number;
  category: number;
  dueDate: string | null;
  tagIds: number[];
}

export interface UpdateTaskItemRequest {
  projectId: number;
  name: string;
  description: string | null;
  status: number;
  category: number;
  dueDate: string | null;
  tagIds: number[];
}

export const TASK_STATUS_LABELS: Record<number, string> = {
  0: 'Todo',
  1: 'In Progress',
  2: 'Done',
  3: 'Blocked',
};

export const TASK_CATEGORY_LABELS: Record<number, string> = {
  0: 'General',
  1: 'Bug',
  2: 'Feature',
  3: 'Chore',
};

@Injectable({ providedIn: 'root' })
export class TaskItemsApi {
  constructor(private readonly http: HttpClient) {}

  getAll() {
    return this.http.get<TaskItemDto[]>('/api/taskitems');
  }

  getById(id: number) {
    return this.http.get<TaskItemDto>(`/api/taskitems/${id}`);
  }

  create(request: CreateTaskItemRequest) {
    return this.http.post<TaskItemDto>('/api/taskitems', request);
  }

  update(id: number, request: UpdateTaskItemRequest) {
    return this.http.put<void>(`/api/taskitems/${id}`, request);
  }

  delete(id: number) {
    return this.http.delete<void>(`/api/taskitems/${id}`);
  }
}
