#!/usr/bin/env bash
# DevFoundry Interactive Demo
# Showcases all tools with visual output and pipeline demonstrations

set -e

# Configuration
CLI_PATH="../src/DevFoundry.Cli"
DATA_PATH="./data"
OUTPUT_PATH="./output"
FAST_MODE=false
NO_PAUSE=false

# Parse arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --fast) FAST_MODE=true; shift ;;
        --no-pause) NO_PAUSE=true; shift ;;
        *) shift ;;
    esac
done

DELAY=$([ "$FAST_MODE" = true ] && echo "0.5" || echo "1.5")

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
MAGENTA='\033[0;35m'
CYAN='\033[0;36m'
GRAY='\033[0;37m'
DARK_GRAY='\033[1;30m'
NC='\033[0m' # No Color

# Functions
write_header() {
    echo -e "\n${CYAN}================================================"
    echo -e "  $1"
    echo -e "================================================${NC}\n"
}

write_step() {
    echo -e "${YELLOW}► $1${NC}"
}

write_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

write_info() {
    echo -e "${BLUE}ℹ $1${NC}"
}

show_content() {
    local label=$1
    local content=$2
    local color=${3:-$GRAY}

    echo -e "\n${MAGENTA}${label}:${NC}"
    echo -e "${DARK_GRAY}─────────────────────────────────────────────${NC}"
    echo -e "${color}${content}${NC}"
    echo -e "${DARK_GRAY}─────────────────────────────────────────────${NC}\n"
}

pause_demo() {
    if [ "$NO_PAUSE" = false ]; then
        echo -e "${DARK_GRAY}\nPress any key to continue...${NC}"
        read -n 1 -s
    fi
    sleep $DELAY
}

# Clear screen and show welcome
clear

cat << "EOF"
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║              🛠️  DevFoundry Demo Showcase  🛠️                ║
║                                                              ║
║           Offline Developer Toolkit - .NET 8                ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝
EOF

echo -e "\n${NC}This demo will showcase all 5 tools available in DevFoundry:\n"
echo -e "  ${GRAY}1. JSON Formatter       - Pretty-print and minify JSON${NC}"
echo -e "  ${GRAY}2. JSON/YAML Converter  - Convert between formats${NC}"
echo -e "  ${GRAY}3. Base64 Encoder       - Encode/decode Base64${NC}"
echo -e "  ${GRAY}4. UUID Generator       - Generate unique identifiers${NC}"
echo -e "  ${GRAY}5. Hash Calculator      - MD5, SHA-1, SHA-256, SHA-512\n${NC}"

pause_demo

# ============================================================
# TOOL 1: JSON Formatter
# ============================================================
write_header "1. JSON Formatter (json.formatter)"

write_step "Loading minified JSON..."
minified_json=$(cat "$DATA_PATH/sample.json")
show_content "Input (Minified)" "$minified_json" "$YELLOW"

pause_demo

write_step "Formatting JSON with indentation..."
formatted_json=$(echo "$minified_json" | dotnet run --project "$CLI_PATH" -- run json.formatter --param indentSize=)
show_content "Output (Formatted)" "$formatted_json" "$GREEN"
echo "$formatted_json" > "$OUTPUT_PATH/sample-formatted.json"
write_success "Saved to: output/sample-formatted.json"

pause_demo

write_step "Now minifying the formatted JSON..."
minified=$(echo "$formatted_json" | dotnet run --project "$CLI_PATH" -- run json.formatter --param minify=true)
show_content "Output (Minified)" "$minified" "$GREEN"
write_success "JSON successfully formatted and minified!"

pause_demo

# ============================================================
# TOOL 2: JSON/YAML Converter
# ============================================================
write_header "2. JSON/YAML Converter (json.yaml)"

write_info "⚠️  Note: JSON-to-YAML has a known bug. Demonstrating YAML-to-JSON only."

write_step "Loading YAML configuration..."
yaml_content=$(cat "$DATA_PATH/sample.yaml")
show_content "Input (YAML)" "$yaml_content" "$YELLOW"

pause_demo

write_step "Converting YAML to JSON..."
converted_json=$(echo "$yaml_content" | dotnet run --project "$CLI_PATH" -- run json.yaml --param --mode yaml-to-json)
show_content "Output (JSON)" "$converted_json" "$GREEN"
echo "$converted_json" > "$OUTPUT_PATH/sample-from-yaml.json"
write_success "Saved to: output/sample-from-yaml.json"

pause_demo

# ============================================================
# TOOL 3: Base64 Encoder
# ============================================================
write_header "3. Base64 Encoder/Decoder (encoding.base64)"

write_step "Loading secret message..."
message=$(cat "$DATA_PATH/secret.txt")
show_content "Input (Plain Text)" "$message" "$YELLOW"

pause_demo

write_step "Encoding to Base64..."
encoded=$(echo "$message" | dotnet run --project "$CLI_PATH" -- run encoding.base64 --param --mode encode)
show_content "Output (Base64 Encoded)" "$encoded" "$GREEN"
echo "$encoded" > "$OUTPUT_PATH/secret-encoded.txt"
write_success "Saved to: output/secret-encoded.txt"

pause_demo

write_step "Decoding back to plain text..."
decoded=$(echo "$encoded" | dotnet run --project "$CLI_PATH" -- run encoding.base64 --param --mode decode)
show_content "Output (Decoded)" "$decoded" "$GREEN"
write_success "Successfully encoded and decoded!"

