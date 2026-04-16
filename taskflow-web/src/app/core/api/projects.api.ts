import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface ProjectDto {
  id: number;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class ProjectsApi {
  constructor(private readonly http: HttpClient) {}

  getAll() {
    return this.http.get<ProjectDto[]>('/api/projects');
  }
}

