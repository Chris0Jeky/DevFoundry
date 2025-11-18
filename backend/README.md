# DevFoundry Backend

This folder contains the .NET 8 backend for DevFoundry, including the core abstractions, tool implementations, CLI, and API.

## Projects

### DevFoundry.Core
Core abstractions and interfaces:
- `ITool` - Interface that all tools must implement
- `ToolDescriptor` - Metadata about tools
- `ToolInput` / `ToolResult` - Input/output models

### DevFoundry.Tools.Basic
Built-in tool implementations:
- JSON Formatter
- JSON ⇄ YAML Converter
- Base64 Encoder/Decoder
- UUID Generator
- Hash Calculator (MD5, SHA-1, SHA-256, SHA-512)

### DevFoundry.Runtime
Runtime services:
- `IToolRegistry` - Tool discovery and retrieval
- `ToolRegistry` - Implementation with DI support

### DevFoundry.Cli
Command-line interface using System.CommandLine:
- `list` - List all available tools
- `describe <toolId>` - Show tool details
- `run <toolId>` - Execute a tool

### DevFoundry.Api
ASP.NET Core minimal API:
- `GET /api/tools` - List all tools
- `POST /api/tools/{toolId}/run` - Execute a tool

## Building

```bash
dotnet build
```

## Running Tests

```bash
dotnet test
```

## Running the CLI

```bash
cd src/DevFoundry.Cli
dotnet run -- list
dotnet run -- describe json.formatter
echo '{"test":true}' | dotnet run -- run json.formatter
```

## Running the API

```bash
cd src/DevFoundry.Api
dotnet run
```

The API will be available at `http://localhost:5000`.

## 🎬 Try the Interactive Demo

DevFoundry includes comprehensive demos showcasing all tools with visual output and pipeline workflows!

### Quick Start

**Command-Line Demo** (Recommended first):
```powershell
# Windows
cd demo
.\run-demo.ps1 -Fast

# Linux/macOS
cd demo
./run-demo.sh --fast
```

**Web-Based Demo**:
1. Start the API: `cd src/DevFoundry.Api && dotnet run`
2. Open `demo/web-demo.html` in your browser

See **[demo/README.md](demo/README.md)** for complete demo documentation.

### What the Demo Shows
- All 5 tools in action with sample data
- Before/after output comparisons
- Real-world pipeline workflow (JSON → Format → UUID → Hash → Base64)
- Beautiful colored terminal output or interactive web UI
- All outputs saved to `demo/output/` directory
