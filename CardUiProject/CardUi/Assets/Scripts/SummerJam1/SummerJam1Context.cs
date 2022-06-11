using System.IO;
using Api;
using App.Utility;

namespace SummerJam1
{
    public class SummerJam1Context : MonoBehaviourSingleton<SummerJam1Context>
    {
        public Context Context { get; private set; }
        public SummerJam1Events Events => (SummerJam1Events)Context.Events;
        public SummerJam1Game Game { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            var events = new SummerJam1Events();
            Context = new Context(events);
            Context.SetPrefabsDirectory(Path.Combine("Assets", "External", "Library", "Prefabs"));
            IEntity game = Context.Root;
            Game = game.AddComponent<SummerJam1Game>();
        }

    }
}