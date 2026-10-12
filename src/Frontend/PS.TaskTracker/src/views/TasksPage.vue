<template>
  <div class="tasks-page">
    <div class="header">
      <h1>Задачи</h1>
      <n-button type="primary" @click="openCreate">Создать задачу</n-button>
    </div>

    <n-spin :show="store.loading">
      <div v-if="store.tasks.length === 0" class="empty">Задач пока нет</div>

      <n-list v-else bordered>
        <n-list-item v-for="task in store.tasks" :key="task.id">
          <div class="task-item">
            <div class="task-info">
              <div class="task-title">{{ task.title }}</div>
              <div v-if="task.description" class="task-description">
                {{ task.description }}
              </div>
            </div>

            <div class="task-actions">
              <task-status-tag :status="task.status" />

              <n-button
                v-if="task.status === 'Todo'"
                size="small"
                type="primary"
                @click="onAction(() => store.start(task.id))"
              >
                Начать
              </n-button>

              <n-button
                v-if="task.status === 'InProgress'"
                size="small"
                type="primary"
                @click="onAction(() => store.complete(task.id))"
              >
                Завершить
              </n-button>

              <n-button
                v-if="task.status === 'InProgress'"
                size="small"
                @click="onAction(() => store.reset(task.id))"
              >
                В Todo
              </n-button>

              <n-button
                v-if="task.status === 'Done'"
                size="small"
                @click="onAction(() => store.reopen(task.id))"
              >
                Переоткрыть
              </n-button>

              <n-button size="small" @click="openEdit(task)">Изменить</n-button>
              <n-button size="small" type="error" @click="confirmDelete(task)">
                Удалить
              </n-button>
            </div>
          </div>
        </n-list-item>
      </n-list>
    </n-spin>

    <task-form-modal
      v-model:show="showForm"
      :task="editingTask"
      @submit="onSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import {
  NButton,
  NList,
  NListItem,
  NSpin,
  useDialog,
  useMessage,
} from 'naive-ui'
import { useTasksStore } from '@/stores/tasks'
import type { Task } from '@/types/task'
import TaskStatusTag from '@/components/tasks/TaskStatusTag.vue'
import TaskFormModal from '@/components/tasks/TaskFormModal.vue'
import { extractErrorMessage } from '@/utils/extractErrorMessage'

const store = useTasksStore()
const message = useMessage()
const dialog = useDialog()

const showForm = ref(false)
const editingTask = ref<Task | null>(null)

onMounted(async () => {
  try {
    await store.fetchAll()
  } catch {
    message.error('Не удалось загрузить задачи')
  }
})

function openCreate() {
  editingTask.value = null
  showForm.value = true
}

function openEdit(task: Task) {
  editingTask.value = task
  showForm.value = true
}

async function onSubmit(payload: { title: string; description: string | null }) {
  try {
    if (editingTask.value) {
      await store.update(editingTask.value.id, payload)
      message.success('Задача обновлена')
    } else {
      await store.create(payload)
      message.success('Задача создана')
    }
    showForm.value = false
  } catch (e) {
    message.error(extractErrorMessage(e))
  }
}

function confirmDelete(task: Task) {
  dialog.warning({
    title: 'Удалить задачу?',
    content: `Задача «${task.title}» будет удалена.`,
    positiveText: 'Удалить',
    negativeText: 'Отмена',
    onPositiveClick: async () => {
      try {
        await store.remove(task.id)
        message.success('Задача удалена')
      } catch (e) {
        message.error(extractErrorMessage(e))
      }
    },
  })
}

async function onAction(action: () => Promise<unknown>) {
  try {
    await action()
  } catch (e) {
    message.error(extractErrorMessage(e))
  }
}
</script>

<style scoped>
.tasks-page {
  max-width: 900px;
  margin: 0 auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.empty {
  text-align: center;
  padding: 40px;
  color: #999;
}

.task-item {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 20px;
}

.task-info {
  flex: 1;
  min-width: 0;
}

.task-title {
  font-weight: 600;
  margin-bottom: 4px;
}

.task-description {
  color: #666;
  font-size: 14px;
}

.task-actions {
  display: flex;
  align-items: center;
  gap: 8px;
  flex-wrap: wrap;
}
</style>
