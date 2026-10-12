import { defineStore } from 'pinia'
import { ref } from 'vue'
import { tasksService } from '@/services/api/tasksService'
import type { CreateTaskRequest, Task, UpdateTaskRequest } from '@/types/task'

export const useTasksStore = defineStore('tasks', () => {
  const tasks = ref<Task[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      tasks.value = await tasksService.getAll()
    } catch (e) {
      error.value = 'Не удалось загрузить задачи'
      throw e
    } finally {
      loading.value = false
    }
  }

  async function create(request: CreateTaskRequest) {
    const created = await tasksService.create(request)
    tasks.value.push(created)
    return created
  }

  async function update(id: string, request: UpdateTaskRequest) {
    const updated = await tasksService.update(id, request)
    replaceInList(updated)
    return updated
  }

  async function remove(id: string) {
    await tasksService.remove(id)
    tasks.value = tasks.value.filter((t) => t.id !== id)
  }

  async function start(id: string) {
    const updated = await tasksService.start(id)
    replaceInList(updated)
    return updated
  }

  async function complete(id: string) {
    const updated = await tasksService.complete(id)
    replaceInList(updated)
    return updated
  }

  async function reopen(id: string) {
    const updated = await tasksService.reopen(id)
    replaceInList(updated)
    return updated
  }

  async function reset(id: string) {
    const updated = await tasksService.reset(id)
    replaceInList(updated)
    return updated
  }

  function replaceInList(updated: Task) {
    const idx = tasks.value.findIndex((t) => t.id === updated.id)
    if (idx !== -1) tasks.value[idx] = updated
  }

  return {
    tasks,
    loading,
    error,
    fetchAll,
    create,
    update,
    remove,
    start,
    complete,
    reopen,
    reset,
  }
})
