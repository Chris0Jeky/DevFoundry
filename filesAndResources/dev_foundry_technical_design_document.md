# DevFoundry – Technical & Product Design Document

## 0. Overview

**Name:** DevFoundry  
**Tagline:** Offline, cross‑platform Swiss‑army toolkit for developers – CLI + GUI on a shared core.

DevFoundry is a modular toolkit that bundles common developer utilities into a single, cohesive application. It aims to:

- Replace random “online tools” with trusted, offline alternatives.
- Provide both **CLI** and **GUI** interfaces powered by the **same core library**.
- Make it **trivial to add new tools** via a plugin‑like architecture.
- Be cross‑platform (Windows, macOS, Linux).

Example tools:

- JSON formatter/minifier.
- JSON ⇄ YAML converter.
- Base64 encode/decode.
- UUID generator.
- Hash calculator (MD5/SHA).
- Timestamp ↔ datetime converter.
- Text diff.

This document describes the **product vision**, **requirements**, **architecture**, **plugin model**, **stack**, **CLI**, **GUI**, and **roadmap**.

---

## 1. Vision & Goals

### 1.1 Problem

Developers regularly need small, focused utilities:

- Format or convert data (JSON, YAML, Base64, timestamps).
- Inspect tokens (e.g., JWT), hashes, or HTTP headers.
- Generate IDs, random data, color codes, etc.

Today, this often means:

- Searching for random websites of unknown trust.
- Installing N separate small tools.
- Writing throwaway scripts.

### 1.2 High‑Level Goals

1. **Local, offline‑first tools**  
   - No external calls required for common operations.
2. **Shared core for CLI + GUI**  
   - Same logic used by command‑line invocations and desktop/web UI.
3. **Modular architecture**  
   - Easy to add a new tool with minimal boilerplate.
4. **Cross‑platform**  
   - Windows, macOS, Linux.
5. **Developer experience focus**  
   - Friendly CLI (good help, composable with pipes).
   - Clean, fast GUI for discovery and quick usage.

### 1.3 Non‑Goals (MVP)

- Multi‑user features, sharing tool configurations between users.
- Cloud accounts, sync, or telemetry.
- Heavy project/workspace management.

---

## 2. Feature Overview

### 2.1 MVP Tool Inventory

Focus on a small but high‑value set of tools:

1. **JSON Formatter**
   - Pretty‑print/minify JSON.
   - Validate JSON syntax and show errors.

2. **JSON ⇄ YAML Converter**
   - Convert JSON → YAML and YAML → JSON.

3. **Base64 Encoder/Decoder**
   - Encode/decode text.
   - Optionally handle files.

4. **UUID Generator**
   - Generate v4 UUIDs.
   - Option: multiple at once.

5. **Hash Calculator**
   - Compute MD5/SHA‑1/SHA‑256 for text input.

These five tools give a solid base and exercise the architecture without complexity.

### 2.2 Near‑Term Tools

- Timestamp converter (Unix ↔ human date/time).
- String case converter (snake/camel/kebab/pascal).
- URL encoder/decoder.
- JWT decoder (header/payload pretty‑print).
- Color utilities (hex ↔ RGB ↔ HSL).

### 2.3 Long‑Term Tool Categories

- Text & encoding tools.
- Data format tools (JSON, XML, YAML, CSV, etc.).
- DevOps/infra tools (port checker, CIDR calculators).
- HTTP tools (small REST client, header parser).
- Git‑related tools (later: integrating Git Analyzer modules).

---

## 3. System Architecture Overview

### 3.1 Architectural Style

DevFoundry is built around a **tool core** with pluggable tool implementations, and multiple frontends that consume that core.

Layers / components:

