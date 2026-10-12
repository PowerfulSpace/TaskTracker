import axios from 'axios'
import type { ProblemDetails } from '@/types/problemDetails'

export function extractErrorMessage(error: unknown): string {
  if (axios.isAxiosError(error) && error.response?.data) {
    const data = error.response.data as ProblemDetails
    if (data.detail) return data.detail
    if (data.errors) {
      const first = Object.values(data.errors)[0]
      if (first && first.length > 0) return first[0]!
    }
  }
  return 'Произошла ошибка'
}
