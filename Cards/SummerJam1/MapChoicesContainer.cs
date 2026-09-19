using System.ComponentModel;

namespace SummerJam1
{
    public class MapChoicesContainer : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            Game.PropertyChanged += GameOnPropertyChanged;
            PopulateMapChoices();
        }

        private void GameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Game.CurrentLevel))
            {
                PopulateMapChoices();
            }
        }

        private void PopulateMapChoices()
        {
            Entity.Children.DestroyRecursive();

            Context.CreateEntity<BattleChoice>(Entity);
            Context.CreateEntity<ShopChoice>(Entity);
        }
    }
}