export type TaskStatus = 'Todo' | 'InProgress' | 'Done'

export interface Task {
  id: string
  title: string
  description: string | null
  status: TaskStatus
  createdAtUtc: string
  updatedAtUtc: string
}

export interface CreateTaskRequest {
  title: string
  description: string | null
}

export interface UpdateTaskRequest {
  title: string
  description: string | null
}
