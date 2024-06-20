using System.ComponentModel;
using JetBrains.Annotations;
using Component = Api.Component;

namespace SummerJam1
{
    public class SummerJam1Component : Component
    {
        protected new SummerJam1Events Events => (SummerJam1Events)base.Events;
        protected Game Game { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Game = Context.Root.GetComponent<Game>();
            PropertyChanged += OnEnabledChanged;
            if (Enabled)
            {
                OnEnable();
            }
        }

        private void OnEnabledChanged([CanBeNull] object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Enabled))
            {
                if (Enabled)
                {
                    OnEnable();
                }
                else
                {
                    OnDisable();
                }
            }
        }

        protected virtual void OnDisable()
        {
            
        }

        protected virtual void OnEnable()
        {
            
        }
    }
}
