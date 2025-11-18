#!/usr/bin/env pwsh
# cleanup-processes.ps1
# Kills all DevFoundry processes to unlock DLL files for builds

Write-Host "🧹 Cleaning up DevFoundry processes..." -ForegroundColor Cyan

# Find all DevFoundry processes
$processes = Get-Process | Where-Object {$_.ProcessName -like "*DevFoundry*"}

if ($processes) {
    Write-Host "Found $($processes.Count) DevFoundry process(es):" -ForegroundColor Yellow
    $processes | Format-Table Id, ProcessName, StartTime -AutoSize

    # Kill all processes
    $processes | Stop-Process -Force

    Write-Host "✅ All DevFoundry processes terminated." -ForegroundColor Green

    # Wait a moment for file locks to release
    Start-Sleep -Seconds 1

    Write-Host "✅ File locks released. You can now run 'dotnet build'." -ForegroundColor Green
} else {
    Write-Host "✅ No DevFoundry processes found running." -ForegroundColor Green
}
