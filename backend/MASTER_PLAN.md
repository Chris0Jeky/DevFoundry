# DevFoundry - Master Implementation Plan

**Version**: 1.1
**Last Updated**: 2025-11-18 (Updated after comprehensive testing)
**Status**: Active Development - **CRITICAL BUG IDENTIFIED**

## Project Status

### Completed ✅
- Core plugin architecture (DevFoundry.Core)
- Tool registry and runtime (DevFoundry.Runtime)
- Basic tool implementations (5 tools)
- CLI interface with list, describe, and run commands
- REST API with tool listing and execution endpoints
- Basic unit tests for core and tools (16 tests, all passing)
- Documentation (README.md, CLAUDE.md, DEVELOPMENT.md, TEST_REPORT.md)
- Project configuration (.gitignore, .editorconfig, Directory.Build.props, global.json)
- Comprehensive testing completed (2025-11-18)

### Current State Analysis

**What Works:**
- ✅ Build succeeds without errors or warnings
- ✅ All 16 unit tests pass (100% pass rate)
- ✅ CLI is functional with piping support (list, describe, run commands)
- ✅ API endpoints are functional with CORS configured
- ✅ Plugin architecture is clean and extensible
- ✅ Dependency injection is properly configured
- ✅ 4 out of 5 tools working correctly (JSON Formatter, Base64, UUID, Hash)
- ✅ Error handling with proper exit codes and messages
- ✅ Project configuration files in place

**Critical Issues:**
- 🔴 **JsonYamlConverterTool JSON-to-YAML conversion is BROKEN** (See Phase 0)
  - Returns "valueKind: Object" instead of proper YAML
  - Root cause: `JsonSerializer.Deserialize<object>()` returns JsonElement
  - YAML-to-JSON works correctly
  - **BLOCKING PRODUCTION DEPLOYMENT**

