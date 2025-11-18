# Contributing to DevFoundry

Thank you for your interest in contributing to DevFoundry!

## Development Setup

### Prerequisites

- .NET 8 SDK
- Node.js 20+ and npm

### Getting Started

1. Clone the repository
2. Install backend dependencies: `cd backend && dotnet restore`
3. Install frontend dependencies: `cd frontend/devfoundry-ui && npm install`

## Project Structure

```
devfoundry/
  backend/          - .NET backend
    src/            - Source projects
    tests/          - Test projects
  frontend/         - Vue frontend
    devfoundry-ui/  - Vue 3 SPA
```

## Adding a New Tool

### Backend

1. Create a new class in `DevFoundry.Tools.Basic` that implements `ITool`
2. Define the `ToolDescriptor` with metadata and parameters
3. Implement the `Execute` method
4. Register the tool in CLI and API `Program.cs`
5. Add unit tests in `DevFoundry.Tools.Basic.Tests`

Example:

```csharp
public sealed class MyTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "category.mytool",
        DisplayName = "My Tool",
        Description = "Does something useful",
        Category = ToolCategory.Other,
        Parameters = new[] { /* ... */ }
    };

    public ToolResult Execute(ToolInput input)
    {
        // Implementation
    }
}
```

### Frontend

1. Create a new panel component in `src/components/tools/`
2. Add it to the mapping in `ToolPanelContainer.vue`
3. Use the shared styles from `tool-panel-styles.css`

## Code Style

### Backend (C#)

- Use PascalCase for classes and public members
- Use camelCase for local variables and private fields
- Enable nullable reference types
- Follow .NET coding conventions

### Frontend (TypeScript/Vue)

- Use TypeScript for all new code
- Use `<script setup lang="ts">` style
- Type all props and emits
- Use Composition API

## Testing

### Backend

```bash
cd backend
dotnet test
```

### Frontend

```bash
cd frontend/devfoundry-ui
npm run type-check
```

## Pull Request Process

1. Create a feature branch from `main`
2. Make your changes with clear commit messages
3. Add tests for new functionality
4. Ensure all tests pass
5. Update documentation as needed
6. Submit a pull request

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
