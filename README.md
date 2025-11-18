# DevFoundry

DevFoundry is an offline, cross-platform Swiss-army toolkit for developers.

- 🧰 Multiple dev tools in one place (JSON, Base64, UUID, hashes, etc.)
- 🔁 Shared core for CLI and GUI
- 📴 Offline-first, no random websites

## Tech Stack

**Core & Backend**

- .NET 8 (C#)
- DevFoundry.Core / DevFoundry.Tools.Basic / DevFoundry.Runtime
- ASP.NET Core minimal API (DevFoundry.Api)

**Frontend**

- Vue 3 + Vite
- TypeScript
- Pinia
- TailwindCSS

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js 20+ and npm

### CLI

```bash
cd backend

dotnet run --project src/DevFoundry.Cli/DevFoundry.Cli.csproj -- list
```

### API

```bash
cd backend

dotnet run --project src/DevFoundry.Api/DevFoundry.Api.csproj
```

The API will be available at `http://localhost:5000/api` by default.

### GUI

```bash
cd frontend/devfoundry-ui
npm install
npm run dev
```

The UI will run on `http://localhost:5173`, using the API at `VITE_API_BASE_URL`.

## Roadmap

* [ ] Core tool abstractions
* [ ] Basic tools: JSON formatter, JSON/YAML, Base64, UUID, hashes
* [ ] CLI with `list`, `describe`, `run`
* [ ] HTTP API and Vue UI
* [ ] Plugin discovery for external tools
* [ ] Desktop packaging

DevFoundry is primarily a personal productivity project, but the architecture is designed for long-term extensibility.
