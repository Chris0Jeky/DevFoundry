#!/usr/bin/env pwsh
# DevFoundry Interactive Demo
# Showcases all tools with visual output and pipeline demonstrations

param(
    [switch]$Fast,
    [switch]$NoPause
)

$ErrorActionPreference = "Stop"

# Configuration
$CliPath = "../src/DevFoundry.Cli"
$DataPath = "./data"
$OutputPath = "./output"
$DelayMs = if ($Fast) { 500 } else { 1500 }

# Colors
function Write-Header {
    param([string]$Text)
    Write-Host "`n================================================" -ForegroundColor Cyan
    Write-Host "  $Text" -ForegroundColor Cyan
    Write-Host "================================================`n" -ForegroundColor Cyan
}

function Write-Step {
    param([string]$Text)
    Write-Host "► $Text" -ForegroundColor Yellow
}

function Write-Success {
    param([string]$Text)
    Write-Host "✓ $Text" -ForegroundColor Green
}

function Write-Info {
    param([string]$Text)
    Write-Host "ℹ $Text" -ForegroundColor Blue
}

function Show-Content {
    param(
        [string]$Label,
        [string]$Content,
        [string]$Color = "Gray"
    )
    Write-Host "`n$Label`:" -ForegroundColor Magenta
    Write-Host "─────────────────────────────────────────────" -ForegroundColor DarkGray
    Write-Host $Content -ForegroundColor $Color
    Write-Host "─────────────────────────────────────────────`n" -ForegroundColor DarkGray
}

function Pause-Demo {
    if (-not $NoPause) {
        Write-Host "`nPress any key to continue..." -ForegroundColor DarkGray
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    }
    Start-Sleep -Milliseconds $DelayMs
}

# Clear screen and show welcome
Clear-Host

Write-Host @"
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║              🛠️  DevFoundry Demo Showcase  🛠️                ║
║                                                              ║
║           Offline Developer Toolkit - .NET 8                ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝
"@ -ForegroundColor Cyan

Write-Host "`nThis demo will showcase all 5 tools available in DevFoundry:`n" -ForegroundColor White
Write-Host "  1. JSON Formatter       - Pretty-print and minify JSON" -ForegroundColor Gray
Write-Host "  2. JSON/YAML Converter  - Convert between formats" -ForegroundColor Gray
Write-Host "  3. Base64 Encoder       - Encode/decode Base64" -ForegroundColor Gray
Write-Host "  4. UUID Generator       - Generate unique identifiers" -ForegroundColor Gray
Write-Host "  5. Hash Calculator      - MD5, SHA-1, SHA-256, SHA-512`n" -ForegroundColor Gray

Pause-Demo

# ============================================================
# TOOL 1: JSON Formatter
# ============================================================
Write-Header "1. JSON Formatter (json.formatter)"

Write-Step "Loading minified JSON..."
$minifiedJson = Get-Content "$DataPath/sample.json" -Raw
Show-Content "Input (Minified)" $minifiedJson "DarkYellow"

Pause-Demo

Write-Step "Formatting JSON with indentation..."
$formattedJson = $minifiedJson | & dotnet run --project $CliPath -- run json.formatter --indentSize 2
Show-Content "Output (Formatted)" $formattedJson "Green"
$formattedJson | Out-File "$OutputPath/sample-formatted.json" -Encoding utf8
Write-Success "Saved to: output/sample-formatted.json"

Pause-Demo

Write-Step "Now minifying the formatted JSON..."
$minified = $formattedJson | & dotnet run --project $CliPath -- run json.formatter --minify true
Show-Content "Output (Minified)" $minified "Green"
Write-Success "JSON successfully formatted and minified!"

Pause-Demo

# ============================================================
# TOOL 2: JSON/YAML Converter (YAML to JSON works, JSON to YAML has known bug)
# ============================================================
Write-Header "2. JSON/YAML Converter (json.yaml)"

Write-Info "⚠️  Note: JSON-to-YAML has a known bug. Demonstrating YAML-to-JSON only."

Write-Step "Loading YAML configuration..."
$yamlContent = Get-Content "$DataPath/sample.yaml" -Raw
Show-Content "Input (YAML)" $yamlContent "DarkYellow"

Pause-Demo

Write-Step "Converting YAML to JSON..."
$convertedJson = $yamlContent | & dotnet run --project $CliPath -- run json.yaml --mode yaml-to-json
Show-Content "Output (JSON)" $convertedJson "Green"
$convertedJson | Out-File "$OutputPath/sample-from-yaml.json" -Encoding utf8
Write-Success "Saved to: output/sample-from-yaml.json"

Pause-Demo

# ============================================================
# TOOL 3: Base64 Encoder
# ============================================================
Write-Header "3. Base64 Encoder/Decoder (encoding.base64)"

Write-Step "Loading secret message..."
$message = Get-Content "$DataPath/secret.txt" -Raw
Show-Content "Input (Plain Text)" $message "DarkYellow"

Pause-Demo

Write-Step "Encoding to Base64..."
$encoded = $message | & dotnet run --project $CliPath -- run encoding.base64 --mode encode
Show-Content "Output (Base64 Encoded)" $encoded "Green"
$encoded | Out-File "$OutputPath/secret-encoded.txt" -Encoding utf8
Write-Success "Saved to: output/secret-encoded.txt"

Pause-Demo