- **DevFoundry.Core** – core abstractions: tool interface, descriptors, common utilities.
- **DevFoundry.Tools.Basic** – built‑in tools (JSON, Base64, UUID, Hash, etc.).
- **DevFoundry.Runtime** – tool registry, configuration loading, discovery.
- **DevFoundry.Cli** – CLI frontend.
- **DevFoundry.Api** (optional, for GUI) – HTTP API for listing and running tools.
- **DevFoundry.Ui (Vue)** – SPA using the API.

### 3.2 High‑Level Component Diagram

Conceptually:

- `devfoundry` CLI → uses `DevFoundry.Runtime` → executes tools via `DevFoundry.Core`.
- `devfoundry-ui` (Vue SPA) → HTTP → `DevFoundry.Api` → uses `DevFoundry.Runtime/Core`.

All business logic is in Core + Tools assemblies. CLI and API are thin wrappers.

### 3.3 Cross‑Cutting Concerns

- Logging: structured logs from tool executions.
- Configuration: tool discovery, default settings.
- Error handling: structured error responses.
- Security: even though it’s local, be mindful when reading/writing files.

---

## 4. Tech Stack Decisions & Rationale

### 4.1 Core & Runtime

- **Language:** C#
- **Runtime:** .NET 8
- **Project Type:** Class libraries

**Why:**

- Great for cross‑platform CLIs and APIs.
- Strong support for abstractions, reflection, plugin‑like patterns.
- Easy integration with both a CLI and ASP.NET Core API.

### 4.2 CLI

- **Project:** .NET console app
- **Libraries:**
  - `System.CommandLine` (or `Spectre.Console.Cli`) for parsing arguments and building a good UX.
  - `Spectre.Console` for nice console output (tables, colors) in the future.

### 4.3 API

- **Framework:** ASP.NET Core minimal APIs
- **Use:** Provide HTTP interface for GUI and possibly external tools.

### 4.4 GUI (Web/Desktop)

- **Frontend:** Vue 3 + Vite + TypeScript
- **UI Library:** TailwindCSS (simple, flexible)
- **State Management:** Pinia

**Desktop Wrapping Options (Future):**

1. **Tauri** – wrap built SPA + local API into a lightweight desktop app.  
2. **Electron** – heavier, more mature ecosystem.

For now, focus is on the browser‑based GUI; wrapping into desktop is a packaging step.

---

## 5. Repository & Folder Structure

Top‑level layout:

```text
devfoundry/
  README.md
  .gitignore
  .editorconfig

  backend/
    DevFoundry.sln
    src/
      DevFoundry.Core/
      DevFoundry.Tools.Basic/
      DevFoundry.Runtime/
      DevFoundry.Cli/
      DevFoundry.Api/
    tests/
      DevFoundry.Core.Tests/
      DevFoundry.Tools.Basic.Tests/

  frontend/
    devfoundry-ui/
      (Vite + Vue + TS project)
```

### 5.1 Backend Projects

**DevFoundry.Core**

- Tool abstractions.
- Shared types (input/output, descriptors, metadata).
- Common utils (e.g. JSON helpers, encoding helpers).

**DevFoundry.Tools.Basic**

- Concrete implementations of basic tools:
  - JsonFormatterTool
  - JsonYamlConverterTool
  - Base64Tool
  - UuidTool
  - HashTool

**DevFoundry.Runtime**

- Tool registry.
- Plugin discovery (from referenced assemblies, and later from external assemblies).
- Configuration loading (e.g., config file listing enabled tools).

**DevFoundry.Cli**

- Entry point executable.
- Command definitions mapping CLI arguments → tool executions.

**DevFoundry.Api**

- ASP.NET Core minimal API.
- Endpoints to list tools and run them.
- Host configuration and DI wiring.

### 5.2 Frontend Project Structure

Inside `frontend/devfoundry-ui`:

