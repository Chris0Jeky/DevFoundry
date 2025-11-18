import http from './http'
import type { ToolDescriptor, ToolRunRequest, ToolRunResult } from '@/types/tool'

export const toolsApi = {
  async getTools(): Promise<ToolDescriptor[]> {
    const response = await http.get<ToolDescriptor[]>('/api/tools')
    return response.data
  },

  async runTool(toolId: string, request: ToolRunRequest): Promise<ToolRunResult> {
    const response = await http.post<ToolRunResult>(`/api/tools/${toolId}/run`, request)
    return response.data
  }
}
