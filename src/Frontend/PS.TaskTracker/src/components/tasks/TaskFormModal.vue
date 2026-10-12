<template>
  <n-modal
    :show="show"
    preset="card"
    :title="isEdit ? 'Редактировать задачу' : 'Новая задача'"
    style="width: 500px; max-width: 90vw;"
    @update:show="(value) => emit('update:show', value)"
  >
    <n-form ref="formRef" :model="form" :rules="rules" label-placement="top">
      <n-form-item label="Заголовок" path="title">
        <n-input v-model:value="form.title" placeholder="Что нужно сделать?" />
      </n-form-item>
      <n-form-item label="Описание" path="description">
        <n-input
          v-model:value="form.description"
          type="textarea"
          :rows="3"
          placeholder="Описание (необязательно)"
        />
      </n-form-item>
    </n-form>

    <template #footer>
      <div class="modal-footer">
        <n-button @click="emit('update:show', false)">Отмена</n-button>
        <n-button type="primary" @click="onSubmit">Сохранить</n-button>
      </div>
    </template>
  </n-modal>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import {
  NModal,
  NForm,
  NFormItem,
  NInput,
  NButton,
  type FormInst,
  type FormRules,
} from 'naive-ui'
import type { Task } from '@/types/task'

const props = defineProps<{
  show: boolean
  task?: Task | null
}>()

const emit = defineEmits<{
  'update:show': [value: boolean]
  submit: [payload: { title: string; description: string | null }]
}>()

const formRef = ref<FormInst | null>(null)

const form = ref({
  title: '',
  description: '',
})

const rules: FormRules = {
  title: [{ required: true, message: 'Заголовок обязателен', trigger: 'blur' }],
}

const isEdit = computed(() => !!props.task)

watch(
  () => props.show,
  (value) => {
    if (value) {
      form.value = {
        title: props.task?.title ?? '',
        description: props.task?.description ?? '',
      }
    }
  },
)

async function onSubmit() {
  try {
    await formRef.value?.validate()
  } catch {
    return
  }

  emit('submit', {
    title: form.value.title.trim(),
    description: form.value.description.trim() || null,
  })
}
</script>

<style scoped>
.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}
</style>