```text
src/
  main.ts
  App.vue

  router/
    index.ts

  store/
    toolsStore.ts
    uiStore.ts

  api/
    http.ts
    toolsApi.ts

  components/
    layout/
      AppShell.vue
      ToolSidebar.vue

    tools/
      ToolPanelContainer.vue
      JsonFormatterPanel.vue
      JsonYamlConverterPanel.vue
      Base64Panel.vue
      UuidPanel.vue
      HashPanel.vue

  views/
    HomeView.vue

  types/
    tool.ts
    jsonFormatter.ts

  assets/
    (icons, logo)
```

---

## 6. Core Abstractions

### 6.1 Tool Identity & Metadata

Each tool is identified by a unique string ID and has metadata for GUI/CLI discovery.

```csharp
namespace DevFoundry.Core;

public enum ToolCategory
{
    DataFormat,
    Encoding,
    Generation,
    Crypto,
    Time,
    Other
}

public sealed class ToolDescriptor
{
    public string Id { get; init; } = default!;           // "json.formatter"
    public string DisplayName { get; init; } = default!;  // "JSON Formatter"
    public string Description { get; init; } = default!;
    public ToolCategory Category { get; init; }
    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();

    // Optionally: define structured input/output schema for dynamic UIs.
    public IReadOnlyList<ToolParameterDescriptor> Parameters { get; init; } = Array.Empty<ToolParameterDescriptor>();
}

public sealed class ToolParameterDescriptor
{
    public string Name { get; init; } = default!;         // "indentSize"
    public string DisplayName { get; init; } = default!;  // "Indentation Size"
    public string Description { get; init; } = default!;
    public string Type { get; init; } = default!;         // "int", "string", "bool"
    public object? DefaultValue { get; init; }
}
```

### 6.2 Tool Input/Output

```csharp
public sealed class ToolInput
{
    public string? Text { get; init; }
    public string? SecondaryText { get; init; }  // e.g., for diff tool
    public IDictionary<string, object?> Parameters { get; init; } = new Dictionary<string, object?>();
}

public sealed class ToolResult
{
    public bool Success { get; init; }
    public string? OutputText { get; init; }
    public string? SecondaryOutputText { get; init; }
    public string? ErrorMessage { get; init; }
    public IDictionary<string, object?> Metadata { get; init; } = new Dictionary<string, object?>();
}
```

### 6.3 Tool Interface

```csharp
public interface ITool
{
    ToolDescriptor Descriptor { get; }
    ToolResult Execute(ToolInput input);
}
```

This interface is intentionally simple. Tools are pure functions: `ToolInput` → `ToolResult`.

---

## 7. Tool Implementations (Examples)

### 7.1 JSON Formatter Tool

**ID:** `json.formatter`  
**Category:** `DataFormat`  
**Inputs:**

- `Text` – JSON string.
- Parameter `indentSize` (int, default 2).
- Parameter `minify` (bool, default false).

**Output:**

- `OutputText` – formatted or minified JSON.

Pseudo‑implementation:

```csharp
public sealed class JsonFormatterTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "json.formatter",
        DisplayName = "JSON Formatter",
        Description = "Pretty-print or minify JSON.",
        Category = ToolCategory.DataFormat,
        Parameters = new []
        {
            new ToolParameterDescriptor
            {
                Name = "indentSize",
                DisplayName = "Indent Size",
                Description = "Number of spaces for indentation.",
                Type = "int",
                DefaultValue = 2
            },
            new ToolParameterDescriptor
            {
                Name = "minify",
                DisplayName = "Minify",
                Description = "Remove whitespace instead of formatting.",
                Type = "bool",
                DefaultValue = false
            }
        }
    };

    public ToolResult Execute(ToolInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Text))
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = "No input text provided."
            };
        }

        try
        {
            using var doc = JsonDocument.Parse(input.Text);

            var minify = input.Parameters.TryGetValue("minify", out var m) && m is bool b && b;
            var indentSize = input.Parameters.TryGetValue("indentSize", out var i) && i is int n ? n : 2;

            var options = new JsonWriterOptions
            {
                Indented = !minify,
                // for more control, custom indentation logic can be added
            };

            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, options))
            {
                doc.WriteTo(writer);
            }

            var resultText = Encoding.UTF8.GetString(stream.ToArray());

            return new ToolResult
            {
                Success = true,
                OutputText = resultText
            };
        }
        catch (JsonException ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"JSON parse error: {ex.Message}"
            };
        }
    }
}
```

