﻿Param ($Version = "1.0.0-pre")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

# Ensure 0install is in PATH
if (!(Get-Command 0install -ErrorAction SilentlyContinue)) {
    $env:PATH = "$(Resolve-Path ..\artifacts\Release);$env:PATH"
}

# Exclude .NET XML Documentation and Debug Symbols from release
rm -Force ..\artifacts\Release\*.xml,..\artifacts\Release\*.pdb -Exclude *.VisualElementsManifest.xml

# Inspect version number
$stability = if($Version.Contains("-")) {"developer"} else {"stable"}

# Build feed and archive
cmd /c "0install run --batch http://0install.net/tools/0template.xml 0install-win.xml.template version=$Version stability=$stability 2>&1" # Redirect stderr to stdout
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}

# Patch archive URL for release builds
if ($stability -eq "stable") {
    $path = Resolve-Path "0install-win-$Version.xml"
    [xml]$xml = Get-Content $path
    $xml.interface.group.implementation.archive.href = "https://github.com/0install/0install-win/releases/download/$Version/$($xml.interface.group.implementation.archive.href)"
    $xml.Save($path)
}

popd
