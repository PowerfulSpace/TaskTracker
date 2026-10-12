import type { TaskStatus } from '@/types/task'

export const taskStatusLabels: Record<TaskStatus, string> = {
  Todo: 'К выполнению',
  InProgress: 'В работе',
  Done: 'Завершено',
}