### 7.2 Base64 Tool

**ID:** `encoding.base64`  
**Inputs:**

- `Text` – text to encode/decode.
- Parameter `mode`: `"encode"` or `"decode"`.

**Output:**

- `OutputText` – Base64 string or decoded string.

Implementation is straightforward using `Convert.ToBase64String` and `Convert.FromBase64String`.

### 7.3 UUID Tool

**ID:** `generation.uuid`  
**Inputs:**

- Parameter `count` (int, default 1).

**Output:**

- `OutputText` – newline‑separated list of UUIDs.

---

## 8. Runtime & Tool Registry

### 8.1 Tool Registration

DevFoundry.Runtime is responsible for discovering and registering tools.

```csharp
public interface IToolRegistry
{
    IEnumerable<ITool> GetAllTools();
    ITool? GetTool(string id);
}

public sealed class ToolRegistry : IToolRegistry
{
    private readonly Dictionary<string, ITool> _tools;

    public ToolRegistry(IEnumerable<ITool> tools)
    {
        _tools = tools.ToDictionary(t => t.Descriptor.Id, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<ITool> GetAllTools() => _tools.Values;

    public ITool? GetTool(string id)
        => _tools.TryGetValue(id, out var tool) ? tool : null;
}
```

In the **MVP**, tools are registered via DI in the API/CLI startup:

```csharp
services.AddSingleton<ITool, JsonFormatterTool>();
services.AddSingleton<ITool, JsonYamlConverterTool>();
services.AddSingleton<ITool, Base64Tool>();
services.AddSingleton<ITool, UuidTool>();
services.AddSingleton<ITool, HashTool>();

services.AddSingleton<IToolRegistry, ToolRegistry>();
```

Later, a plugin discovery mechanism can load tools from external assemblies.

### 8.2 Plugin Discovery (Future)

Possible approaches:

- Specify a `plugins` folder; load all DLLs that reference `DevFoundry.Core`.
- Use reflection to find types implementing `ITool` and register them.
- Optional config file to enable/disable tools by ID.

Simple config example (`devfoundry.config.json`):

```json
{
  "enabledTools": [
    "json.formatter",
    "json.yaml",
    "encoding.base64",
    "generation.uuid",
    "crypto.hash"
  ]
}
```

---

## 9. CLI Design

### 9.1 Goals

- Easy to discover tools and their parameters.
- Easy to pipe stdin into tools and pipe output onward.
- Fallback to interactive prompts when arguments are missing.

### 9.2 Command Structure

Top‑level command: `devfoundry`

Subcommands:

- `devfoundry list` – list available tools.
- `devfoundry describe <toolId>` – show tool metadata and parameters.
- `devfoundry run <toolId>` – run a tool.
- (Optional) Shortcuts like `devfoundry json.formatter` directly.

Examples:

```bash
# List all tools
devfoundry list

# Describe a tool in detail
devfoundry describe json.formatter

# Run JSON Formatter from a file and print result
devfoundry run json.formatter --file input.json --indent 4

# Use stdin/stdout
cat input.json | devfoundry run json.formatter > output.json

# Base64 encode a string directly
devfoundry run encoding.base64 --mode encode --text "hello world"
```

### 9.3 CLI Implementation Sketch

Using `System.CommandLine`, map commands to tool execution.

- Parse toolId and parameters.
- Build `ToolInput`:
  - `Text` from `--text`, `--file`, or stdin.
  - Parameters from `--paramName value` pairs.
- Call `IToolRegistry.GetTool(toolId).Execute(input)`.
- Output `ToolResult.OutputText` to stdout or file.

