function Test-FileLock {
    param([string]$FilePath)
    try {
        $file = [System.IO.File]::Open($FilePath, 'Open', 'ReadWrite', 'None')
        $file.Close()
        return $false
    }
    catch {
        return $true
    }
}

Write-Host "Building InterPlanetaryLauncherAutomation mod..." -ForegroundColor Cyan

# Build the solution in Release mode (script runs from ONIMod folder)
& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" "InterPlanetaryLauncherAutomation.sln" /p:Configuration=Release /nologo /v:minimal

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Exiting..." -ForegroundColor Red
    exit 1
}

Write-Host "Build successful! Deploying mod..." -ForegroundColor Cyan

# Define the mod directory
$modDir = "$env:USERPROFILE\Documents\Klei\OxygenNotIncluded\mods\Dev\InterPlanetaryLauncherAutomationMod"

# Create the mod directory if it doesn't exist
New-Item -ItemType Directory -Force -Path $modDir | Out-Null

# Check if target DLL is locked (game might be running)
$targetDll = Join-Path $modDir "InterPlanetaryLauncherAutomationMod.dll"
if (Test-Path $targetDll) {
    if (Test-FileLock $targetDll) {
        Write-Host "`nWARNING: The mod DLL is currently locked!" -ForegroundColor Yellow
        Write-Host "This usually means Oxygen Not Included is running and has the mod loaded." -ForegroundColor Yellow
        Write-Host "Please close the game and try again, or restart the game to load the new version." -ForegroundColor Yellow
        Write-Host "`nAttempting to copy anyway..." -ForegroundColor Cyan
    }
}

# Copy required files with error handling
$copySuccess = $true
try {
    Copy-Item "bin\Release\InterPlanetaryLauncherAutomationMod.dll" -Destination $modDir -Force -ErrorAction Stop
    Write-Host "[OK] DLL copied successfully" -ForegroundColor Green
} catch {
    Write-Host "[FAIL] Failed to copy DLL: $_" -ForegroundColor Red
    Write-Host "  Make sure Oxygen Not Included is closed, then try again." -ForegroundColor Yellow
    $copySuccess = $false
}

try {
    Copy-Item "mod.yaml" -Destination $modDir -Force -ErrorAction Stop
    Write-Host "[OK] mod.yaml copied successfully" -ForegroundColor Green
} catch {
    Write-Host "[FAIL] Failed to copy mod.yaml: $_" -ForegroundColor Red
    $copySuccess = $false
}

try {
    Copy-Item "mod_info.yaml" -Destination $modDir -Force -ErrorAction Stop
    Write-Host "[OK] mod_info.yaml copied successfully" -ForegroundColor Green
} catch {
    Write-Host "[FAIL] Failed to copy mod_info.yaml: $_" -ForegroundColor Red
    $copySuccess = $false
}

if ($copySuccess) {
    Write-Host "`nBuild and deployment complete!" -ForegroundColor Green
    Write-Host "Mod copied to: $modDir" -ForegroundColor Green
    Write-Host "`nYou can now test the mod in Oxygen Not Included!" -ForegroundColor Yellow
} else {
    Write-Host "`nDeployment completed with errors. Please check the messages above." -ForegroundColor Yellow
    exit 1
}

