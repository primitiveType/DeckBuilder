// See https://aka.ms/new-console-template for more information

using Api;
using SummerJam1;
//load all assets and re-save them. Cleans up missing components before publish.
Logging.Initialize(new DefaultLogger());
var context = new Context(new SummerJam1Events());
IEntity gameEntity = context.Root;
context.SetPrefabsDirectory("../../../../SummerJam1/StreamingAssets/Prefabs","../../../../SummerJam1/StreamingAssets/Resources" );
var game = gameEntity.AddComponent<Game>();

DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath));

TestDirectory(info);

void TestDirectory(DirectoryInfo dir)
{
    foreach (DirectoryInfo enumerateDirectory in dir.EnumerateDirectories())
    {
        if (enumerateDirectory.Name == "Battles")
        {
            return;//battle infos are not entities.
        }
        TestDirectory(enumerateDirectory);
    }

    foreach (FileInfo enumerateFile in dir.EnumerateFiles())
    {
        IEntity entity = context.CreateEntity(null, enumerateFile.FullName);
        entity.RemoveComponent<UnknownComponent>();
        var str = Serializer.SerializeWithoutIds(entity);
        File.WriteAllText(enumerateFile.FullName, str);
    }
}