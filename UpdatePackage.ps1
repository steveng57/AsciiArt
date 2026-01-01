dotnet pack -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet pack failed"
    exit $LASTEXITCODE
}

# Uninstall the previously installed tool package id (legacy)
dotnet tool uninstall --global asciiart 2>$null

# Uninstall the current tool package id (new)
dotnet tool uninstall --global steveng57.asciiart 2>$null

dotnet tool install --global --add-source ./bin/Release steveng57.asciiart
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet tool install failed"
    exit $LASTEXITCODE
}