Write-Step "Decoding back to plain text..."
$decoded = $encoded | & dotnet run --project $CliPath -- run encoding.base64 --mode decode
Show-Content "Output (Decoded)" $decoded "Green"
Write-Success "Successfully encoded and decoded!"

Pause-Demo

# ============================================================
# TOOL 4: UUID Generator
# ============================================================
Write-Header "4. UUID Generator (generation.uuid)"

Write-Step "Generating 5 unique UUIDs..."
$uuids = "" | & dotnet run --project $CliPath -- run generation.uuid --count 5
Show-Content "Output (UUIDs)" $uuids "Green"
$uuids | Out-File "$OutputPath/generated-uuids.txt" -Encoding utf8
Write-Success "Saved to: output/generated-uuids.txt"

Pause-Demo

# ============================================================
# TOOL 5: Hash Calculator
# ============================================================
Write-Header "5. Hash Calculator (crypto.hash)"

Write-Step "Loading message for hashing..."
$hashMessage = Get-Content "$DataPath/message.txt" -Raw
Show-Content "Input (Message)" $hashMessage "DarkYellow"

Pause-Demo

Write-Step "Calculating MD5 hash..."
$md5 = $hashMessage | & dotnet run --project $CliPath -- run crypto.hash --algorithm md5
Show-Content "MD5" $md5 "Cyan"

Write-Step "Calculating SHA-1 hash..."
$sha1 = $hashMessage | & dotnet run --project $CliPath -- run crypto.hash --algorithm sha1
Show-Content "SHA-1" $sha1 "Cyan"

Write-Step "Calculating SHA-256 hash..."
$sha256 = $hashMessage | & dotnet run --project $CliPath -- run crypto.hash --algorithm sha256
Show-Content "SHA-256" $sha256 "Cyan"

Write-Step "Calculating SHA-512 hash..."
$sha512 = $hashMessage | & dotnet run --project $CliPath -- run crypto.hash --algorithm sha512
Show-Content "SHA-512" $sha512 "Cyan"

# Save all hashes
@"
Message: $hashMessage

MD5:     $md5
SHA-1:   $sha1
SHA-256: $sha256
SHA-512: $sha512
"@ | Out-File "$OutputPath/message-hashes.txt" -Encoding utf8
Write-Success "Saved to: output/message-hashes.txt"

Pause-Demo

# ============================================================
# PIPELINE DEMONSTRATION
# ============================================================
Write-Header "🔗 Pipeline Demonstration"

Write-Info "Demonstrating a real-world workflow pipeline:"
Write-Host "  1. Load API response (JSON)" -ForegroundColor Gray
Write-Host "  2. Format it for readability" -ForegroundColor Gray
Write-Host "  3. Extract user ID and generate similar UUIDs" -ForegroundColor Gray
Write-Host "  4. Calculate hash of the formatted output`n" -ForegroundColor Gray

Pause-Demo

Write-Step "Step 1: Load API response..."
$apiResponse = Get-Content "$DataPath/api-response.json" -Raw
Show-Content "API Response (Raw)" $apiResponse "DarkYellow"

Pause-Demo

Write-Step "Step 2: Format JSON for readability..."
$formatted = $apiResponse | & dotnet run --project $CliPath -- run json.formatter --indentSize 2
Show-Content "Formatted Response" $formatted "Green"

Pause-Demo

Write-Step "Step 3: Generate batch of UUIDs for user sessions..."
$sessionIds = "" | & dotnet run --project $CliPath -- run generation.uuid --count 3
Show-Content "Generated Session IDs" $sessionIds "Cyan"

Pause-Demo

Write-Step "Step 4: Calculate integrity hash of the response..."
$responseHash = $formatted | & dotnet run --project $CliPath -- run crypto.hash --algorithm sha256
Show-Content "Response Hash (SHA-256)" $responseHash "Magenta"

# Save pipeline results
@"
=================================================
Pipeline Results - $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
=================================================

FORMATTED API RESPONSE:
$formatted

GENERATED SESSION IDs:
$sessionIds

INTEGRITY HASH (SHA-256):
$responseHash

=================================================
"@ | Out-File "$OutputPath/pipeline-results.txt" -Encoding utf8
Write-Success "Pipeline results saved to: output/pipeline-results.txt"

Pause-Demo

# ============================================================
# FINALE
# ============================================================
Write-Header "🎉 Demo Complete!"

Write-Success "All tools demonstrated successfully!`n"

Write-Info "Output files created:"
Write-Host "  • output/sample-formatted.json" -ForegroundColor Gray
Write-Host "  • output/sample-from-yaml.json" -ForegroundColor Gray
Write-Host "  • output/secret-encoded.txt" -ForegroundColor Gray
Write-Host "  • output/generated-uuids.txt" -ForegroundColor Gray
Write-Host "  • output/message-hashes.txt" -ForegroundColor Gray
Write-Host "  • output/pipeline-results.txt" -ForegroundColor Gray

Write-Host "`n"
Write-Info "Next steps:"
Write-Host "  • Try the tools yourself: cd ../src/DevFoundry.Cli && dotnet run -- list" -ForegroundColor Gray
Write-Host "  • View the API: cd ../src/DevFoundry.Api && dotnet run" -ForegroundColor Gray
Write-Host "  • Open the web demo: demo/web-demo.html" -ForegroundColor Gray

Write-Host "`n"
Write-Host "Thank you for exploring DevFoundry! 🛠️" -ForegroundColor Cyan
Write-Host ""
