import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProjectDto {
  id: number;
  name: string;
}

export interface ProjectsDto {
  id: number | null;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class ProjectsApi {
  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<ProjectDto[]> {
    return this.http.get<ProjectDto[]>('/api/projects');
  }

  create(request: ProjectsDto): Observable<ProjectDto> {
    // Backend expects { name }; `id` is ignored for create.
    return this.http.post<ProjectDto>('/api/projects', { name: request.name });
  }

  update(request: ProjectsDto): Observable<ProjectDto> {
    if (request.id == null) {
      throw new Error('ProjectsApi.update requires a non-null id.');
    }

    // Backend expects { name } at PUT /api/projects/:id.
    return this.http.put<ProjectDto>(`/api/projects/${request.id}`, { name: request.name });
  }

  delete(projectId: number): Observable<void> {
    return this.http.delete<void>(`/api/projects/${projectId}`);
  }
}

