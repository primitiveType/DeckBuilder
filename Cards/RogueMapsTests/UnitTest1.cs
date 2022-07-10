using System;
using Api;
using CardsAndPiles;
using NUnit.Framework;
using RogueMaps;

namespace Tests
{
    public class Tests
    {
        private Context Context;
        [SetUp]
        public void Setup()
        {
            Context = new Context(new CardEvents());
        }

        [Test]
        public void Test1()
        {
            var map = Context.Root.AddComponent<MapComponent>();
            
            Console.WriteLine(map.Map.ToString());
        }
    }
}
