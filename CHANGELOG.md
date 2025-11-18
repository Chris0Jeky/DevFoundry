# Changelog

All notable changes to DevFoundry will be documented in this file.

## [0.2.0] - 2024-02-15

### Added

#### Backend
- **Timestamp Converter Tool** - Convert between Unix timestamp and human-readable date/time
  - Supports both seconds and milliseconds
  - Bidirectional conversion (Unix → Human, Human → Unix)
  - Multiple output formats (UTC, Local, ISO 8601)
- **String Case Converter Tool** - Convert between different case formats
  - Supports camelCase, PascalCase, snake_case, kebab-case, UPPERCASE, lowercase, Title Case
  - Intelligent word boundary detection
- **URL Encoder/Decoder Tool** - URL-safe encoding and decoding
  - Percent encoding support with RFC 3986 semantics (spaces encoded as `%20`)
  - Uses `Uri.EscapeDataString` / `Uri.UnescapeDataString`
- **JWT Decoder Tool** - Decode and inspect JWT tokens
  - Displays header, payload, and signature
  - Pretty-printed JSON output
  - Security warning about signature verification
- Comprehensive test suite for JsonYamlConverterTool
- Added tests for timestamp, string case, URL encoder, and JWT decoder tools

#### Frontend
- Individual panels for all new tools
- Enhanced tool categorization in sidebar
- Improved UX with better placeholders and descriptions

### Technical Details
- All new tools follow the ITool interface pattern
- Enhanced tool categorization (Time, Text categories)
- Consistent error handling across all tools

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
