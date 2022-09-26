using System.ComponentModel;
using SummerJam1.Cards;

namespace SummerJam1.Statuses
{
    public class IsTopMonster : EnabledWhenAtTopOfEncounterSlot
    {
        protected override void Initialize()
        {
            base.Initialize();
            PropertyChanged += EntityOnPropertyChanged;
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is not (nameof(EnableOnTurnStart) or nameof(Enabled)))
            {
                return;
            }

            if (Enabled || EnableOnTurnStart)
            {
                Entity.RemoveComponent<FaceDown>();
            }
            else 
            {
                Entity.GetOrAddComponent<FaceDown>();
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            PropertyChanged -= EntityOnPropertyChanged;
        }
    }
}
