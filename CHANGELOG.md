# Changelog

All notable changes to DevFoundry will be documented in this file.

## [0.1.0] - 2024-01-01

### Added

#### Backend
- Core abstractions: ITool interface, ToolDescriptor, ToolInput, ToolResult
- Tool registry and runtime with dependency injection support
- 5 MVP tools:
  - JSON Formatter (pretty-print and minify)
  - JSON ⇄ YAML Converter (bidirectional)
  - Base64 Encoder/Decoder
  - UUID Generator (v4, configurable count)
  - Hash Calculator (MD5, SHA-1, SHA-256, SHA-512)
- CLI with list, describe, and run commands
- ASP.NET Core minimal API with CORS support
- Unit tests for all core components and tools

#### Frontend
- Vue 3 + TypeScript + Vite setup
- TailwindCSS for styling
- Pinia state management
- Vue Router for navigation
- HTTP client with Axios
- AppShell layout with header and sidebar
- Tool sidebar with search and category grouping
- Individual panels for all 5 tools
- Copy-to-clipboard functionality
- Error handling and loading states

#### Documentation
- Main README with getting started guide
- Backend-specific README
- Frontend-specific README
- CONTRIBUTING.md with development guidelines
- MIT LICENSE
- .editorconfig for consistent code style
- Comprehensive .gitignore

### Technical Details
- .NET 8 backend with C# 12
- Vue 3.4+ with Composition API
- TypeScript 5.4+ with strict mode
- System.CommandLine for CLI
- YamlDotNet for YAML support
- xUnit for testing
