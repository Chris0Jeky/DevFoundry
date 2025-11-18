<template>
  <div class="tool-panel-container">
    <div v-if="!toolsStore.selectedTool" class="welcome">
      <h2>Welcome to DevFoundry</h2>
      <p>Select a tool from the sidebar to get started.</p>
      <div class="features">
        <div class="feature">
          <h3>Offline First</h3>
          <p>All tools work completely offline</p>
        </div>
        <div class="feature">
          <h3>Privacy Focused</h3>
          <p>Your data never leaves your machine</p>
        </div>
        <div class="feature">
          <h3>Developer Friendly</h3>
          <p>Built by developers, for developers</p>
        </div>
      </div>
    </div>

    <component
      v-else
      :is="currentComponent"
      :tool="toolsStore.selectedTool"
    />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useToolsStore } from '@/stores/toolsStore'
import JsonFormatterPanel from './JsonFormatterPanel.vue'
import JsonYamlConverterPanel from './JsonYamlConverterPanel.vue'
import Base64Panel from './Base64Panel.vue'
import UuidPanel from './UuidPanel.vue'
import HashPanel from './HashPanel.vue'
import TimestampPanel from './TimestampPanel.vue'
import StringCasePanel from './StringCasePanel.vue'
import UrlEncoderPanel from './UrlEncoderPanel.vue'
import JwtPanel from './JwtPanel.vue'

const toolsStore = useToolsStore()

const toolComponents: Record<string, any> = {
  'json.formatter': JsonFormatterPanel,
  'json.yaml': JsonYamlConverterPanel,
  'encoding.base64': Base64Panel,
  'generation.uuid': UuidPanel,
  'crypto.hash': HashPanel,
  'time.timestamp': TimestampPanel,
  'text.case': StringCasePanel,
  'encoding.url': UrlEncoderPanel,
  'crypto.jwt': JwtPanel
}

const currentComponent = computed(() => {
  if (!toolsStore.selectedTool) return null
  return toolComponents[toolsStore.selectedTool.id] || null
})
</script>

<style scoped>
.tool-panel-container {
  max-width: 900px;
}

.welcome {
  text-align: center;
  padding: 3rem 1rem;
}

.welcome h2 {
  font-size: 2rem;
  margin-bottom: 1rem;
  color: var(--color-text);
}

.welcome > p {
  font-size: 1.125rem;
  color: var(--color-text-secondary);
  margin-bottom: 3rem;
}

.features {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 2rem;
  margin-top: 2rem;
}

.feature {
  padding: 1.5rem;
  background: var(--color-surface);
  border-radius: 0.5rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.feature h3 {
  font-size: 1.125rem;
  margin: 0 0 0.5rem 0;
  color: var(--color-primary);
}

.feature p {
  font-size: 0.875rem;
  color: var(--color-text-secondary);
  margin: 0;
}
</style>
