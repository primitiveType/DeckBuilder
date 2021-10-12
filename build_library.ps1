#!/bin/pwsh
dotnet clean $(Join-Path "Library" "DeckbuilderLibrary")
dotnet build $(Join-Path "Library" "DeckbuilderLibrary")
$dependencies = [IO.PATH]::Combine("App", "Assets", "Dependencies")
if (!(Test-Path $dependencies) ){mkdir $dependencies}
cp $([IO.PATH]::Combine("Library", "DeckbuilderLibrary", "DeckbuilderLibrary", "bin", "Debug", "netstandard2.0", "DeckbuilderLibrary.dll")) $dependencies
cp $([IO.PATH]::Combine("Library", "DeckbuilderLibrary", "DeckbuilderLibrary", "bin", "Debug", "netstandard2.0", "DeckbuilderLibrary.pdb")) $dependencies
cp $([IO.PATH]::Combine("Library", "DeckbuilderLibrary", "DeckbuilderLibrary", "bin", "Debug", "netstandard2.0", "JsonNet.ContractResolvers.dll")) $dependencies
cp $([IO.PATH]::Combine("Library", "DeckbuilderLibrary", "Content", "bin", "Debug", "netstandard2.0", "Content.dll")) $dependencies
cp $([IO.PATH]::Combine("Library", "DeckbuilderLibrary", "Content", "bin", "Debug", "netstandard2.0", "Content.pdb")) $dependencies
cp $([IO.PATH]::Combine("Library", "DeckbuilderLibrary", "DeckbuilderLibrary", "bin", "Debug", "netstandard2.0", "HexGrid.dll")) $dependencies
