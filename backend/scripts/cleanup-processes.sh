#!/usr/bin/env bash
# cleanup-processes.sh
# Kills all DevFoundry processes to unlock DLL files for builds

echo "🧹 Cleaning up DevFoundry processes..."

# Find all DevFoundry processes
PROCESSES=$(ps aux | grep -i "[D]evFoundry" | awk '{print $2}')

if [ -n "$PROCESSES" ]; then
    echo "Found DevFoundry process(es):"
    ps aux | grep -i "[D]evFoundry"

    # Kill all processes
    echo "$PROCESSES" | xargs kill -9 2>/dev/null

    echo "✅ All DevFoundry processes terminated."

    # Wait a moment for file locks to release
    sleep 1

    echo "✅ File locks released. You can now run 'dotnet build'."
else
    echo "✅ No DevFoundry processes found running."
fi
