# DevFoundry Quick Start Guide

Get DevFoundry up and running in minutes!

## Prerequisites

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 20+** - [Download](https://nodejs.org/)

## 1. Clone and Setup

```bash
# Clone the repository
git clone <repository-url>
cd DevFoundry
```

## 2. Start the Backend API

```bash
cd backend
dotnet restore
dotnet run --project src/DevFoundry.Api/DevFoundry.Api.csproj
```

The API will start at `http://localhost:5000`.

## 3. Start the Frontend (New Terminal)

```bash
cd frontend/devfoundry-ui
npm install
npm run dev
```

The UI will start at `http://localhost:5173`.

## 4. Open Your Browser

Navigate to `http://localhost:5173` and start using DevFoundry!

## Using the CLI

```bash
cd backend

# List all available tools
dotnet run --project src/DevFoundry.Cli/DevFoundry.Cli.csproj -- list

# Get help for a specific tool
dotnet run --project src/DevFoundry.Cli/DevFoundry.Cli.csproj -- describe json.formatter

# Format JSON
echo '{"name":"test"}' | dotnet run --project src/DevFoundry.Cli/DevFoundry.Cli.csproj -- run json.formatter

# Generate UUIDs
dotnet run --project src/DevFoundry.Cli/DevFoundry.Cli.csproj -- run generation.uuid --param count=5

# Calculate hash
echo "hello world" | dotnet run --project src/DevFoundry.Cli/DevFoundry.Cli.csproj -- run crypto.hash --param algorithm=sha256
```

## Running Tests

```bash
# Backend tests
cd backend
dotnet test

# Frontend type checking
cd frontend/devfoundry-ui
npm run type-check
```

## Next Steps

- Read the [CONTRIBUTING.md](CONTRIBUTING.md) to learn how to add new tools
- Check out the [backend README](backend/README.md) for API details
- See the [frontend README](frontend/devfoundry-ui/README.md) for UI development

## Troubleshooting

### API won't start
- Ensure .NET 8 SDK is installed: `dotnet --version`
- Check if port 5000 is available

### Frontend won't start
- Ensure Node.js is installed: `node --version`
- Clear node_modules: `rm -rf node_modules && npm install`
- Check if port 5173 is available

### CORS errors
- Ensure the API is running on `http://localhost:5000`
- Check `VITE_API_BASE_URL` in frontend `.env` file
