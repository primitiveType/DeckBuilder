namespace SummerJam1.Units
{
    public class BloodRequirement : SummerJam1Component //, IAmount, ITooltip
    {
        // [JsonProperty] public int Amount { get; set; }
        // public string Tooltip => $"This card requires a slot with at least {Amount} blood.";
        //
        // [OnRequestPlayCard]
        // private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        // {
        //     if (args.CardId != Entity)
        //     {
        //         return;
        //     }
        //     EncounterSlotPile slot = args.Target.GetComponentInSelfOrParent<EncounterSlotPile>();
        //
        //     if (slot == null || slot.Entity.Children.Count > 0)
        //     {
        //         args.Blockers.Add(CardBlockers.INVALID_TARGET);
        //     }
        //
        //     if (Amount <= 0)
        //     {
        //         return;
        //     }
        //
        //     Blood blood = slot.Entity.GetOrAddComponent<Blood>();
        //     if (blood.Amount < Amount)
        //     {
        //         args.Blockers.Add(CardBlockers.NOT_ENOUGH_BLOOD);
        //     }
        // }
    }
}
