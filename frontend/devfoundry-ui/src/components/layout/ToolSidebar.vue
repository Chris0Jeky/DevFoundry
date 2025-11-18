<template>
  <div class="tool-sidebar">
    <div class="search-box">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search tools..."
        class="search-input"
      />
    </div>

    <div v-if="toolsStore.isLoading" class="loading">
      Loading tools...
    </div>

    <div v-else-if="toolsStore.error" class="error">
      {{ toolsStore.error }}
    </div>

    <div v-else class="tools-list">
      <div
        v-for="(tools, category) in filteredToolsByCategory"
        :key="category"
        class="category-group"
      >
        <h3 class="category-title">{{ category }}</h3>
        <div
          v-for="tool in tools"
          :key="tool.id"
          :class="['tool-item', { active: toolsStore.selectedToolId === tool.id }]"
          @click="selectTool(tool.id)"
        >
          <div class="tool-name">{{ tool.displayName }}</div>
          <div class="tool-description">{{ tool.description }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useToolsStore } from '@/stores/toolsStore'

const router = useRouter()
const toolsStore = useToolsStore()
const searchQuery = ref('')

const filteredToolsByCategory = computed(() => {
  const query = searchQuery.value.toLowerCase()
  if (!query) {
    return toolsStore.toolsByCategory
  }

  const filtered: Record<string, any[]> = {}
  Object.entries(toolsStore.toolsByCategory).forEach(([category, tools]) => {
    const matchingTools = tools.filter(tool =>
      tool.displayName.toLowerCase().includes(query) ||
      tool.description.toLowerCase().includes(query) ||
      tool.tags.some(tag => tag.toLowerCase().includes(query))
    )
    if (matchingTools.length > 0) {
      filtered[category] = matchingTools
    }
  })
  return filtered
})

function selectTool(toolId: string) {
  toolsStore.selectTool(toolId)
  router.push({ name: 'tool', params: { id: toolId } })
}
</script>

<style scoped>
.tool-sidebar {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.search-box {
  padding: 1rem;
  border-bottom: 1px solid var(--color-border);
}

.search-input {
  width: 100%;
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--color-border);
  border-radius: 0.375rem;
  font-size: 0.875rem;
  outline: none;
}

.search-input:focus {
  border-color: var(--color-primary);
}

.loading,
.error {
  padding: 1rem;
  text-align: center;
  color: var(--color-text-secondary);
}

.error {
  color: #dc2626;
}

.tools-list {
  flex: 1;
  overflow-y: auto;
  padding: 0.5rem 0;
}

.category-group {
  margin-bottom: 1rem;
}

.category-title {
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  color: var(--color-text-secondary);
  padding: 0.5rem 1rem;
  margin: 0;
}

.tool-item {
  padding: 0.75rem 1rem;
  cursor: pointer;
  transition: background-color 0.15s;
}

.tool-item:hover {
  background-color: #f3f4f6;
}

.tool-item.active {
  background-color: #dbeafe;
  border-left: 3px solid var(--color-primary);
}

.tool-name {
  font-weight: 500;
  font-size: 0.875rem;
  color: var(--color-text);
}

.tool-description {
  font-size: 0.75rem;
  color: var(--color-text-secondary);
  margin-top: 0.25rem;
}
</style>
