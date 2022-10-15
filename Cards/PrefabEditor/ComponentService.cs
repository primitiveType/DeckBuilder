using Api;
using CardsAndPiles.Components;
using SummerJam1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using IComponent = Api.IComponent;

namespace PrefabEditor
{
    public class ComponentService  : INotifyPropertyChanged
    {
        private string PrefabsPath { get; set; } = "Assets/StreamingAssets";
        private Context Context { get; set; }
        private Game Game { get; set; }

        private IEntity EditRoot { get; set; }
        public IEntity CurrentEntity { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyList<Type> ComponentTypes => AllComponentTypes;
        private List<Type> AllComponentTypes = new List<Type>();

        public ComponentService()
        {
        
        }

        public void Start(string prefabsPath)
        {
            PrefabsPath = prefabsPath;
            Logging.Initialize(new DefaultLogger());
            CreateContext();
            EditRoot = Context.CreateEntity(Context.Root);

            AllComponentTypes.Clear();
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in a.GetTypes())
                {
                    if (typeof(IComponent).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface && !type.IsGenericType)
                    {
                        AllComponentTypes.Add(type);
                    }
                }
            }
        }

        private void CreateContext()
        {
            SummerJam1Events events = new SummerJam1Events();
            Context = new Context(events);

            Context.SetPrefabsDirectory(PrefabsPath);
            IEntity game = Context.Root;


            Game = game.AddComponent<Game>();
        }

        public List<string> GetFilelist()
        {
            List<string> files = new List<string>();
            DirectoryInfo di = new DirectoryInfo(PrefabsPath);
            foreach (FileInfo fileInfo in di.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                if (fileInfo.Extension != ".json")
                {
                    continue;
                }
                files.Add(fileInfo.FullName.Replace(PrefabsPath, "").TrimStart('\\'));
            }

            return files;
        }

        public void LoadPrefab(string fileName)
        {
            Logging.Log($"Loading file {fileName}.");

            CurrentEntity = Context.CreateEntity(null, fileName);
            CurrentEntity.GetOrAddComponent<SourcePrefab>().Prefab = fileName;
            Logging.Log($"Loaded {CurrentEntity.GetComponent<NameComponent>()?.Value}.");
        }

  

        internal void SaveCurrentPrefab()
        {
            var source = CurrentEntity.GetComponent<SourcePrefab>();
            var json = Serializer.Serialize(CurrentEntity);
            File.WriteAllText(Path.Combine(Context.PrefabsPath, source.Prefab), json);
        }
    }

}
