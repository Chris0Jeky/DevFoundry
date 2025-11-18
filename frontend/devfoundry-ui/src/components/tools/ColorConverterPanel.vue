<template>
  <div class="tool-panel">
    <h2 class="panel-title">{{ tool.displayName }}</h2>
    <p class="panel-description">{{ tool.description }}</p>

    <div class="panel-content">
      <div class="option">
        <label class="label">Target Format</label>
        <select v-model="targetFormat" class="select">
          <option value="hex">HEX (#RRGGBB)</option>
          <option value="rgb">RGB (rgb(r, g, b))</option>
          <option value="rgba">RGBA (rgba(r, g, b, a))</option>
          <option value="hsl">HSL (hsl(h, s%, l%))</option>
          <option value="hsla">HSLA (hsla(h, s%, l%, a))</option>
        </select>
      </div>

      <div class="form-section">
        <label class="label">Input Color</label>
        <input
          v-model="inputText"
          type="text"
          class="input"
          placeholder="e.g., #FF5733, rgb(255, 87, 51), hsl(11, 100%, 60%)"
          @input="updatePreview"
        />
        <div v-if="inputColorPreview" class="color-preview-container">
          <div
            class="color-preview"
            :style="{ backgroundColor: inputColorPreview }"
            :title="inputColorPreview"
          ></div>
          <span class="color-preview-label">Input Preview</span>
        </div>
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
        <input
          v-model="outputText"
          type="text"
          class="input"
          readonly
        />
        <div class="color-preview-container">
          <div
            class="color-preview"
            :style="{ backgroundColor: outputText }"
            :title="outputText"
          ></div>
          <span class="color-preview-label">Output Preview</span>
        </div>
      </div>

      <!-- Quick color picker -->
      <div class="form-section">
        <label class="label">Or pick a color</label>
        <input
          type="color"
          v-model="pickedColor"
          @change="usePickedColor"
          class="color-picker"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useToolsStore } from '@/stores/toolsStore'
import type { ToolDescriptor } from '@/types/tool'

const props = defineProps<{
  tool: ToolDescriptor
}>()

const toolsStore = useToolsStore()
const inputText = ref('')
const outputText = ref('')
const targetFormat = ref('rgb')
const isLoading = ref(false)
const error = ref('')
const pickedColor = ref('#FF5733')
const inputColorPreview = ref('')

async function runTool() {
  if (!inputText.value) return

  isLoading.value = true
  error.value = ''
  outputText.value = ''

  try {
    const result = await toolsStore.runTool({
      text: inputText.value,
      parameters: { targetFormat: targetFormat.value }
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

function updatePreview() {
  try {
    // Try to use the input as a color preview
    const color = inputText.value.trim()
    if (color) {
      inputColorPreview.value = color
    }
  } catch {
    inputColorPreview.value = ''
  }
}

function usePickedColor() {
  inputText.value = pickedColor.value
  updatePreview()
}

function copyToClipboard() {
  navigator.clipboard.writeText(outputText.value)
}
</script>

<style scoped>
@import './tool-panel-styles.css';

.color-preview-container {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-top: 0.75rem;
}

.color-preview {
  width: 80px;
  height: 80px;
  border-radius: 0.5rem;
  border: 2px solid var(--color-border);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.color-preview-label {
  font-size: 0.875rem;
  color: var(--color-text-secondary);
}

.color-picker {
  width: 120px;
  height: 50px;
  border: 2px solid var(--color-border);
  border-radius: 0.5rem;
  cursor: pointer;
}

.color-picker::-webkit-color-swatch-wrapper {
  padding: 0;
}

.color-picker::-webkit-color-swatch {
  border: none;
  border-radius: 0.375rem;
}
</style>
