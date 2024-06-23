using Api;
using CardsAndPiles.Components;
using SummerJam1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows.Forms;
using IComponent = Api.IComponent;

namespace PrefabEditor
{
    public class ComponentService  : INotifyPropertyChanged
    {
        private string PrefabsPath { get; set; } = "Assets/StreamingAssets";
        private Context Context { get; set; }
        private Game Game { get; set; }

        private IEntity EditRoot { get; set; }
        public List<IEntity> CurrentEntity { get; set; } = new List<IEntity>();
        public bool MultiMode => CurrentEntity.Count > 1;

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

        public void LoadPrefab(ListBox.SelectedObjectCollection fileName)
        {
            Logging.Log($"Loading file {fileName}.");

            CurrentEntity.Clear();
            foreach (var entityPrefab in fileName)
            {
                var tempEntity = Context.CreateEntity(null, entityPrefab.ToString());
                CurrentEntity.Add(tempEntity);
                tempEntity.GetOrAddComponent<SourcePrefab>().Prefab = entityPrefab.ToString();
                Logging.Log($"Loaded {tempEntity.GetComponent<NameComponent>()?.Value}.");
            }
        
        }

  

        internal void SaveCurrentPrefab()
        {
            foreach (var entity in CurrentEntity)
            {
                var source = entity.GetComponent<SourcePrefab>();
                var json = Serializer.Serialize(entity);
                string path = Path.Combine(Context.PrefabsPath, source.Prefab);
                var info = new FileInfo(path);
                if (!Directory.Exists(info.Directory.FullName))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllText(path, json);
            }
            
        }
    }

}
