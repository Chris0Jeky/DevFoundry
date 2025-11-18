<template>
  <div class="tool-panel">
    <h2 class="panel-title">{{ tool.displayName }}</h2>
    <p class="panel-description">{{ tool.description }}</p>

    <div class="panel-content">
      <div class="options">
        <div class="option">
          <label class="label">Count</label>
          <input
            v-model.number="count"
            type="number"
            min="1"
            max="100"
            class="input"
          />
        </div>
        <div class="option">
          <label class="checkbox-label">
            <input v-model="uppercase" type="checkbox" class="checkbox" />
            Uppercase
          </label>
        </div>
      </div>

      <button @click="runTool" class="button" :disabled="isLoading">
        {{ isLoading ? 'Generating...' : 'Generate UUIDs' }}
      </button>

      <div v-if="error" class="error-message">
        {{ error }}
      </div>

      <div v-if="outputText" class="form-section">
        <div class="label-row">
          <label class="label">Generated UUIDs</label>
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
const outputText = ref('')
const count = ref(1)
const uppercase = ref(false)
const isLoading = ref(false)
const error = ref('')

async function runTool() {
  isLoading.value = true
  error.value = ''
  outputText.value = ''

  try {
    const result = await toolsStore.runTool({
      parameters: {
        count: count.value,
        uppercase: uppercase.value
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
