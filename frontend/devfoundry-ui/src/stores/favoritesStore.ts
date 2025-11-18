import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export interface ToolUsage {
  toolId: string
  timestamp: number
  input?: string
}

export const useFavoritesStore = defineStore('favorites', () => {
  const FAVORITES_KEY = 'devfoundry-favorites'
  const HISTORY_KEY = 'devfoundry-history'
  const MAX_HISTORY_ITEMS = 50

  const favorites = ref<Set<string>>(
    new Set(JSON.parse(localStorage.getItem(FAVORITES_KEY) || '[]'))
  )

  const history = ref<ToolUsage[]>(
    JSON.parse(localStorage.getItem(HISTORY_KEY) || '[]')
  )

  // Computed properties
  const favoritesList = computed(() => Array.from(favorites.value))
  const recentTools = computed(() => {
    // Get unique recent tools (last 10)
    const seen = new Set<string>()
    return history.value
      .slice()
      .reverse()
      .filter(usage => {
        if (seen.has(usage.toolId)) return false
        seen.add(usage.toolId)
        return true
      })
      .slice(0, 10)
  })

  // Favorites methods
  const toggleFavorite = (toolId: string) => {
    if (favorites.value.has(toolId)) {
      favorites.value.delete(toolId)
    } else {
      favorites.value.add(toolId)
    }
    saveFavorites()
  }

  const isFavorite = (toolId: string) => {
    return favorites.value.has(toolId)
  }

  const saveFavorites = () => {
    localStorage.setItem(FAVORITES_KEY, JSON.stringify(favoritesList.value))
  }

  // History methods
  const addToHistory = (toolId: string, input?: string) => {
    const usage: ToolUsage = {
      toolId,
      timestamp: Date.now(),
      input: input?.substring(0, 100) // Store only first 100 chars
    }

    history.value.push(usage)

    // Keep only the last MAX_HISTORY_ITEMS
    if (history.value.length > MAX_HISTORY_ITEMS) {
      history.value = history.value.slice(-MAX_HISTORY_ITEMS)
    }

    saveHistory()
  }

  const clearHistory = () => {
    history.value = []
    saveHistory()
  }

  const saveHistory = () => {
    localStorage.setItem(HISTORY_KEY, JSON.stringify(history.value))
  }

  const getToolUsageCount = (toolId: string): number => {
    return history.value.filter(usage => usage.toolId === toolId).length
  }

  return {
    favorites,
    favoritesList,
    history,
    recentTools,
    toggleFavorite,
    isFavorite,
    addToHistory,
    clearHistory,
    getToolUsageCount
  }
})
