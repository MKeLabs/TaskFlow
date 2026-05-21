import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface CommentDto {
  id: number;
  taskItemId: number;
  text: string;
}

export interface CreateCommentRequest {
  taskItemId: number;
  text: string;
}

@Injectable({ providedIn: 'root' })
export class TaskCommentsApi {
  constructor(private readonly http: HttpClient) {}

  create(request: CreateCommentRequest) {
    return this.http.post<CommentDto>('/api/taskcomments', request);
  }

  delete(id: number) {
    return this.http.delete<void>(`/api/taskcomments/${id}`);
  }
}
