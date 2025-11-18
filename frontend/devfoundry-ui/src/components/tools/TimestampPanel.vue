<template>
  <div class="tool-panel">
    <h2 class="panel-title">{{ tool.displayName }}</h2>
    <p class="panel-description">{{ tool.description }}</p>

    <div class="panel-content">
      <div class="option">
        <label class="label">Mode</label>
        <select v-model="mode" class="select">
          <option value="from-unix">Unix → Human-readable</option>
          <option value="to-unix">Human-readable → Unix</option>
        </select>
      </div>

      <div v-if="mode === 'from-unix'" class="option">
        <label class="checkbox-label">
          <input type="checkbox" v-model="useMilliseconds" />
          Use milliseconds (instead of seconds)
        </label>
      </div>

      <div class="form-section">
        <label class="label">Input</label>
        <textarea
          v-model="inputText"
          class="textarea"
          rows="4"
          :placeholder="mode === 'from-unix' ? 'Unix timestamp (e.g., 1704067200)' : 'Date/time (e.g., 2024-01-01 12:00:00)'"
        ></textarea>
      </div>

      <button @click="runTool" class="button" :disabled="!inputText || isLoading">
        {{ isLoading ? 'Converting...' : 'Convert' }}
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
const mode = ref('from-unix')
const useMilliseconds = ref(false)
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
      parameters: {
        mode: mode.value,
        useMilliseconds: useMilliseconds.value
      }
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
