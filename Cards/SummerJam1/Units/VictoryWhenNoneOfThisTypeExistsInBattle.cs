using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SummerJam1.Units
{
    public class VictoryWhenNoneOfThisTypeExistsInBattle : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
            {
                CheckForWinCondition();
            }
        }

        private void CheckForWinCondition()
        {
            List<VictoryWhenNoneOfThisTypeExistsInBattle> these =
                Game.Battle.Entity.GetComponentsInChildren<VictoryWhenNoneOfThisTypeExistsInBattle>();
            if (these.Any(conditional => conditional.Entity.GetComponentInParent<IPlayerControl>() == null))
            {
                return; //if any of them are not in our control, return;
            }

            Events.OnBattleEnded(new BattleEndedEventArgs(true));
        }
    }
}