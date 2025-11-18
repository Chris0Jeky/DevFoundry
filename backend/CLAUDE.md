# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

DevFoundry is an offline developer toolkit built with .NET 8. It provides a plugin-based tool system accessible via both CLI and API. The backend is structured as a modular solution with clear separation between core abstractions, tool implementations, runtime services, and execution environments.

## Architecture

### Core Plugin System

The architecture follows a plugin pattern centered on the `ITool` interface:

1. **DevFoundry.Core** - Defines the contract for all tools:
   - `ITool` interface with `Descriptor` property and `Execute()` method
   - `ToolDescriptor` contains metadata (ID, display name, description, category, tags, parameters)
   - `ToolInput` / `ToolResult` for tool execution I/O
   - `ToolParameterDescriptor` for declaring tool parameters

2. **DevFoundry.Runtime** - Discovery and execution:
   - `IToolRegistry` interface for tool discovery and retrieval
   - `ToolRegistry` implementation using a dictionary keyed by tool ID (case-insensitive)
   - `ServiceCollectionExtensions` provides DI registration helpers:
     - `AddDevFoundryRuntime()` - registers `IToolRegistry`
     - `AddTool<TTool>()` - registers individual tools as `ITool` singletons

3. **Tool Registration Pattern** - Both CLI and API use the same registration approach:
   ```csharp
   services.AddTool<JsonFormatterTool>();
   services.AddTool<JsonYamlConverterTool>();
   services.AddTool<Base64Tool>();
   services.AddTool<UuidTool>();
   services.AddTool<HashTool>();
   services.AddDevFoundryRuntime();
   ```

   The runtime collects all registered `ITool` instances via DI and builds the registry.

### Execution Environments

**CLI (DevFoundry.Cli)**:
- Uses System.CommandLine for command parsing
- Commands: `list`, `describe <toolId>`, `run <toolId>`
- Piping support: `echo '{"test":true}' | dotnet run -- run json.formatter`
- Located in `src/DevFoundry.Cli/Commands/` directory

**API (DevFoundry.Api)**:
- ASP.NET Core minimal API
- Endpoints:
  - `GET /api/tools` - List all available tools
  - `POST /api/tools/{toolId}/run` - Execute a tool
- CORS configured for `http://localhost:5173` and `http://localhost:5174`
- DTOs in `DevFoundry.Api.DTOs` namespace

## Development Commands

### Building
```bash
dotnet build
```

### Testing
```bash
# Run all tests
dotnet test

# Run tests for a specific project
dotnet test tests/DevFoundry.Core.Tests
dotnet test tests/DevFoundry.Tools.Basic.Tests
```

### Running the CLI
```bash
cd src/DevFoundry.Cli
dotnet run -- list
dotnet run -- describe json.formatter
echo '{"test":true}' | dotnet run -- run json.formatter
```

### Running the API
```bash
cd src/DevFoundry.Api
dotnet run
```
API will be available at `http://localhost:5000`

### Cleanup Orphaned Processes
If build fails with "file is locked" errors, orphaned DevFoundry processes may be running:

```bash
# Windows (PowerShell)
.\scripts\cleanup-processes.ps1

# Linux/macOS
./scripts/cleanup-processes.sh

# Or manually:
# Windows: taskkill /F /IM DevFoundry.Api.exe
# Linux/macOS: pkill -f DevFoundry
```

See DEVELOPMENT.md (Troubleshooting #6) for details.

## Creating New Tools

1. Implement `ITool` interface in a new class
2. Define `ToolDescriptor` with metadata and parameters
3. Implement `Execute(ToolInput input)` method
4. Register the tool in both CLI and API `Program.cs`:
   ```csharp
   services.AddTool<YourNewTool>();
   ```

Tool IDs follow the pattern: `category.toolname` (e.g., `json.formatter`, `hash.calculator`)

## Project Dependencies

- **DevFoundry.Tools.Basic** depends on **DevFoundry.Core**
- **DevFoundry.Runtime** depends on **DevFoundry.Core**
- **DevFoundry.Cli** depends on **DevFoundry.Core**, **DevFoundry.Runtime**, **DevFoundry.Tools.Basic**
- **DevFoundry.Api** depends on **DevFoundry.Core**, **DevFoundry.Runtime**, **DevFoundry.Tools.Basic**

## Technology Stack

- .NET 8
- System.CommandLine (CLI)
- ASP.NET Core Minimal API (API)
- Microsoft.Extensions.DependencyInjection (DI)
- xUnit (Testing)
- YamlDotNet (JSON/YAML conversion)

## Known Issues

### 🔴 CRITICAL: JsonYamlConverterTool Bug
**Status**: OPEN (as of 2025-11-18)
**Tool**: `json.yaml`
**Issue**: JSON-to-YAML conversion produces incorrect output ("valueKind: Object" instead of proper YAML)
**Location**: `src/DevFoundry.Tools.Basic/JsonYamlConverterTool.cs:76`
**Root Cause**: `JsonSerializer.Deserialize<object>(json)` returns `JsonElement` instead of deserialized object
**Impact**: HIGH - JSON-to-YAML feature is non-functional
**YAML-to-JSON**: Works correctly ✅

**Recommended Fix**:
```csharp
// Instead of:
var jsonObject = JsonSerializer.Deserialize<object>(json);

// Use:
using var doc = JsonDocument.Parse(json);
var jsonElement = doc.RootElement;
// Pass JsonElement directly to YamlDotNet with proper configuration
```

**Before fixing**: Add comprehensive tests for JsonYamlConverterTool (currently has ZERO tests)

See TEST_REPORT.md for complete details.

## Test Coverage Status

**Last Tested**: 2025-11-18
**Overall Coverage**: ~45% (Target: 80%)

| Project | Coverage | Tests | Status |
|---------|----------|-------|--------|
| DevFoundry.Core | ~50% | 2 | ⚠️ Needs more |
| DevFoundry.Runtime | 0% | 0 | ❌ No tests |
| DevFoundry.Tools.Basic | ~85% | 14 | ⚠️ JsonYaml missing |
| DevFoundry.Cli | 0% | 0 | ❌ No tests |
| DevFoundry.Api | 0% | 0 | ❌ No tests |

**Missing Tests**:
- JsonYamlConverterTool (0 tests) - **HIGH PRIORITY**
- DevFoundry.Runtime (0 tests) - **HIGH PRIORITY**
- DevFoundry.Api (0 tests) - **MEDIUM PRIORITY**
- DevFoundry.Cli (0 tests) - **MEDIUM PRIORITY**

## Important Files

- **CLAUDE.md** (this file) - Working with this codebase
- **MASTER_PLAN.md** - Implementation roadmap and task list
- **DEVELOPMENT.md** - Development practices and guides
- **TEST_REPORT.md** - Latest test results and findings
- **REVIEW_SUMMARY.md** - Code review summary
- **README.md** - Project overview