Return non‑zero exit codes on failure:

- `1` – generic error.
- `2` – tool not found.
- `3` – invalid input parameters.

---

## 10. HTTP API Design

### 10.1 Use Cases

- List tools for GUI (categories, descriptions).
- Run a tool with given input and parameters.

### 10.2 Endpoints

Base path: `/api`.

**List Tools**

`GET /api/tools`

Response:

```json
[
  {
    "id": "json.formatter",
    "displayName": "JSON Formatter",
    "description": "Pretty-print or minify JSON.",
    "category": "DataFormat",
    "tags": ["json", "format"],
    "parameters": [
      {
        "name": "indentSize",
        "displayName": "Indent Size",
        "description": "Spaces for indentation.",
        "type": "int",
        "defaultValue": 2
      },
      {
        "name": "minify",
        "displayName": "Minify",
        "description": "Remove whitespace.",
        "type": "bool",
        "defaultValue": false
      }
    ]
  }
]
```

**Run Tool**

`POST /api/tools/{toolId}/run`

Request:

```json
{
  "text": "{\"name\":\"Alice\"}",
  "secondaryText": null,
  "parameters": {
    "indentSize": 4,
    "minify": false
  }
}
```

Response:

```json
{
  "success": true,
  "outputText": "{\n    \"name\": \"Alice\"\n}",
  "secondaryOutputText": null,
  "errorMessage": null,
  "metadata": {}
}
```

Errors return `HTTP 400` or `500` with body:

```json
{
  "success": false,
  "errorMessage": "JSON parse error: ..."
}
```

---

## 11. GUI / Web UI Design

### 11.1 UX Goals

- Quick discovery:
  - Tools categorized and searchable.
- Single‑page interaction:
  - Choose a tool in a sidebar.
  - See its form and results in the main panel.
- Snappy feedback:
  - No page reloads; results show instantly.

### 11.2 Screens & Layout

**AppShell.vue**

- Top bar: logo, app name, optional theme toggle.
- Left sidebar: list of tools grouped by category.
- Right main area: selected tool’s panel.

**HomeView.vue**

- Shows a welcome message and a grid of most popular tools.

**ToolSidebar.vue**

- Groups tools by category.
- Allows searching by name/tag.

**ToolPanelContainer.vue**

- Given a selected tool (from store), renders the correct panel component.
- Fallback to a generic dynamic form in the future.

**Specific Panels (MVP)**

- `JsonFormatterPanel.vue`
- `JsonYamlConverterPanel.vue`
- `Base64Panel.vue`
- `UuidPanel.vue`
- `HashPanel.vue`

### 11.3 State Management (Pinia)

**toolsStore**

State:

- `tools: ToolDescriptorDto[]`
- `selectedToolId: string | null`

Actions:

- `fetchTools()` – load from `/api/tools`.
- `selectTool(id: string)`.
- `runTool(payload)` – call `/api/tools/{id}/run`.

**uiStore**

State:

- `isLoading: boolean`
- `errorMessage: string | null`
- `searchQuery: string`

Actions:

- `setLoading`, `setError`, `setSearchQuery`.

### 11.4 API Client Module

`api/http.ts`:

- Axios instance with `VITE_API_BASE_URL`.

`api/toolsApi.ts`:

- `getTools()` → `ToolDescriptorDto[]`.
- `runTool(toolId, request)` → `ToolRunResultDto`.

### 11.5 Example Panel Flow – JSON Formatter

1. User selects "JSON Formatter" in sidebar.
2. `JsonFormatterPanel` shows:
   - Textarea for input JSON.
   - Number input or select for indent size.
   - Minify toggle.
   - Format button.
3. On click, `toolsStore.runTool` is called with:

```ts
{
  text: inputText,
  secondaryText: null,
  parameters: {
    indentSize: indentSize,
    minify: minify
  }
}
```

