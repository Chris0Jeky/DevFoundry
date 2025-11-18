#!/usr/bin/env pwsh
# Simple test script

$OutputPath = "./output"
$DataPath = "./data"

# Test 1: Simple path
Write-Host "Test 1: Simple path"
$test1 = "Hello World"
$test1 | Out-File (Join-Path $OutputPath "test1.txt") -Encoding utf8
Write-Host "Success 1"

# Test 2: Heredoc
Write-Host "Test 2: Heredoc"
$test2 = @"
Line 1
Line 2
"@
$test2 | Out-File (Join-Path $OutputPath "test2.txt") -Encoding utf8
Write-Host "Success 2"

# Test 3: Final message
Write-Host "Test 3: Final message"
Write-Host "Thank you for testing!" -ForegroundColor Cyan
Write-Host "Done"