pause_demo

# ============================================================
# TOOL 4: UUID Generator
# ============================================================
write_header "4. UUID Generator (generation.uuid)"

write_step "Generating 5 unique UUIDs..."
uuids=$(echo "" | dotnet run --project "$CLI_PATH" -- run generation.uuid --param --count 5)
show_content "Output (UUIDs)" "$uuids" "$GREEN"
echo "$uuids" > "$OUTPUT_PATH/generated-uuids.txt"
write_success "Saved to: output/generated-uuids.txt"

pause_demo

# ============================================================
# TOOL 5: Hash Calculator
# ============================================================
write_header "5. Hash Calculator (crypto.hash)"

write_step "Loading message for hashing..."
hash_message=$(cat "$DATA_PATH/message.txt")
show_content "Input (Message)" "$hash_message" "$YELLOW"

pause_demo

write_step "Calculating MD5 hash..."
md5=$(echo "$hash_message" | dotnet run --project "$CLI_PATH" -- run crypto.hash --param --algorithm md5)
show_content "MD5" "$md5" "$CYAN"

write_step "Calculating SHA-1 hash..."
sha1=$(echo "$hash_message" | dotnet run --project "$CLI_PATH" -- run crypto.hash --param --algorithm sha1)
show_content "SHA-1" "$sha1" "$CYAN"

write_step "Calculating SHA-256 hash..."
sha256=$(echo "$hash_message" | dotnet run --project "$CLI_PATH" -- run crypto.hash --param --algorithm sha256)
show_content "SHA-256" "$sha256" "$CYAN"

write_step "Calculating SHA-512 hash..."
sha512=$(echo "$hash_message" | dotnet run --project "$CLI_PATH" -- run crypto.hash --param --algorithm sha512)
show_content "SHA-512" "$sha512" "$CYAN"

# Save all hashes
cat > "$OUTPUT_PATH/message-hashes.txt" << HASHES
Message: $hash_message

MD5:     $md5
SHA-1:   $sha1
SHA-256: $sha256
SHA-512: $sha512
HASHES
write_success "Saved to: output/message-hashes.txt"

pause_demo

# ============================================================
# PIPELINE DEMONSTRATION
# ============================================================
write_header "🔗 Pipeline Demonstration"

write_info "Demonstrating a real-world workflow pipeline:"
echo -e "  ${GRAY}1. Load API response (JSON)${NC}"
echo -e "  ${GRAY}2. Format it for readability${NC}"
echo -e "  ${GRAY}3. Extract user ID and generate similar UUIDs${NC}"
echo -e "  ${GRAY}4. Calculate hash of the formatted output\n${NC}"

pause_demo

write_step "Step 1: Load API response..."
api_response=$(cat "$DATA_PATH/api-response.json")
show_content "API Response (Raw)" "$api_response" "$YELLOW"

pause_demo

write_step "Step 2: Format JSON for readability..."
formatted=$(echo "$api_response" | dotnet run --project "$CLI_PATH" -- run json.formatter --param indentSize=)
show_content "Formatted Response" "$formatted" "$GREEN"

pause_demo

write_step "Step 3: Generate batch of UUIDs for user sessions..."
session_ids=$(echo "" | dotnet run --project "$CLI_PATH" -- run generation.uuid --param --count 3)
show_content "Generated Session IDs" "$session_ids" "$CYAN"

pause_demo

write_step "Step 4: Calculate integrity hash of the response..."
response_hash=$(echo "$formatted" | dotnet run --project "$CLI_PATH" -- run crypto.hash --param --algorithm sha256)
show_content "Response Hash (SHA-256)" "$response_hash" "$MAGENTA"

# Save pipeline results
cat > "$OUTPUT_PATH/pipeline-results.txt" << PIPELINE
=================================================
Pipeline Results - $(date "+%Y-%m-%d %H:%M:%S")
=================================================

FORMATTED API RESPONSE:
$formatted

GENERATED SESSION IDs:
$session_ids

INTEGRITY HASH (SHA-256):
$response_hash

=================================================
PIPELINE
write_success "Pipeline results saved to: output/pipeline-results.txt"

pause_demo

# ============================================================
# FINALE
# ============================================================
write_header "🎉 Demo Complete!"

write_success "All tools demonstrated successfully!\n"

write_info "Output files created:"
echo -e "  ${GRAY}• output/sample-formatted.json${NC}"
echo -e "  ${GRAY}• output/sample-from-yaml.json${NC}"
echo -e "  ${GRAY}• output/secret-encoded.txt${NC}"
echo -e "  ${GRAY}• output/generated-uuids.txt${NC}"
echo -e "  ${GRAY}• output/message-hashes.txt${NC}"
echo -e "  ${GRAY}• output/pipeline-results.txt${NC}"

echo -e "\n"
write_info "Next steps:"
echo -e "  ${GRAY}• Try the tools yourself: cd ../src/DevFoundry.Cli && dotnet run -- list${NC}"
echo -e "  ${GRAY}• View the API: cd ../src/DevFoundry.Api && dotnet run${NC}"
echo -e "  ${GRAY}• Open the web demo: demo/web-demo.html${NC}"

echo -e "\n"
echo -e "${CYAN}Thank you for exploring DevFoundry! 🛠️${NC}"
echo ""
