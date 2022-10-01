using Api;
using CardsAndPiles.Components;
using SummerJam1;
using System;
using System.IO;
using System.Reflection;

namespace PrefabEditor
{
    public class ComponentService 
    {
        public string m_PrefabsPath = "Assets/StreamingAssets";
        private Context Context { get; set; }
        private Game Game { get; set; }

        private IEntity EditRoot { get; set; }
        public IEntity CurrentEntity { get; set; }
        
        public void Start()
        {
            Logging.Initialize(new DefaultLogger());
            CreateContext();
            EditRoot = Context.CreateEntity(Context.Root);
            RefreshFileList();
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in a.GetTypes())
                {
                    if (typeof(IComponent).IsAssignableFrom(type))
                    {
                        Logging.Log(type.FullName);
                    }
                }
            }
        }

        private void CreateContext()
        {
            SummerJam1Events events = new SummerJam1Events();
            Context = new Context(events);

            Context.SetPrefabsDirectory(m_PrefabsPath);
            IEntity game = Context.Root;


            Game = game.AddComponent<Game>();
        }

        private void RefreshFileList()
        {
            DirectoryInfo di = new DirectoryInfo(m_PrefabsPath);
            foreach (FileInfo fileInfo in di.GetFiles())
            {
                if (fileInfo.Extension != ".json")
                {
                    continue;
                }
              //do forms stuff.
            }
        }

        private void LoadPrefab(string fileName)
        {
            Logging.Log($"Loading file {fileName}.");

            CurrentEntity = Context.CreateEntity(EditRoot, fileName);

            Logging.Log($"Loaded {CurrentEntity.GetComponent<NameComponent>().Value}.");
        }

        // Update is called once per frame
        void Update()
        {
        }
    }

}
