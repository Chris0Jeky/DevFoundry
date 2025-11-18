import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useUiStore = defineStore('ui', () => {
  const searchQuery = ref('')
  const sidebarCollapsed = ref(false)

  function setSearchQuery(query: string) {
    searchQuery.value = query
  }

  function toggleSidebar() {
    sidebarCollapsed.value = !sidebarCollapsed.value
  }

  return {
    searchQuery,
    sidebarCollapsed,
    setSearchQuery,
    toggleSidebar
  }
})
