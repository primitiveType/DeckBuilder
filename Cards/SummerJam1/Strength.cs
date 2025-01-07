using Api;
using CardsAndPiles;
using Newtonsoft.Json;

namespace SummerJam1
{
    public class Strength : SummerJam1Component, IAmount
    {
        [JsonProperty] public int Amount { get; set; }

        [OnRequestDamageModifiers]
        private void OnRequestDamageModifiers(object sender, RequestDamageModifiersEventArgs args)
        {
            if (args.Source == Entity)
            {
                args.Modifiers.Add(Amount);
            }
        }
    }
}