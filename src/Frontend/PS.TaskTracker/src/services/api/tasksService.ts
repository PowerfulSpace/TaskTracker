import { apiClient } from './client'
import type { CreateTaskRequest, Task, UpdateTaskRequest } from '@/types/task'

export const tasksService = {
  async getAll(): Promise<Task[]> {
    const { data } = await apiClient.get<Task[]>('/api/tasks')
    return data
  },

  async getById(id: string): Promise<Task> {
    const { data } = await apiClient.get<Task>(`/api/tasks/${id}`)
    return data
  },

  async create(request: CreateTaskRequest): Promise<Task> {
    const { data } = await apiClient.post<Task>('/api/tasks', request)
    return data
  },

  async update(id: string, request: UpdateTaskRequest): Promise<Task> {
    const { data } = await apiClient.put<Task>(`/api/tasks/${id}`, request)
    return data
  },

  async remove(id: string): Promise<void> {
    await apiClient.delete(`/api/tasks/${id}`)
  },

  async start(id: string): Promise<Task> {
    const { data } = await apiClient.post<Task>(`/api/tasks/${id}/start`)
    return data
  },

  async complete(id: string): Promise<Task> {
    const { data } = await apiClient.post<Task>(`/api/tasks/${id}/complete`)
    return data
  },

  async reopen(id: string): Promise<Task> {
    const { data } = await apiClient.post<Task>(`/api/tasks/${id}/reopen`)
    return data
  },

  async reset(id: string): Promise<Task> {
    const { data } = await apiClient.post<Task>(`/api/tasks/${id}/reset`)
    return data
  },
}
