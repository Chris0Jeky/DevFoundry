import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { toolsApi } from '@/api/toolsApi'
import type { ToolDescriptor, ToolRunRequest, ToolRunResult } from '@/types/tool'

export const useToolsStore = defineStore('tools', () => {
  const tools = ref<ToolDescriptor[]>([])
  const selectedToolId = ref<string | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const selectedTool = computed(() => {
    if (!selectedToolId.value) return null
    return tools.value.find(t => t.id === selectedToolId.value) || null
  })

  const toolsByCategory = computed(() => {
    const grouped: Record<string, ToolDescriptor[]> = {}
    tools.value.forEach(tool => {
      if (!grouped[tool.category]) {
        grouped[tool.category] = []
      }
      grouped[tool.category].push(tool)
    })
    return grouped
  })

  async function fetchTools() {
    isLoading.value = true
    error.value = null
    try {
      tools.value = await toolsApi.getTools()
    } catch (e: any) {
      error.value = e.message || 'Failed to fetch tools'
      console.error('Error fetching tools:', e)
    } finally {
      isLoading.value = false
    }
  }

  function selectTool(toolId: string | null) {
    selectedToolId.value = toolId
  }

  async function runTool(request: ToolRunRequest): Promise<ToolRunResult> {
    if (!selectedToolId.value) {
      throw new Error('No tool selected')
    }

    isLoading.value = true
    error.value = null
    try {
      const result = await toolsApi.runTool(selectedToolId.value, request)
      return result
    } catch (e: any) {
      error.value = e.message || 'Failed to run tool'
      throw e
    } finally {
      isLoading.value = false
    }
  }

  return {
    tools,
    selectedToolId,
    selectedTool,
    toolsByCategory,
    isLoading,
    error,
    fetchTools,
    selectTool,
    runTool
  }
})
