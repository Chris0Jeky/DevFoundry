export interface ToolParameterDescriptor {
  name: string
  displayName: string
  description: string
  type: string
  defaultValue?: any
}

export interface ToolDescriptor {
  id: string
  displayName: string
  description: string
  category: string
  tags: string[]
  parameters: ToolParameterDescriptor[]
}

export interface ToolRunRequest {
  text?: string
  secondaryText?: string
  parameters: Record<string, any>
}

export interface ToolRunResult {
  success: boolean
  outputText?: string
  secondaryOutputText?: string
  errorMessage?: string
  metadata: Record<string, any>
}