4. Result’s `outputText` displayed in an output textarea with copy button.

---

## 12. Testing Strategy

### 12.1 Core & Tools Tests

**DevFoundry.Core.Tests**

- Test `ToolResult` behaviors and any shared helpers.

**DevFoundry.Tools.Basic.Tests**

- JSON Formatter:
  - Formats valid JSON with default and custom indentation.
  - Minify mode.
  - Fails gracefully on invalid JSON.

- Base64 Tool:
  - Encode/decode simple strings.
  - Reject invalid Base64 input.

- UUID Tool:
  - Generates valid GUIDs, correct count.

### 12.2 CLI Tests

- Where feasible, test command handlers separately from `Main`.
- Use snapshot tests for output (e.g., verifying `devfoundry list` formatting).

### 12.3 API & GUI Tests (Later)

- Integration tests for API endpoints (minimal API).
- Frontend unit tests for panels (Vitest).
- E2E tests for basic flows (Cypress/Playwright).

---

## 13. Deployment & Execution

### 13.1 Local Development

Backend:

- From `backend/DevFoundry.Api`:

```bash
dotnet run --project src/DevFoundry.Api/DevFoundry.Api.csproj
```

Frontend:

- From `frontend/devfoundry-ui`:

```bash
npm install
npm run dev
```

CLI:

- From `backend/DevFoundry.Cli`:

```bash
dotnet run --project src/DevFoundry.Cli/DevFoundry.Cli.csproj -- list
```

### 13.2 Distribution (Future)

- Publish CLI as a .NET global tool (`dotnet tool install -g devfoundry`).
- Publish API + UI as a self‑contained app for local use.
- Package UI with Tauri/Electron for full desktop experience.

---

## 14. Roadmap & Milestones

### Phase 1 – Core & Basic Tools

- Implement `DevFoundry.Core` abstractions.
- Implement basic tools in `DevFoundry.Tools.Basic`.
- Implement `DevFoundry.Runtime` with manually registered tools.
- Implement `DevFoundry.Cli` with `list`, `describe`, and `run`.

### Phase 2 – API & GUI

- Implement ASP.NET Core minimal API for tool listing and execution.
- Scaffold Vue 3 + Vite UI.
- Implement basic ToolSidebar + JSON Formatter panel.
- Add other tool panels (JSON/YAML, Base64, UUID, Hash).

### Phase 3 – UX & Plugins

- Improve CLI UX (help, examples, colors).
- Add search & categories in GUI.
- Implement plugin discovery from external assemblies.
- Add more tools based on your needs.

### Phase 4 – Desktop & Integration

- Wrap UI into a desktop app via Tauri/Electron.
- Integrate with your other tools (e.g., Git Analyzer as a `git.stats` tool).

---

## 15. Coding Guidelines

### 15.1 C#

- Use `PascalCase` for classes, `camelCase` for local variables and fields.
- Keep tools small and focused.
- Avoid mixing I/O and logic in tool implementations; prefer pure transformations.

### 15.2 TypeScript/Vue

- Use `<script setup lang="ts">` style.
- Type all props and store state.
- Keep components focused on one tool; do not bloat with unrelated logic.

### 15.3 Git & Repo Hygiene

- Branch names: `feature/<description>`, `fix/<description>`.
- Keep PRs/commits small and tool‑scoped where possible.

---

## 16. Initial README Skeleton

```markdown
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

- [ ] Core tool abstractions
- [ ] Basic tools: JSON formatter, JSON/YAML, Base64, UUID, hashes
- [ ] CLI with `list`, `describe`, `run`
- [ ] HTTP API and Vue UI
- [ ] Plugin discovery for external tools
- [ ] Desktop packaging

DevFoundry is primarily a personal productivity project, but the architecture is designed for long-term extensibility.
```

---

This document is a living design reference; as the implementation evolves, update the architecture, tool list, and examples to match the actual codebase.

