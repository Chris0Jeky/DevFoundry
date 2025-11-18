# DevFoundry UI

This is the frontend web interface for DevFoundry, built with Vue 3, TypeScript, and Vite.

## Tech Stack

- **Vue 3** - Progressive JavaScript framework
- **TypeScript** - Type-safe JavaScript
- **Vite** - Fast build tool and dev server
- **Pinia** - State management
- **Vue Router** - Client-side routing
- **TailwindCSS** - Utility-first CSS framework
- **Axios** - HTTP client

## Project Structure

```
src/
  api/          - HTTP client and API services
  assets/       - CSS and static assets
  components/   - Vue components
    layout/     - Layout components (AppShell, ToolSidebar)
    tools/      - Tool panels
  router/       - Vue Router configuration
  stores/       - Pinia stores
  types/        - TypeScript type definitions
  views/        - View components
```

## Development

### Install Dependencies

```bash
npm install
```

### Run Dev Server

```bash
npm run dev
```

The UI will be available at `http://localhost:5173`.

### Build for Production

```bash
npm run build
```

### Type Check

```bash
npm run type-check
```

## Environment Variables

Create a `.env` file:

```
VITE_API_BASE_URL=http://localhost:5000
```

## Adding a New Tool Panel

1. Create a new component in `src/components/tools/`
2. Add it to the `toolComponents` mapping in `ToolPanelContainer.vue`
3. Use the shared styles from `tool-panel-styles.css`
