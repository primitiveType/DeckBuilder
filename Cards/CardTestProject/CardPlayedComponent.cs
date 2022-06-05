﻿using CardsAndPiles;
using Component = Api.Component;

namespace CardTestProject
{
    public class CardPlayedComponent : Component
    {
        public bool CardPlayed { get; private set; }

        [OnCardPlayed]
        private void OnCardPlayed()
        {
            CardPlayed = true;
        }
    }
}