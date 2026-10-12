<template>
  <n-tag :type="tagType" size="small">{{ label }}</n-tag>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { NTag } from 'naive-ui'
import { taskStatusLabels } from '@/constants/taskStatus'
import type { TaskStatus } from '@/types/task'

const props = defineProps<{
  status: TaskStatus
}>()

const label = computed(() => taskStatusLabels[props.status])

const tagType = computed<'default' | 'info' | 'success'>(() => {
  switch (props.status) {
    case 'Todo':
      return 'default'
    case 'InProgress':
      return 'info'
    case 'Done':
      return 'success'
    default:
      return 'default'
  }
})
</script>
