<template>
  <div class="tool-panel">
    <h2 class="panel-title">{{ tool.displayName }}</h2>
    <p class="panel-description">{{ tool.description }}</p>

    <div class="panel-content">
      <div class="option">
        <label class="label">Mode</label>
        <select v-model="mode" class="select">
          <option value="encode">Encode</option>
          <option value="decode">Decode</option>
        </select>
      </div>

      <div class="form-section">
        <label class="label">Input</label>
        <textarea
          v-model="inputText"
          class="textarea"
          rows="8"
          :placeholder="mode === 'encode' ? 'Text to encode...' : 'Base64 string to decode...'"
        ></textarea>
      </div>

      <button @click="runTool" class="button" :disabled="!inputText || isLoading">
        {{ isLoading ? 'Processing...' : mode === 'encode' ? 'Encode' : 'Decode' }}
      </button>

      <div v-if="error" class="error-message">
        {{ error }}
      </div>

      <div v-if="outputText" class="form-section">
        <div class="label-row">
          <label class="label">Output</label>
          <button @click="copyToClipboard" class="copy-button">Copy</button>
        </div>
        <textarea
          v-model="outputText"
          class="textarea"
          rows="8"
          readonly
        ></textarea>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useToolsStore } from '@/stores/toolsStore'
import type { ToolDescriptor } from '@/types/tool'

const props = defineProps<{
  tool: ToolDescriptor
}>()

const toolsStore = useToolsStore()
const inputText = ref('')
const outputText = ref('')
const mode = ref('encode')
const isLoading = ref(false)
const error = ref('')

async function runTool() {
  if (!inputText.value) return

  isLoading.value = true
  error.value = ''
  outputText.value = ''

  try {
    const result = await toolsStore.runTool({
      text: inputText.value,
      parameters: { mode: mode.value }
    })

    if (result.success) {
      outputText.value = result.outputText || ''
    } else {
      error.value = result.errorMessage || 'Unknown error'
    }
  } catch (e: any) {
    error.value = e.message || 'Failed to run tool'
  } finally {
    isLoading.value = false
  }
}

function copyToClipboard() {
  navigator.clipboard.writeText(outputText.value)
}
</script>

<style scoped src="./tool-panel-styles.css"></style>
