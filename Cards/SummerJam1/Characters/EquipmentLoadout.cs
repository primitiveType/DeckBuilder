using System.Collections.Generic;
using System.Linq;
using Api;
using Newtonsoft.Json;

namespace SummerJam1.Characters
{
    public class EquipmentLoadout : SummerJam1Component
    {
        [JsonProperty] private Dictionary<EquipmentSlot, int> EquippedItemIds { get; set; } = new();

        public IReadOnlyDictionary<EquipmentSlot, int> EquippedItems => EquippedItemIds;

        public bool Equip(IEntity item)
        {
            Equipment equipment = item.GetComponent<Equipment>();
            if (equipment == null)
            {
                return false;
            }

            EquippedItemIds[equipment.Slot] = item.Id;
            return true;
        }

        public void Unequip(EquipmentSlot slot)
        {
            EquippedItemIds.Remove(slot);
        }

        public IEnumerable<string> GetContributedCardPrefabs()
        {
            foreach (int itemId in EquippedItemIds.Values)
            {
                if (!Context.EntityDatabase.TryGetValue(itemId, out IEntity item))
                {
                    continue;
                }

                Equipment equipment = item.GetComponent<Equipment>();
                if (equipment == null)
                {
                    continue;
                }

                foreach (string cardPrefab in equipment.CardPrefabs.Where(cardPrefab => !string.IsNullOrWhiteSpace(cardPrefab)))
                {
                    yield return cardPrefab;
                }
            }
        }
    }
}
