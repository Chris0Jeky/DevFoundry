<template>
  <div class="app-shell">
    <header class="header">
      <div class="header-content">
        <div class="header-left">
          <h1 class="title">DevFoundry</h1>
          <p class="subtitle">Offline Developer Toolkit</p>
        </div>
        <div class="header-right">
          <ThemeToggle />
        </div>
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
import ThemeToggle from './ThemeToggle.vue'
</script>

<style scoped>
.app-shell {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.header {
  background: var(--color-header-bg);
  color: white;
  padding: 1.5rem 2rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: background 0.3s ease;
}

.header-content {
  max-width: 1400px;
  margin: 0 auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-left {
  flex: 1;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 0.5rem;
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
