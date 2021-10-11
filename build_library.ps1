#!/bin/pwsh
dotnet clean $(Join-Path "Library" "DeckbuilderLibrary")
dotnet build $(Join-Path "Library" "DeckbuilderLibrary")
$dependencies = $(Join-Path "App" "Assets" "Dependencies")
if (!(Test-Path $dependencies) ){mkdir $dependencies}
cp $(Join-Path "Library" "DeckbuilderLibrary" "DeckbuilderLibrary" "bin" "Debug" "netstandard2.0" "DeckbuilderLibrary.dll") $dependencies
cp $(Join-Path "Library" "DeckbuilderLibrary" "DeckbuilderLibrary" "bin" "Debug" "netstandard2.0" "DeckbuilderLibrary.pdb") $dependencies
cp $(Join-Path "Library" "DeckbuilderLibrary" "DeckbuilderLibrary" "bin" "Debug" "netstandard2.0" "JsonNet.ContractResolvers.dll") $dependencies
cp $(Join-Path "Library" "DeckbuilderLibrary" "Content" "bin" "Debug" "netstandard2.0" "Content.dll") $dependencies
cp $(Join-Path "Library" "DeckbuilderLibrary" "Content" "bin" "Debug" "netstandard2.0" "Content.pdb") $dependencies
cp $(Join-Path "Library" "DeckbuilderLibrary" "DeckbuilderLibrary" "bin" "Debug" "netstandard2.0" "HexGrid.dll") $dependencies
