dotnet clean "Library\DeckbuilderLibrary\" 
dotnet build "Library\DeckbuilderLibrary\" 
$dependencies = "App\Assets\Dependencies\"
if (!(Test-Path $dependencies) ){mkdir $dependencies}
cp "Library\DeckbuilderLibrary\DeckbuilderLibrary\bin\Debug\netstandard2.0\DeckbuilderLibrary.dll"  $dependencies
cp "Library\DeckbuilderLibrary\DeckbuilderLibrary\bin\Debug\netstandard2.0\DeckbuilderLibrary.pdb" $dependencies
cp "Library\DeckbuilderLibrary\Content\bin\Debug\netstandard2.0\Content.dll" $dependencies
cp "Library\DeckbuilderLibrary\Content\bin\Debug\netstandard2.0\Content.pdb" $dependencies