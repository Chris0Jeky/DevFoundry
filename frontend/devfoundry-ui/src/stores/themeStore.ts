import { defineStore } from 'pinia'
import { ref, watch } from 'vue'

export type Theme = 'light' | 'dark' | 'auto'

export const useThemeStore = defineStore('theme', () => {
  const STORAGE_KEY = 'devfoundry-theme'

  const theme = ref<Theme>((localStorage.getItem(STORAGE_KEY) as Theme) || 'auto')
  const isDark = ref(false)

  // Update isDark based on theme preference and system preference
  const updateIsDark = () => {
    if (theme.value === 'dark') {
      isDark.value = true
    } else if (theme.value === 'light') {
      isDark.value = false
    } else {
      // auto: use system preference
      isDark.value = window.matchMedia('(prefers-color-scheme: dark)').matches
    }

    // Update document class
    if (isDark.value) {
      document.documentElement.classList.add('dark')
    } else {
      document.documentElement.classList.remove('dark')
    }
  }

  // Watch for theme changes
  watch(theme, (newTheme) => {
    localStorage.setItem(STORAGE_KEY, newTheme)
    updateIsDark()
  })

  // Listen for system theme changes when in auto mode
  const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)')
  mediaQuery.addEventListener('change', () => {
    if (theme.value === 'auto') {
      updateIsDark()
    }
  })

  // Initialize
  updateIsDark()

  const setTheme = (newTheme: Theme) => {
    theme.value = newTheme
  }

  const toggleTheme = () => {
    if (theme.value === 'light') {
      theme.value = 'dark'
    } else if (theme.value === 'dark') {
      theme.value = 'auto'
    } else {
      theme.value = 'light'
    }
  }

  return {
    theme,
    isDark,
    setTheme,
    toggleTheme
  }
})
