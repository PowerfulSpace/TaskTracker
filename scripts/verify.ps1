Set-Location -LiteralPath (Join-Path $PSScriptRoot "..")

dotnet build src/Backend/PS.TaskTracker/PS.TaskTracker.slnx --nologo
if ($LASTEXITCODE -ne 0) {
    Write-Host "BUILD FAILED"
    exit $LASTEXITCODE
}

dotnet test src/Backend/PS.TaskTracker/PS.TaskTracker.slnx --nologo
if ($LASTEXITCODE -ne 0) {
    Write-Host "TESTS FAILED"
    exit $LASTEXITCODE
}

Write-Host "VERIFY OK"
exit 0
