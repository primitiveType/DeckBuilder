// See https://aka.ms/new-console-template for more information

using Api;
using SummerJam1;

Logging.Initialize(new DefaultLogger());
var context = new Context(new SummerJam1Events());
IEntity gameEntity = context.Root;
context.SetPrefabsDirectory("../../../../SummerJam1/StreamingAssets");
var game = gameEntity.AddComponent<Game>();

DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath));

TestDirectory(info);

void TestDirectory(DirectoryInfo dir)
{
    foreach (DirectoryInfo enumerateDirectory in dir.EnumerateDirectories())
    {
        Console.WriteLine($"Checking dir {enumerateDirectory}");

        TestDirectory(enumerateDirectory);
    }

    foreach (FileInfo enumerateFile in dir.EnumerateFiles())
    {
        Console.WriteLine($"Creating entity {enumerateFile.Name}");
        IEntity entity = context.CreateEntity(null, enumerateFile.FullName);
        entity.RemoveComponent<UnknownComponent>();
        var str = Serializer.Serialize(entity);
        File.WriteAllText(enumerateFile.FullName, str);
    }
}