**What's Missing:**
- ❌ JsonYamlConverterTool has no tests (This is why bug wasn't caught!)
- ❌ No tests for DevFoundry.Runtime (0% coverage)
- ❌ No tests for DevFoundry.Api (0% coverage)
- ❌ No tests for DevFoundry.Cli (0% coverage)
- ❌ No integration tests
- ❌ No CI/CD configuration
- ❌ No API documentation (OpenAPI/Swagger)
- ❌ No input size limits (potential DoS vulnerability)
- ❌ Generic exception catching in 3 tool files
- ❌ No XML documentation on public APIs
- ❌ No performance benchmarks
- ❌ No logging/telemetry
- ❌ No configuration management

**Overall Test Coverage**: ~45% (Target: 80%)

---

## Implementation Roadmap

### Phase 0: 🔴 CRITICAL BUG FIX (Priority: URGENT)

**Status**: **MUST FIX BEFORE PRODUCTION**
**Discovered**: 2025-11-18 during comprehensive testing
**Blocking**: Production deployment

#### 0.1 Fix JsonYamlConverterTool Bug
**Goal**: Restore JSON-to-YAML conversion functionality

**Tasks:**
- [ ] **STEP 1: Add comprehensive tests FIRST (TDD approach)**
  - [ ] Test JSON-to-YAML with simple object
  - [ ] Test JSON-to-YAML with nested objects
  - [ ] Test JSON-to-YAML with arrays
  - [ ] Test JSON-to-YAML with mixed types
  - [ ] Test YAML-to-JSON (already works, but add regression tests)
  - [ ] Test error cases (invalid JSON, invalid YAML)
  - [ ] All tests should FAIL initially (TDD red phase)

- [ ] **STEP 2: Fix the bug**
  - [ ] Replace `JsonSerializer.Deserialize<object>(json)` approach
  - [ ] Use `JsonDocument.Parse()` and properly handle JsonElement
  - [ ] OR deserialize to `Dictionary<string, object>` / `List<object>`
  - [ ] Configure YamlDotNet to handle the correct object type
  - [ ] Location: `src/DevFoundry.Tools.Basic/JsonYamlConverterTool.cs:76`

- [ ] **STEP 3: Verify fix**
  - [ ] All tests should PASS (TDD green phase)
  - [ ] Manual testing via CLI: `echo '{"name":"test"}' | dotnet run -- run json.yaml`
  - [ ] Manual testing via API
  - [ ] Test with complex nested structures
  - [ ] Test with edge cases

**Estimate**: 1-2 hours
**Dependencies**: None
**Blocking**: ALL other phases

**Reference**: See TEST_REPORT.md for detailed bug analysis

---

### Phase 1: Foundation & Quality (Priority: HIGH)

#### 1.1 Testing Infrastructure
**Goal**: Achieve comprehensive test coverage

**Tasks:**
- [ ] Add tests for JsonYamlConverterTool (json-to-yaml, yaml-to-json, error cases)
- [ ] Add DevFoundry.Runtime.Tests project
  - [ ] Test ToolRegistry.GetAllTools()
  - [ ] Test ToolRegistry.GetTool() with valid/invalid IDs
  - [ ] Test case-insensitive tool ID lookup
  - [ ] Test ServiceCollectionExtensions.AddDevFoundryRuntime()
  - [ ] Test ServiceCollectionExtensions.AddTool<T>()
- [ ] Add DevFoundry.Api.Tests project
  - [ ] Test GET /api/tools endpoint
  - [ ] Test POST /api/tools/{toolId}/run endpoint
  - [ ] Test 404 for non-existent tools
  - [ ] Test CORS configuration
  - [ ] Test DTO mapping
- [ ] Add DevFoundry.Cli.Tests project
  - [ ] Test ListCommand output formatting
  - [ ] Test ListCommand --category filter
  - [ ] Test DescribeCommand output
  - [ ] Test DescribeCommand error handling
  - [ ] Test RunCommand with --text, --file, stdin
  - [ ] Test RunCommand parameter parsing
  - [ ] Test RunCommand exit codes
- [ ] Add integration tests
  - [ ] CLI end-to-end tests for each tool
  - [ ] API end-to-end tests for each tool
  - [ ] Tool chaining scenarios (if applicable)

**Estimate**: 3-5 days
**Dependencies**: None

#### 1.2 Project Configuration ✅ COMPLETED (2025-11-18)
**Goal**: Establish development standards and tooling

**Tasks:**
- [x] Create .gitignore file (standard .NET template)
  - [x] bin/, obj/ folders
  - [x] .vs/, .idea/, .vscode/ folders
  - [x] *.user, *.suo files
  - [x] NuGet packages
- [x] Create .editorconfig file
  - [x] Coding style (indentation, line endings, etc.)
  - [x] Naming conventions
  - [x] Code analysis rules
- [x] Create Directory.Build.props
  - [x] Centralize common properties (version, company, copyright)
  - [x] Enable nullable reference types globally
  - [x] Enable warnings as errors for production builds
  - [x] Configure code analysis
- [x] Create global.json to pin .NET SDK version
- [x] Simplified test project files (removed duplicates)
- [ ] Create nuget.config for package sources (if needed) - OPTIONAL

**Estimate**: 1 day (COMPLETED in < 1 day)
**Dependencies**: None
**Status**: ✅ COMPLETE

#### 1.3 CI/CD Pipeline
**Goal**: Automate build, test, and deployment

**Tasks:**
- [ ] Create GitHub Actions workflow (or equivalent)
  - [ ] Build on push/PR
  - [ ] Run all tests
  - [ ] Generate test coverage report
  - [ ] Run code analysis/linting
  - [ ] Publish artifacts on release
- [ ] Set up semantic versioning
- [ ] Configure automated releases
- [ ] Add build status badges to README

**Estimate**: 2 days
**Dependencies**: 1.1, 1.2

---

### Phase 2: Core Enhancements (Priority: MEDIUM)

#### 2.1 API Documentation & Observability
**Goal**: Make API discoverable and observable

**Tasks:**
- [ ] Add Swagger/OpenAPI support to DevFoundry.Api
  - [ ] Install Swashbuckle.AspNetCore
  - [ ] Configure Swagger UI
  - [ ] Add XML documentation comments
  - [ ] Add API versioning support
- [ ] Add logging framework
  - [ ] Integrate Serilog or Microsoft.Extensions.Logging
  - [ ] Add structured logging to API endpoints
  - [ ] Add structured logging to CLI commands
  - [ ] Add logging to tool execution
  - [ ] Configure different log levels for development/production
- [ ] Add health checks to API
  - [ ] /health endpoint
  - [ ] Tool registry status
  - [ ] Dependency health checks

**Estimate**: 3 days
**Dependencies**: None

#### 2.2 Configuration Management
**Goal**: Make the system configurable without code changes

**Tasks:**
- [ ] Add appsettings.json support to API
  - [ ] Configure CORS origins from settings
  - [ ] Configure logging from settings
  - [ ] Configure API port/host from settings
- [ ] Add configuration support to CLI
  - [ ] User preferences file (~/.devfoundry/config.json)
  - [ ] Default tool parameters
  - [ ] Output formatting preferences
- [ ] Add environment-specific configurations (Development, Staging, Production)

**Estimate**: 2 days
**Dependencies**: None

#### 2.3 Tool Lifecycle Management
**Goal**: Support tool versioning and deprecation

**Tasks:**
- [ ] Add Version property to ToolDescriptor
- [ ] Add IsDeprecated flag to ToolDescriptor
- [ ] Add DeprecationMessage to ToolDescriptor
- [ ] Update CLI to show deprecation warnings
- [ ] Update API to include version/deprecation in responses
- [ ] Create tool migration guide template

**Estimate**: 2 days
**Dependencies**: None

---

### Phase 3: Advanced Features (Priority: MEDIUM-LOW)

#### 3.1 Additional Tool Categories
**Goal**: Expand tool library with new categories

**Potential Tools:**
- [ ] **Text Tools**
  - [ ] Case converter (upper, lower, title, camel, pascal, snake, kebab)
  - [ ] String escape/unescape (JSON, XML, HTML, URL, CSV)
  - [ ] Lorem ipsum generator
  - [ ] Word/character counter
  - [ ] Diff tool (text comparison)
- [ ] **Time Tools**
  - [ ] Unix timestamp converter
  - [ ] Timezone converter
  - [ ] Date formatter
  - [ ] Duration calculator
- [ ] **URL Tools**
  - [ ] URL encoder/decoder
  - [ ] Query string parser
  - [ ] URL builder
- [ ] **JWT Tools**
  - [ ] JWT decoder
  - [ ] JWT generator (with signing)
  - [ ] JWT validator
- [ ] **Regular Expression Tools**
  - [ ] Regex tester
  - [ ] Regex builder (from examples)
  - [ ] Regex explainer
- [ ] **Number Tools**
  - [ ] Number base converter (binary, octal, hex, decimal)
  - [ ] Number formatter (thousand separators, locales)
- [ ] **Image Tools** (requires image processing library)
  - [ ] Image resizer
  - [ ] Image format converter
  - [ ] QR code generator
  - [ ] Barcode generator
- [ ] **Network Tools**
  - [ ] IP address parser/validator
  - [ ] CIDR calculator
  - [ ] MAC address formatter
- [ ] **Color Tools**
  - [ ] Color converter (HEX, RGB, HSL, HSV)
  - [ ] Color palette generator

**Estimate**: 1-2 days per category
**Dependencies**: Phase 1 complete

#### 3.2 Tool Composition & Pipelines
**Goal**: Allow chaining multiple tools together

**Tasks:**
- [ ] Design pipeline specification format (JSON/YAML)
- [ ] Implement pipeline executor in Runtime
- [ ] Add CLI support for running pipelines
- [ ] Add API endpoint for pipeline execution
- [ ] Add pipeline validation
- [ ] Create pipeline examples/templates
- [ ] Add tests for pipeline execution

**Estimate**: 5 days
**Dependencies**: Phase 1 complete

#### 3.3 Plugin System
**Goal**: Allow external tool plugins to be loaded dynamically

**Tasks:**
- [ ] Design plugin manifest format
- [ ] Implement plugin discovery from directories
- [ ] Add plugin loading/unloading
- [ ] Add plugin security/sandboxing (if needed)
- [ ] Add CLI commands for plugin management (list, add, remove)
- [ ] Add API endpoints for plugin information
- [ ] Create plugin development guide
- [ ] Create sample plugin project template

**Estimate**: 7-10 days
**Dependencies**: Phase 2 complete

---

### Phase 4: Performance & Scalability (Priority: LOW)

#### 4.1 Performance Optimization
**Goal**: Optimize tool execution and API response times

**Tasks:**
- [ ] Add BenchmarkDotNet project
- [ ] Create benchmarks for each tool
- [ ] Create benchmarks for registry lookup
- [ ] Profile API endpoint performance
- [ ] Optimize hot paths identified in profiling
- [ ] Add caching for tool descriptors (if beneficial)
- [ ] Add response caching to API (if applicable)

**Estimate**: 3-4 days
**Dependencies**: Phase 1 complete

#### 4.2 Batch Processing
**Goal**: Support processing multiple inputs efficiently

**Tasks:**
- [ ] Add batch execution support to ITool interface
- [ ] Implement batch execution in tools
- [ ] Add CLI support for batch input (from files, directories)
- [ ] Add API endpoint for batch execution
- [ ] Add progress reporting for batch operations
- [ ] Add cancellation support for long-running operations

**Estimate**: 5 days
**Dependencies**: Phase 1 complete

---

### Phase 5: Developer Experience (Priority: MEDIUM)

#### 5.1 CLI Enhancements
**Goal**: Improve CLI usability and discoverability

**Tasks:**
- [ ] Add shell autocompletion (bash, zsh, PowerShell)
- [ ] Add interactive mode (REPL)
- [ ] Add tool favorites/aliases
- [ ] Add command history
- [ ] Add colorized output (with --no-color flag)
- [ ] Add --json output flag for scripting
- [ ] Add --quiet flag for minimal output
- [ ] Add --version flag
- [ ] Improve error messages with suggestions

**Estimate**: 4-5 days
**Dependencies**: Phase 1 complete

#### 5.2 Distribution & Packaging
**Goal**: Make installation and updates easy

**Tasks:**
- [ ] Publish CLI as .NET Global Tool
- [ ] Create NuGet packages for Core, Runtime, Tools.Basic
- [ ] Create Docker image for API
- [ ] Create installation scripts (Windows, macOS, Linux)
- [ ] Add update check mechanism to CLI
- [ ] Create Homebrew formula (macOS)
- [ ] Create Chocolatey package (Windows)
- [ ] Add to winget repository (Windows)

**Estimate**: 3-4 days
**Dependencies**: Phase 1, 1.3 complete

---

## Technical Debt & Code Quality

### Current Issues

1. **Missing Tests**
   - JsonYamlConverterTool: 0% coverage
   - DevFoundry.Runtime: 0% coverage
   - DevFoundry.Api: 0% coverage
   - DevFoundry.Cli: 0% coverage
   - Integration tests: 0 tests

2. **Code Organization**
   - DTOs defined inline in API Program.cs should be in separate files ✅ (Already fixed)
   - No validation attributes on DTOs
   - No input validation in tools (parameter ranges, formats)

3. **Error Handling**
   - Generic Exception catch blocks in Base64Tool and HashTool
   - No logging of exceptions
   - No error codes for categorizing failures

4. **Security**
   - No rate limiting on API endpoints
   - No authentication/authorization (may be intentional for offline tool)
   - No input size limits (potential DoS via large inputs)
   - UuidTool allows up to 100 UUIDs but no similar limits on other tools

5. **Documentation**
   - No XML documentation comments
   - No inline code comments explaining complex logic
   - No API documentation (Swagger)
   - No examples in tool descriptions

6. **Configuration**
   - CORS origins hardcoded in Program.cs
   - No configuration files
   - No environment-specific settings

### Refactoring Opportunities

1. **Extract Common Tool Patterns**
   - Create base class for tools with common parameter parsing
   - Create helper methods for common error responses
   - Extract parameter validation logic

2. **Standardize Error Handling**
   - Create custom exception types
   - Implement consistent error response format
   - Add error codes/categories

3. **Improve Parameter Handling**
   - Add parameter validation attributes
   - Create strongly-typed parameter objects
   - Add parameter builder/fluent API

4. **Tool Discovery Enhancement**
   - Add metadata caching
   - Add tool categories to registry queries
   - Add tool search/filtering capabilities

---

## Future Considerations

### Architecture Evolution

1. **Microservices**
   - Split tools into separate services
   - Add API gateway
   - Add service discovery

2. **Event-Driven**
   - Add event bus for tool execution
   - Add audit logging via events
   - Add metrics collection via events

3. **Multi-Tenancy**
   - Add user authentication
   - Add user-specific tool configurations
   - Add usage quotas/limits

4. **Cloud Integration**
   - Deploy API as Azure Functions / AWS Lambda
   - Add managed identity support
   - Add cloud storage for user data

### Tool Enhancement Ideas

1. **AI-Powered Tools**
   - Text summarization
   - Code generation
   - Data extraction from text

2. **Database Tools**
   - SQL formatter
   - Connection string builder
   - Query result to JSON/CSV converter

3. **API Tools**
   - HTTP request builder
   - API response formatter
   - Postman collection converter

4. **Cryptography Tools**
   - Encryption/decryption (AES, RSA)
   - Digital signatures
   - Certificate parsing
   - Key generation

---

## Success Metrics

### Phase 1 Success Criteria
- ✅ Test coverage > 80% for all projects
- ✅ All builds pass in CI
- ✅ Code analysis produces no warnings
- ✅ .gitignore prevents committing build artifacts

### Phase 2 Success Criteria
- ✅ Swagger UI accessible and functional
- ✅ Logs provide actionable information for debugging
- ✅ Configuration changes don't require recompilation

### Phase 3 Success Criteria
- ✅ 20+ tools available
- ✅ Pipeline execution works for common scenarios
- ✅ External plugins can be loaded successfully

### Phase 4 Success Criteria
- ✅ API response time < 100ms for p95
- ✅ Tool execution time benchmarked and optimized
- ✅ Batch processing reduces overhead vs individual calls

### Phase 5 Success Criteria
- ✅ CLI can be installed via package manager
- ✅ Autocompletion works in major shells
- ✅ CLI available as .NET Global Tool

---

## Risk Assessment

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| Test coverage remains low | High | Medium | Prioritize Phase 1.1, enforce coverage in CI |
| Tool interface changes break compatibility | High | Low | Add versioning early (Phase 2.3) |
| Performance issues with many tools | Medium | Medium | Benchmark early (Phase 4.1) |
| Plugin security vulnerabilities | High | Medium | Add sandboxing (Phase 3.3) |
| Frontend-backend integration issues | Medium | Low | Already mitigated with CORS, add API versioning |

---

## Notes for Future Sessions

### Key Decisions Made
1. Using .NET 8 for broad compatibility
2. Minimal API design for simplicity
3. Plugin pattern for extensibility
4. Synchronous execution model (async can be added later)
5. Case-insensitive tool IDs for better UX

### Open Questions
1. Should tools support async execution?
2. Should we add a web-based UI in this repo or keep it separate?
3. What's the deployment model (self-hosted, cloud, both)?
4. Should we support tool versioning at the API level?
5. Do we need authentication/authorization?

### Quick Start for Next Session
```bash
# Build everything
dotnet build

# Run all tests
dotnet test

# Run CLI
cd src/DevFoundry.Cli
dotnet run -- list

# Run API
cd src/DevFoundry.Api
dotnet run
```

### Current Priority
**Focus on Phase 1** (Foundation & Quality) before adding new features. The project is functional but needs solid testing and infrastructure foundation.
