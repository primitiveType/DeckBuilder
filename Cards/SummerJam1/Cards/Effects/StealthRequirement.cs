using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class StealthRequirement : SummerJam1Component, IDescription
    {
        public int MaxStealth { get; set; } = 999;
        public int MinStealth { get; set; } = 0;

        public string Description
        {
            get
            {
                if (MaxStealth < 999 && MinStealth > 0)
                {
                    return $"Can't be played if Stealth is greater than {MinStealth} or less then {MaxStealth}";
                }

                if (MaxStealth < 99)
                {
                    return $"Can't be played if Stealth is greater than {MaxStealth}";
                }

                if (MinStealth > 0)
                {
                    return $"Can't be played if Stealth is lesz than {MinStealth}";
                }

                return $"Can't be played if Stealth is greater than {MinStealth} or less then {MaxStealth}";
            }
        }

        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            if (Game.Player.Entity.GetComponent<Stealth>().Amount < MinStealth)
            {
                args.Blockers.Add($"Stealth Must be Greater than {MinStealth}!");
            }

            if (Game.Player.Entity.GetComponent<Stealth>().Amount > MaxStealth)
            {
                args.Blockers.Add($"Stealth Must be Less than {MaxStealth}!");
            }
        }
    }
}
