using System;
using Api;
using CardsAndPiles;

namespace SummerJam1Tests
{
    public class TestEvents : Component
    {
        public Action ToDo { get; set; }

        [OnTurnEnded]
        private void OnTurnEnded()
        {
            ToDo.Invoke();
        }
    }
}
