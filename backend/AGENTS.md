# Repository Guidelines

This document guides contributors working on the DevFoundry backend (.NET 8) in `backend/`.

## Project Structure & Module Organization
- Source projects live under `src/` (e.g., `DevFoundry.Core`, `DevFoundry.Tools.Basic`, `DevFoundry.Runtime`, `DevFoundry.Cli`, `DevFoundry.Api`).
- Tests live under `tests/` with one test project per source project (for example, `tests/DevFoundry.Core.Tests`).
- Keep shared abstractions in `DevFoundry.Core`, tool implementations in `DevFoundry.Tools.*`, and avoid cross-project references that break this layering.

## Build, Test, and Development Commands
- `dotnet build` – build all projects in the solution.
- `dotnet test` – run the full test suite in `tests/`.
- `cd src/DevFoundry.Cli; dotnet run -- list` – run the CLI; use `describe` and `run` subcommands when adding tools.
- `cd src/DevFoundry.Api; dotnet run` – run the API locally at `http://localhost:5000`.

## Coding Style & Naming Conventions
- Use standard C# conventions: 4-space indentation, `PascalCase` for classes and public members, `camelCase` for locals and parameters.
- Keep files small and focused; one public type per file where practical.
- When adding tools, follow existing naming patterns like `JsonFormatterTool`, `Base64Tool`, and place them under appropriate `DevFoundry.Tools.*` namespaces.

## Testing Guidelines
- Use xUnit (as in `DevFoundry.Core.Tests` and `DevFoundry.Tools.Basic.Tests`) for all new tests.
- Name test classes `<TypeName>Tests` and methods with clear behavior-focused names.
- Ensure new features include tests; prefer adding to existing test projects that match the target assembly.

## Commit & Pull Request Guidelines
- Write concise, imperative commit messages (e.g., `Add SHA-512 hashing tool`, `Fix tool registry resolution`).
- For pull requests, include: a short summary, rationale, key changes, how to run tests, and any screenshots or example CLI/API invocations when UI or behavior changes are involved.
- Reference related issues (e.g., `Fixes #123`) when applicable.
