dotnet pack -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet pack failed"
    exit $LASTEXITCODE
}

dotnet tool uninstall --global AsciiArt
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet tool uninstall failed"
}

dotnet tool install --global --add-source ./bin/Release AsciiArt --framework net9.0
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet tool install failed"
    exit $LASTEXITCODE
}