using System.Collections.Specialized;
using Api;
using CardsAndPiles;
using NUnit.Framework;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using Tile = Wunderwunsch.HexMapLibrary.Tile;

namespace Tests
{
    public class Tester
    {
        public Tester()
        {
            
        }
    }
    public class Tests
    {
        private Context Context { get; set; }

        public HexMapExtended Game { get; set; }
        public int Size => 4;

        [SetUp]
        public void Setup()
        {
            Context = new Context(new CardEvents());
            IEntity gameEntity = Context.Root;
            Context.SetPrefabsDirectory("StreamingAssets");

            Game = gameEntity.AddComponent<HexMapExtended>();
            Game.Setup(HexMapBuilder.CreateHexagonalShapedMap(Size));
        }


        [Test]
        public void Test1()
        {
            Tile originTile = Game.TilesByPosition[new Vector3Int(0,1,-1)] ;
            Assert.NotNull(originTile);
            originTile.Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;

            bool hadChange = false;

            void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                hadChange = true;
            }

            var entity = Context.CreateEntity();
            entity.TrySetParent(originTile.Entity);


            Assert.IsTrue(hadChange);
            
            Assert.Greater(Game.Entity.Children.Count, 100);
        }
        
    }
}
