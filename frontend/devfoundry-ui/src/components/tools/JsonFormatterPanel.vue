<template>
  <div class="tool-panel">
    <h2 class="panel-title">{{ tool.displayName }}</h2>
    <p class="panel-description">{{ tool.description }}</p>

    <div class="panel-content">
      <div class="form-section">
        <label class="label">Input JSON</label>
        <textarea
          v-model="inputText"
          class="textarea"
          rows="10"
          placeholder='{"name": "example", "value": 123}'
        ></textarea>
      </div>

      <div class="options">
        <div class="option">
          <label class="label">Indent Size</label>
          <input
            v-model.number="indentSize"
            type="number"
            min="1"
            max="8"
            class="input"
          />
        </div>
        <div class="option">
          <label class="checkbox-label">
            <input v-model="minify" type="checkbox" class="checkbox" />
            Minify
          </label>
        </div>
      </div>

      <button @click="runTool" class="button" :disabled="!inputText || isLoading">
        {{ isLoading ? 'Processing...' : 'Format JSON' }}
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
          rows="10"
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
const indentSize = ref(2)
const minify = ref(false)
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
        indentSize: indentSize.value,
        minify: minify.value
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
