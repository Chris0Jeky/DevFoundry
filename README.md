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

## Quick Start

See [QUICKSTART.md](QUICKSTART.md) for detailed setup instructions.

**TL;DR:**

```bash
# Terminal 1 - Start the API
cd backend
dotnet run --project src/DevFoundry.Api/DevFoundry.Api.csproj

# Terminal 2 - Start the UI
cd frontend/devfoundry-ui
npm install
npm run dev
```

Then open `http://localhost:5173` in your browser.

## Available Tools

- **JSON Formatter** - Pretty-print or minify JSON
- **JSON ⇄ YAML Converter** - Convert between formats
- **Base64 Encoder/Decoder** - Encode/decode text
- **UUID Generator** - Generate v4 UUIDs
- **Hash Calculator** - MD5, SHA-1, SHA-256, SHA-512

## Roadmap

### Completed (v0.1.0)
* [x] Core tool abstractions
* [x] Basic tools: JSON formatter, JSON/YAML, Base64, UUID, hashes
* [x] CLI with `list`, `describe`, `run`
* [x] HTTP API and Vue UI

### Future
* [ ] Plugin discovery for external tools
* [ ] Desktop packaging (Tauri/Electron)
* [ ] Additional tools (JWT decoder, timestamp converter, etc.)
* [ ] Dark mode
* [ ] Tool favorites and history

## Documentation

- [Quick Start Guide](QUICKSTART.md) - Get up and running quickly
- [Contributing Guide](CONTRIBUTING.md) - How to contribute
- [Backend README](backend/README.md) - Backend architecture and API
- [Frontend README](frontend/devfoundry-ui/README.md) - Frontend development
- [Changelog](CHANGELOG.md) - Version history

## License

MIT License - see [LICENSE](LICENSE) file for details.

DevFoundry is a personal productivity project designed for long-term extensibility.
