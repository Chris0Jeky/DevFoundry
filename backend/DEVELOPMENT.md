# DevFoundry - Development & Testing Guide

**Last Updated**: 2025-11-18

This document outlines the development practices, testing strategies, and quality standards for the DevFoundry project.

---

## Table of Contents
1. [Development Environment Setup](#development-environment-setup)
2. [Development Workflow](#development-workflow)
3. [Testing Strategy](#testing-strategy)
4. [Code Quality Standards](#code-quality-standards)
5. [Common Development Tasks](#common-development-tasks)
6. [Troubleshooting](#troubleshooting)

---

## Development Environment Setup

### Prerequisites
- .NET 8 SDK (8.0.415 or later)
- IDE: Visual Studio 2022, VS Code, or JetBrains Rider
- Git for version control

### Initial Setup
```bash
# Clone the repository
git clone <repository-url>
cd backend

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Verify CLI works
cd src/DevFoundry.Cli
dotnet run -- list
cd ../..

# Verify API works
cd src/DevFoundry.Api
dotnet run
# Open browser to http://localhost:5000/api/tools
cd ../..
```

### Recommended IDE Extensions
- **Visual Studio Code**:
  - C# Dev Kit
  - .NET Extension Pack
  - EditorConfig for VS Code
  - GitLens

- **Visual Studio 2022**:
  - ReSharper (optional but recommended)
  - CodeMaid (for code cleanup)

- **JetBrains Rider**:
  - Built-in .NET support
  - Built-in test runner

---

## Development Workflow

### Branching Strategy
```
main
  ├── feature/<feature-name>
  ├── bugfix/<bug-name>
  ├── hotfix/<hotfix-name>
  └── release/<version>
```

### Making Changes

1. **Create a Feature Branch**
   ```bash
   git checkout -b feature/add-regex-tool
   ```

2. **Make Changes Following Standards**
   - Write code following the code quality standards below
   - Add tests for new functionality
   - Update documentation if needed

3. **Run Local Verification**
   ```bash
   # Build
   dotnet build

   # Run tests
   dotnet test

   # Test manually (CLI)
   cd src/DevFoundry.Cli
   dotnet run -- describe <your-tool-id>
   dotnet run -- run <your-tool-id> --text "test input"
   ```

4. **Commit Changes**
   ```bash
   git add .
   git commit -m "feat: add regex tester tool"
   ```

5. **Push and Create PR**
   ```bash
   git push origin feature/add-regex-tool
   # Create PR via GitHub/GitLab/etc.
   ```

### Commit Message Convention
Follow [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `test`: Adding or updating tests
- `refactor`: Code refactoring
- `perf`: Performance improvements
- `chore`: Maintenance tasks
- `ci`: CI/CD changes

**Examples:**
```
feat(tools): add regex tester tool
fix(api): handle null parameters correctly
docs(readme): update installation instructions
test(core): add tests for ToolRegistry
refactor(cli): extract parameter parsing logic
```

---

## Testing Strategy

### Testing Pyramid

```
           /\
          /  \         E2E Tests (Few)
         /____\        - Integration tests
        /      \       - API endpoint tests
       /________\      - CLI command tests
      /          \
     /____________\    Unit Tests (Many)
                       - Tool tests
                       - Core logic tests
                       - Registry tests
```

### Test Organization

```
tests/
├── DevFoundry.Core.Tests/          # Unit tests for core abstractions
├── DevFoundry.Runtime.Tests/       # Unit tests for runtime services
├── DevFoundry.Tools.Basic.Tests/   # Unit tests for basic tools
├── DevFoundry.Api.Tests/           # Unit tests for API endpoints
├── DevFoundry.Cli.Tests/           # Unit tests for CLI commands
└── DevFoundry.Integration.Tests/   # Integration tests
```

### Writing Unit Tests

**Test Naming Convention:**
```
MethodName_Scenario_ExpectedBehavior
```

**Examples:**
```csharp
Execute_ValidInput_ReturnsSuccess
Execute_NullInput_ReturnsError
Execute_InvalidParameter_ReturnsError
GetTool_ValidId_ReturnsTool
GetTool_InvalidId_ReturnsNull
GetTool_CaseInsensitiveId_ReturnsTool
```

**Test Structure (AAA Pattern):**
```csharp
[Fact]
public void Execute_ValidJson_ReturnsFormattedOutput()
{
    // Arrange
    var tool = new JsonFormatterTool();
    var input = new ToolInput
    {
        Text = "{\"name\":\"Alice\"}"
    };

    // Act
    var result = tool.Execute(input);

    // Assert
    Assert.True(result.Success);
    Assert.NotNull(result.OutputText);
    Assert.Contains("\"name\"", result.OutputText);
}
```

### Test Coverage Goals

| Component | Target Coverage | Current Coverage |
|-----------|----------------|------------------|
| DevFoundry.Core | 90% | ~50% |
| DevFoundry.Runtime | 90% | 0% |
| DevFoundry.Tools.Basic | 90% | ~85% (missing JsonYaml tests) |
| DevFoundry.Api | 80% | 0% |
| DevFoundry.Cli | 80% | 0% |

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run tests for specific project
dotnet test tests/DevFoundry.Core.Tests

# Run specific test
dotnet test --filter "FullyQualifiedName~JsonFormatterToolTests.Execute_ValidJson"

# Run tests with detailed output
dotnet test --verbosity detailed

# Run tests and generate HTML report (requires ReportGenerator)
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
reportgenerator -reports:coverage.cobertura.xml -targetdir:coveragereport
```

### Test Checklist for New Tools

When creating a new tool, ensure you have tests for:

- ✅ Successful execution with valid input
- ✅ Error handling for null/empty input
- ✅ Error handling for invalid input format
- ✅ Parameter parsing and validation
- ✅ Default parameter values
- ✅ Edge cases specific to the tool
- ✅ Tool descriptor metadata is correct
- ✅ Output format matches expectations

**Template:**
```csharp
public class NewToolTests
{
    private readonly NewTool _tool = new();

    [Fact]
    public void Execute_ValidInput_ReturnsSuccess()
    {
        // Test happy path
    }

    [Fact]
    public void Execute_NullInput_ReturnsError()
    {
        // Test null input
    }

    [Fact]
    public void Execute_EmptyInput_ReturnsError()
    {
        // Test empty input
    }

    [Fact]
    public void Execute_InvalidInput_ReturnsError()
    {
        // Test invalid format
    }

    [Theory]
    [InlineData("param1", "value1")]
    [InlineData("param2", "value2")]
    public void Execute_WithParameters_AppliesCorrectly(string param, string value)
    {
        // Test parameter variations
    }

    [Fact]
    public void Descriptor_HasCorrectMetadata()
    {
        // Test descriptor properties
        Assert.Equal("expected.id", _tool.Descriptor.Id);
        Assert.NotEmpty(_tool.Descriptor.DisplayName);
        Assert.NotEmpty(_tool.Descriptor.Description);
    }
}
```

---

## Code Quality Standards

### Coding Conventions

1. **Naming**
   - PascalCase for types, methods, properties, public fields
   - camelCase for local variables, parameters, private fields
   - Prefix interfaces with `I` (e.g., `ITool`)
   - Suffix abstract classes with `Base` (e.g., `ToolBase`)

2. **File Organization**
   - One public type per file
   - File name matches type name
   - Organize usings alphabetically
   - Use `global using` in global usings file for common namespaces

3. **Documentation**
   - Add XML documentation for public APIs
   - Use `<summary>`, `<param>`, `<returns>`, `<exception>`
   - Keep comments up-to-date with code changes

   ```csharp
   /// <summary>
   /// Executes the tool with the provided input.
   /// </summary>
   /// <param name="input">The tool input containing text and parameters.</param>
   /// <returns>A <see cref="ToolResult"/> indicating success or failure.</returns>
   public ToolResult Execute(ToolInput input)
   {
       // Implementation
   }
   ```

4. **Error Handling**
   - Use specific exception types
   - Catch specific exceptions, not `Exception`
   - Provide meaningful error messages
   - Include context in error messages

   ```csharp
   // ❌ Bad
   catch (Exception ex)
   {
       return new ToolResult { Success = false, ErrorMessage = ex.Message };
   }

   // ✅ Good
   catch (JsonException ex)
   {
       return new ToolResult
       {
           Success = false,
           ErrorMessage = $"JSON parse error: {ex.Message}"
       };
   }
   ```

5. **Null Safety**
   - Enable nullable reference types
   - Use null-forgiving operator `!` sparingly
   - Validate parameters with ArgumentNullException
   - Use `?.`, `??`, `??=` operators appropriately

6. **Resource Management**
   - Use `using` statements for IDisposable
   - Prefer `using` declarations over blocks
   - Dispose of resources in all code paths

### Code Analysis

**Recommended Analyzers:**
- Microsoft.CodeAnalysis.NetAnalyzers (included with .NET SDK)
- StyleCop.Analyzers (for style consistency)
- SonarAnalyzer.CSharp (for code quality)

**Enable in Directory.Build.props:**
```xml
<PropertyGroup>
  <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  <EnableNETAnalyzers>true</EnableNETAnalyzers>
  <AnalysisLevel>latest</AnalysisLevel>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
</PropertyGroup>
```

### Performance Considerations

1. **String Handling**
   - Use `StringBuilder` for multiple concatenations
   - Use `string.Create` for complex string building
   - Use `Span<T>` for stack-allocated temporary buffers

2. **Collections**
   - Use `IReadOnlyList<T>` for immutable collections
   - Use `List<T>` over `IEnumerable<T>` when multiple iterations needed
   - Pre-size collections when count is known

3. **Allocations**
   - Avoid boxing value types
   - Reuse arrays/buffers where possible
   - Use object pooling for high-frequency allocations

4. **LINQ**
   - Avoid LINQ in hot paths (use loops instead)
   - Don't enumerate IEnumerable multiple times
   - Use `Any()` instead of `Count() > 0`

---

## Common Development Tasks

### Adding a New Tool

1. **Create the Tool Class**
   ```bash
   # Create file: src/DevFoundry.Tools.Basic/MyNewTool.cs
   ```

   ```csharp
   using DevFoundry.Core;

   namespace DevFoundry.Tools.Basic;

   public sealed class MyNewTool : ITool
   {
       public ToolDescriptor Descriptor { get; } = new()
       {
           Id = "category.toolname",
           DisplayName = "My New Tool",
           Description = "What this tool does.",
           Category = ToolCategory.Other,
           Tags = new[] { "tag1", "tag2" },
           Parameters = new[]
           {
               new ToolParameterDescriptor
               {
                   Name = "paramName",
                   DisplayName = "Parameter Name",
                   Description = "What this parameter does.",
                   Type = "string",
                   DefaultValue = "default"
               }
           }
       };

       public ToolResult Execute(ToolInput input)
       {
           // Validate input
           if (string.IsNullOrEmpty(input.Text))
           {
               return new ToolResult
               {
                   Success = false,
                   ErrorMessage = "No input text provided."
               };
           }

           // Get parameters
           var paramValue = input.Parameters.TryGetValue("paramName", out var p) && p is string s
               ? s
               : "default";

           try
           {
               // Execute tool logic
               var result = ProcessInput(input.Text, paramValue);

               return new ToolResult
               {
                   Success = true,
                   OutputText = result
               };
           }
           catch (Exception ex)
           {
               return new ToolResult
               {
                   Success = false,
                   ErrorMessage = $"Error: {ex.Message}"
               };
           }
       }

       private static string ProcessInput(string text, string param)
       {
           // Implementation
           return text;
       }
   }
   ```

2. **Register the Tool**

   In `src/DevFoundry.Cli/Program.cs`:
   ```csharp
   services.AddTool<MyNewTool>();
   ```

   In `src/DevFoundry.Api/Program.cs`:
   ```csharp
   builder.Services.AddTool<MyNewTool>();
   ```

3. **Create Tests**

   Create file: `tests/DevFoundry.Tools.Basic.Tests/MyNewToolTests.cs`
   ```csharp
   using DevFoundry.Core;
   using DevFoundry.Tools.Basic;
   using Xunit;

   namespace DevFoundry.Tools.Basic.Tests;

   public class MyNewToolTests
   {
       private readonly MyNewTool _tool = new();

       [Fact]
       public void Execute_ValidInput_ReturnsSuccess()
       {
           var input = new ToolInput
           {
               Text = "test input"
           };

           var result = _tool.Execute(input);

           Assert.True(result.Success);
           Assert.NotNull(result.OutputText);
       }

       // Add more tests...
   }
   ```

4. **Test the Tool**
   ```bash
   # Build
   dotnet build

   # Run tests
   dotnet test

   # Test via CLI
   cd src/DevFoundry.Cli
   dotnet run -- describe category.toolname
   dotnet run -- run category.toolname --text "test input"
   ```

### Adding a New Tool Category

1. Update `src/DevFoundry.Core/ToolCategory.cs`:
   ```csharp
   public enum ToolCategory
   {
       DataFormat,
       Encoding,
       Generation,
       Crypto,
       Time,
       Text,      // New category
       Other
   }
   ```

2. Create tools using the new category
3. Update documentation

### Debugging Tests

**Visual Studio:**
- Right-click test → Debug Test
- Set breakpoints in test or source code
- Use Test Explorer to see test results

**VS Code:**
- Install .NET Extension Pack
- Set breakpoints
- Run → Start Debugging → .NET Core Test

**Rider:**
- Right-click test → Debug
- Built-in test runner with debugging support

**Command Line:**
```bash
# Attach debugger
dotnet test --filter "FullyQualifiedName~MyTest"

# Use Debug.WriteLine for diagnostics
```

### Profiling Performance

```bash
# Install dotnet-trace
dotnet tool install --global dotnet-trace

# Run API
cd src/DevFoundry.Api
dotnet run &
PID=$!

# Collect trace
dotnet-trace collect --process-id $PID

# Generate flamegraph or analyze in PerfView/Visual Studio
```

---

## Troubleshooting

### Common Issues

#### 1. Build Fails with Package Restore Errors
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore

# Clean and rebuild
dotnet clean
dotnet build
```

#### 2. Tests Fail with "File Not Found"
- Ensure you're running from the solution root
- Check that project references are correct
- Verify test project .csproj has correct references

#### 3. CLI Tool Not Found
```bash
# Make sure you're in the CLI directory
cd src/DevFoundry.Cli

# Use full path to run
dotnet run --project src/DevFoundry.Cli -- list
```

#### 4. API CORS Errors
- Check CORS origins in `src/DevFoundry.Api/Program.cs`
- Verify frontend is running on allowed origin
- Check browser console for specific error

#### 5. Parameter Not Recognized in CLI
- Check parameter name matches ToolParameterDescriptor
- Ensure parameter type is correctly parsed (int, bool, string)
- Use `--param name=value` format

#### 6. Build Fails with "File is Locked" Errors
**Symptom**: Build fails with errors like `MSB3027: Could not copy "DevFoundry.Api.dll"` or `The file is locked by: "DevFoundry.Api (PID)"`

**Cause**: Orphaned DevFoundry.Api or DevFoundry.Cli processes still running from previous test sessions, locking DLL files.

**Solution**:

**Windows (PowerShell):**
```powershell
# Kill all DevFoundry processes
Get-Process | Where-Object {$_.ProcessName -like "*DevFoundry*"} | Stop-Process -Force

# Or use the cleanup script
.\scripts\cleanup-processes.ps1
```

**Windows (CMD):**
```cmd
# Kill specific process by PID (from error message)
taskkill /F /PID <PID>

# Kill all DevFoundry processes
taskkill /F /IM DevFoundry.Api.exe
taskkill /F /IM DevFoundry.Cli.exe
```

**Linux/macOS:**
```bash
# Kill all DevFoundry processes
pkill -f DevFoundry

# Or use the cleanup script
./scripts/cleanup-processes.sh
```

**Prevention**:
1. Always stop API/CLI processes when done testing
2. Use `Ctrl+C` to gracefully stop foreground processes
3. Run `dotnet clean` before switching branches
4. Use the provided cleanup scripts regularly
5. Consider using the API only for targeted endpoint tests, not long-running sessions

### Getting Help

1. Check existing documentation (README.md, CLAUDE.md, DEVELOPMENT.md)
2. Search closed issues in the repository
3. Review test cases for examples
4. Ask in team chat or create an issue

---

## Best Practices Summary

### ✅ Do
- Write tests before or alongside production code
- Keep tools simple and focused on one task
- Validate all inputs
- Provide clear error messages
- Use nullable reference types
- Document public APIs
- Follow naming conventions
- Run tests before committing
- Keep commits focused and atomic

### ❌ Don't
- Catch generic Exception without re-throwing
- Hardcode configuration values
- Skip writing tests "to save time"
- Leave commented-out code
- Use magic numbers/strings
- Ignore compiler warnings
- Commit build artifacts
- Push failing tests

---

## Continuous Improvement

### Regular Tasks

**Weekly:**
- Review and update documentation
- Check for outdated dependencies
- Review open issues and PRs

**Monthly:**
- Update .NET SDK version
- Review and update test coverage
- Performance benchmarking
- Security audit

**Quarterly:**
- Review architecture decisions
- Evaluate new tools/libraries
- Major dependency updates
- Review and update this guide

---

## Additional Resources

### Learning Resources
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [xUnit Documentation](https://xunit.net/)
- [ASP.NET Core Minimal APIs](https://docs.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [System.CommandLine Documentation](https://github.com/dotnet/command-line-api)

### Tools
- [.NET CLI Reference](https://docs.microsoft.com/dotnet/core/tools/)
- [NuGet Package Manager](https://www.nuget.org/)
- [BenchmarkDotNet](https://benchmarkdotnet.org/)
- [Coverlet](https://github.com/coverlet-coverage/coverlet) (code coverage)

---

## Appendix: Project Structure

```
DevFoundry/
├── src/
│   ├── DevFoundry.Core/           # Core abstractions
│   │   ├── ITool.cs
│   │   ├── ToolDescriptor.cs
│   │   ├── ToolInput.cs
│   │   ├── ToolResult.cs
│   │   ├── ToolCategory.cs
│   │   └── ToolParameterDescriptor.cs
│   │
│   ├── DevFoundry.Runtime/        # Runtime services
│   │   ├── IToolRegistry.cs
│   │   ├── ToolRegistry.cs
│   │   └── ServiceCollectionExtensions.cs
│   │
│   ├── DevFoundry.Tools.Basic/    # Built-in tools
│   │   ├── JsonFormatterTool.cs
│   │   ├── JsonYamlConverterTool.cs
│   │   ├── Base64Tool.cs
│   │   ├── UuidTool.cs
│   │   └── HashTool.cs
│   │
│   ├── DevFoundry.Cli/            # Command-line interface
│   │   ├── Program.cs
│   │   └── Commands/
│   │       ├── ListCommand.cs
│   │       ├── DescribeCommand.cs
│   │       └── RunCommand.cs
│   │
│   └── DevFoundry.Api/            # REST API
│       ├── Program.cs
│       └── DTOs/
│           ├── ToolDescriptorDto.cs
│           ├── ToolRunRequest.cs
│           └── ToolRunResult.cs
│
├── tests/
│   ├── DevFoundry.Core.Tests/
│   └── DevFoundry.Tools.Basic.Tests/
│
├── DevFoundry.sln
├── README.md
├── CLAUDE.md
├── MASTER_PLAN.md
└── DEVELOPMENT.md
```
