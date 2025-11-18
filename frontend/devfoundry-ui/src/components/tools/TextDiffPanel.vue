<template>
  <div class="tool-panel">
    <h2 class="panel-title">{{ tool.displayName }}</h2>
    <p class="panel-description">{{ tool.description }}</p>

    <div class="panel-content">
      <div class="options-row">
        <div class="option">
          <label class="label">Format</label>
          <select v-model="format" class="select">
            <option value="unified">Unified</option>
            <option value="sidebyside">Side by Side</option>
          </select>
        </div>

        <div class="option">
          <label class="checkbox-label">
            <input type="checkbox" v-model="ignoreWhitespace" />
            Ignore Whitespace
          </label>
        </div>

        <div class="option">
          <label class="checkbox-label">
            <input type="checkbox" v-model="ignoreCase" />
            Ignore Case
          </label>
        </div>
      </div>

      <div class="diff-inputs">
        <div class="form-section">
          <label class="label">Original Text</label>
          <textarea
            v-model="originalText"
            class="textarea"
            rows="10"
            placeholder="Paste original text here..."
          ></textarea>
        </div>

        <div class="form-section">
          <label class="label">Modified Text</label>
          <textarea
            v-model="modifiedText"
            class="textarea"
            rows="10"
            placeholder="Paste modified text here..."
          ></textarea>
        </div>
      </div>

      <button
        @click="runTool"
        class="button"
        :disabled="!originalText || !modifiedText || isLoading"
      >
        {{ isLoading ? 'Comparing...' : 'Compare' }}
      </button>

      <div v-if="error" class="error-message">
        {{ error }}
      </div>

      <div v-if="outputText" class="form-section">
        <div class="label-row">
          <label class="label">Diff Result</label>
          <button @click="copyToClipboard" class="copy-button">Copy</button>
        </div>
        <pre class="diff-output">{{ outputText }}</pre>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useToolsStore } from '@/stores/toolsStore'
import type { ToolDescriptor, ToolRunRequest } from '@/types/tool'

const props = defineProps<{
  tool: ToolDescriptor
}>()

const toolsStore = useToolsStore()
const originalText = ref('')
const modifiedText = ref('')
const outputText = ref('')
const format = ref('unified')
const ignoreWhitespace = ref(false)
const ignoreCase = ref(false)
const isLoading = ref(false)
const error = ref('')

async function runTool() {
  if (!originalText.value || !modifiedText.value) return

  isLoading.value = true
  error.value = ''
  outputText.value = ''

  try {
    // For diff tool, we need both text and secondaryText
    const result = await toolsStore.runTool({
      text: originalText.value,
      secondaryText: modifiedText.value,
      parameters: {
        format: format.value,
        ignoreWhitespace: ignoreWhitespace.value,
        ignoreCase: ignoreCase.value
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

<style scoped>
@import './tool-panel-styles.css';

.options-row {
  display: flex;
  gap: 1.5rem;
  flex-wrap: wrap;
  margin-bottom: 1rem;
}

.diff-inputs {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-bottom: 1rem;
}

.diff-output {
  font-family: 'Courier New', Courier, monospace;
  background-color: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: 0.375rem;
  padding: 1rem;
  overflow-x: auto;
  white-space: pre-wrap;
  font-size: 0.875rem;
  line-height: 1.5;
  color: var(--color-text);
  max-height: 500px;
  overflow-y: auto;
}

@media (max-width: 768px) {
  .diff-inputs {
    grid-template-columns: 1fr;
  }
}
</style>
