<template>
  <div class="home-view">
    <AppShell />
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useToolsStore } from '@/stores/toolsStore'
import AppShell from '@/components/layout/AppShell.vue'

const route = useRoute()
const toolsStore = useToolsStore()

onMounted(async () => {
  await toolsStore.fetchTools()

  // If there's a tool ID in the route, select it
  if (route.params.id && typeof route.params.id === 'string') {
    toolsStore.selectTool(route.params.id)
  }
})
</script>

<style scoped>
.home-view {
  min-height: 100vh;
}
</style>
