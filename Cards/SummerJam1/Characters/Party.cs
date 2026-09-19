using System;
using System.Collections.Generic;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards;

namespace SummerJam1.Characters
{
    public class Party : SummerJam1Component
    {
        [JsonProperty] public int MaxPartySize { get; set; } = 4;

        public List<IEntity> ActiveMembers =>
            Entity.GetComponentsInChildren<PartyMember>()
                .Where(member => member.IsActive)
                .Select(member => member.Entity)
                .ToList();

        public bool TryAddMember(IEntity member)
        {
            if (ActiveMembers.Count >= MaxPartySize)
            {
                return false;
            }

            member.GetOrAddComponent<PartyMember>();
            member.GetOrAddComponent<EquipmentLoadout>();
            return member.TrySetParent(Entity);
        }

        public IEntity HireCharacter(string prefab)
        {
            if (ActiveMembers.Count >= MaxPartySize)
            {
                return null;
            }

            IEntity member = Context.CreateEntity(Entity, prefab);
            member.GetOrAddComponent<PartyMember>();
            member.GetOrAddComponent<EquipmentLoadout>();
            return member;
        }

        public bool SetActive(IEntity member, bool isActive)
        {
            PartyMember partyMember = member.GetComponent<PartyMember>();
            if (partyMember == null)
            {
                return false;
            }

            if (isActive && !partyMember.IsActive && ActiveMembers.Count >= MaxPartySize)
            {
                return false;
            }

            partyMember.IsActive = isActive;
            return true;
        }

        public IEntity GetMemberById(int ownerId)
        {
            if (ownerId < 0)
            {
                return null;
            }

            return ActiveMembers.FirstOrDefault(member => member.Id == ownerId);
        }

        public IEntity GetCardOwner(IEntity card)
        {
            CardOwner owner = card.GetComponent<CardOwner>();
            return owner == null ? null : GetMemberById(owner.OwnerId);
        }

        public void RebuildDeck()
        {
            foreach (IEntity card in Game.Deck.Entity.Children.ToList())
            {
                card.Destroy();
            }

            foreach (IEntity member in ActiveMembers)
            {
                AddCardsForMember(member);
            }
        }

        public void AddCardsForMember(IEntity member)
        {
            foreach (string cardPrefab in GetCardPrefabsForMember(member))
            {
                AddOwnedCardToDeck(member, cardPrefab);
            }
        }

        public IEntity AddOwnedCardToDeck(IEntity member, string cardPrefab)
        {
            IEntity card = Context.CreateEntity(Game.Deck.Entity, cardPrefab);
            card.GetOrAddComponent<CardOwner>().OwnerId = member.Id;
            return card;
        }

        public IEnumerable<string> GetCardPrefabsForMember(IEntity member)
        {
            foreach (string starter in GetStartingCardPrefabs(member))
            {
                yield return starter;
            }

            EquipmentLoadout loadout = member.GetComponent<EquipmentLoadout>();
            if (loadout == null)
            {
                yield break;
            }

            foreach (string equipmentCard in loadout.GetContributedCardPrefabs())
            {
                yield return equipmentCard;
            }
        }

        private IEnumerable<string> GetStartingCardPrefabs(IEntity member)
        {
            string character = member.GetComponent<ICharacterClass>()?.Name ?? "Any";
            foreach (StartingCard starterCard in Game.PrefabsContainer.GetComponentsInChildren<StartingCard>())
            {
                CharacterConstraint constraint = starterCard.Entity.GetComponent<CharacterConstraint>();
                if (constraint is { Character: not "Any" } && !string.Equals(constraint.Character, character, StringComparison.Ordinal))
                {
                    continue;
                }

                for (int i = 0; i < starterCard.Amount; i++)
                {
                    yield return starterCard.Entity.GetComponent<SourcePrefab>().Prefab;
                }
            }
        }
    }
}
