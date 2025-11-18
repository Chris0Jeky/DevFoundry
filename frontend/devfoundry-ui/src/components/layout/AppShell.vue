<template>
  <div class="app-shell">
    <header class="header">
      <div class="header-content">
        <h1 class="title">DevFoundry</h1>
        <p class="subtitle">Offline Developer Toolkit</p>
      </div>
    </header>

    <div class="main-container">
      <ToolSidebar class="sidebar" />
      <main class="main-content">
        <ToolPanelContainer />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import ToolSidebar from './ToolSidebar.vue'
import ToolPanelContainer from '@/components/tools/ToolPanelContainer.vue'
</script>

<style scoped>
.app-shell {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.header {
  background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%);
  color: white;
  padding: 1.5rem 2rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.header-content {
  max-width: 1400px;
  margin: 0 auto;
}

.title {
  font-size: 1.75rem;
  font-weight: 700;
  margin: 0;
}

.subtitle {
  font-size: 0.875rem;
  margin: 0.25rem 0 0 0;
  opacity: 0.9;
}

.main-container {
  flex: 1;
  display: flex;
  max-width: 1400px;
  margin: 0 auto;
  width: 100%;
}

.sidebar {
  width: 280px;
  flex-shrink: 0;
  border-right: 1px solid var(--color-border);
  background: var(--color-surface);
}

.main-content {
  flex: 1;
  padding: 2rem;
  overflow-y: auto;
}
</style>
