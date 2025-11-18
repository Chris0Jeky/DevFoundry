# DevFoundry Demo Showcase

Interactive demonstrations of all DevFoundry tools with visual output and pipeline workflows.

## 🎯 Available Demos

### 1. Command-Line Demo (Recommended)
Beautiful terminal-based demo with colored output showcasing all 5 tools plus a pipeline demonstration.

**Windows (PowerShell):**
```powershell
cd demo
.\run-demo.ps1

# Fast mode (shorter delays)
.\run-demo.ps1 -Fast

# No pauses (runs continuously)
.\run-demo.ps1 -NoPause

# Both options
.\run-demo.ps1 -Fast -NoPause
```

**Linux/macOS (Bash):**
```bash
cd demo
./run-demo.sh

# Fast mode (shorter delays)
./run-demo.sh --fast

# No pauses (runs continuously)
./run-demo.sh --no-pause

# Both options
./run-demo.sh --fast --no-pause
```

### 2. Web-Based Interactive Demo
Try all tools directly in your browser with a beautiful GUI.

**Setup:**
1. Start the DevFoundry API:
   ```bash
   cd src/DevFoundry.Api
   dotnet run
   ```

2. Open the demo in your browser:
   ```
   demo/web-demo.html
   ```
   Or simply double-click the file.

3. The demo will automatically detect if the API is running and show connection status.

## 📁 Demo Directory Structure

```
demo/
├── data/                      # Sample input files
│   ├── sample.json           # Minified JSON for formatting demo
│   ├── sample.yaml           # YAML for conversion demo
│   ├── message.txt           # Text for Base64/Hash demo
│   ├── secret.txt            # Secret for encoding demo
│   └── api-response.json     # Realistic API response for pipeline
│
├── output/                    # Generated output files
│   ├── sample-formatted.json
│   ├── sample-from-yaml.json
│   ├── secret-encoded.txt
│   ├── generated-uuids.txt
│   ├── message-hashes.txt
│   └── pipeline-results.txt
│
├── run-demo.ps1              # PowerShell demo script
├── run-demo.sh               # Bash demo script
├── web-demo.html             # Interactive web demo
└── README.md                 # This file
```

## 🛠️ Tools Demonstrated

### 1. JSON Formatter (`json.formatter`)
- Formats minified JSON with custom indentation
- Minifies formatted JSON back to single line
- **Demo**: Load sample.json → Format → Minify

### 2. JSON/YAML Converter (`json.yaml`)
- Converts YAML to JSON
- ⚠️ **Note**: JSON-to-YAML has a known bug (documented in TEST_REPORT.md)
- **Demo**: Load sample.yaml → Convert to JSON

### 3. Base64 Encoder (`encoding.base64`)
- Encodes text to Base64
- Decodes Base64 back to text
- **Demo**: Load secret → Encode → Decode

### 4. UUID Generator (`generation.uuid`)
- Generates unique UUIDs (1-100 at a time)
- **Demo**: Generate 5 random UUIDs

### 5. Hash Calculator (`crypto.hash`)
- Supports MD5, SHA-1, SHA-256, SHA-512
- **Demo**: Hash a message with all algorithms

## 🔗 Pipeline Demonstration

The demo includes a real-world pipeline workflow:

1. **Load** realistic API response (JSON)
2. **Format** it for readability (JSON Formatter)
3. **Generate** session IDs (UUID Generator)
4. **Hash** the formatted response for integrity (Hash Calculator)
5. **Encode** the hash (Base64 Encoder)

**Output**: Complete pipeline results saved to `output/pipeline-results.txt`

## 🎨 Features

### Command-Line Demo
- ✨ Colorized output for better readability
- ⏸️ Interactive pauses between demonstrations
- 📝 Shows input, process, and output for each tool
- 💾 Saves all outputs to files
- 🚀 Fast mode and no-pause mode options

### Web Demo
- 🎨 Modern, beautiful gradient UI
- 🔄 Real-time API status indicator
- 📋 Sample data loaders for each tool
- 🎯 Interactive parameter configuration
- 🔗 Complete pipeline demo in browser
- ✅ Success/error indicators with colored output

## 📝 Example Output

**JSON Formatter:**
```
Input (Minified):
{"name":"DevFoundry","version":"0.1.0"}

Output (Formatted):
{
  "name": "DevFoundry",
  "version": "0.1.0"
}
```

**UUID Generator:**
```
550e8400-e29b-41d4-a716-446655440000
6ba7b810-9dad-11d1-80b4-00c04fd430c8
6ba7b811-9dad-11d1-80b4-00c04fd430c8
```

**Hash Calculator:**
```
MD5:     5d41402abc4b2a76b9719d911017c592
SHA-256: 2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824
```

## 🎬 Quick Start

### Fastest Way to See Everything
```powershell
# Windows
cd demo
.\run-demo.ps1 -Fast

# Linux/macOS
cd demo
./run-demo.sh --fast
```

### Interactive Web Experience
```bash
# Terminal 1: Start API
cd src/DevFoundry.Api
dotnet run

# Terminal 2: Open browser to demo/web-demo.html
```

## 📚 Additional Resources

- **CLAUDE.md** - Working with this codebase
- **MASTER_PLAN.md** - Implementation roadmap
- **DEVELOPMENT.md** - Development guide
- **TEST_REPORT.md** - Latest test results and known issues
- **README.md** - Project overview

## 🐛 Known Issues

- **JSON-to-YAML conversion** has a bug (returns "valueKind: Object")
- YAML-to-JSON works correctly
- See TEST_REPORT.md for full details and planned fix

## 💡 Tips

- Run the command-line demo first to see all tools in action
- Use the web demo for interactive experimentation
- Check `output/` directory for all generated files
- The pipeline demo shows how tools can be chained together
- Use `-Fast` or `--fast` if you're short on time

---

**Enjoy exploring DevFoundry!** 🛠️
