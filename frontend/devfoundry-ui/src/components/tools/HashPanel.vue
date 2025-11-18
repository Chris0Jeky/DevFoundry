<template>
  <div class="tool-panel">
    <h2 class="panel-title">{{ tool.displayName }}</h2>
    <p class="panel-description">{{ tool.description }}</p>

    <div class="panel-content">
      <div class="form-section">
        <label class="label">Input Text</label>
        <textarea
          v-model="inputText"
          class="textarea"
          rows="6"
          placeholder="Text to hash..."
        ></textarea>
      </div>

      <div class="options">
        <div class="option">
          <label class="label">Algorithm</label>
          <select v-model="algorithm" class="select">
            <option value="md5">MD5</option>
            <option value="sha1">SHA-1</option>
            <option value="sha256">SHA-256</option>
            <option value="sha512">SHA-512</option>
          </select>
        </div>
        <div class="option">
          <label class="checkbox-label">
            <input v-model="uppercase" type="checkbox" class="checkbox" />
            Uppercase
          </label>
        </div>
      </div>

      <button @click="runTool" class="button" :disabled="!inputText || isLoading">
        {{ isLoading ? 'Calculating...' : 'Calculate Hash' }}
      </button>

      <div v-if="error" class="error-message">
        {{ error }}
      </div>

      <div v-if="outputText" class="form-section">
        <div class="label-row">
          <label class="label">Hash</label>
          <button @click="copyToClipboard" class="copy-button">Copy</button>
        </div>
        <textarea
          v-model="outputText"
          class="textarea"
          rows="3"
          readonly
        ></textarea>
        <div v-if="metadata" class="result-metadata">
          Algorithm: {{ metadata.algorithm }} | Length: {{ metadata.length }} bits
        </div>
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
const algorithm = ref('sha256')
const uppercase = ref(false)
const isLoading = ref(false)
const error = ref('')
const metadata = ref<any>(null)

async function runTool() {
  if (!inputText.value) return

  isLoading.value = true
  error.value = ''
  outputText.value = ''
  metadata.value = null

  try {
    const result = await toolsStore.runTool({
      text: inputText.value,
      parameters: {
        algorithm: algorithm.value,
        uppercase: uppercase.value
      }
    })

    if (result.success) {
      outputText.value = result.outputText || ''
      metadata.value = result.metadata
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
