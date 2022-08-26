using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
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
        public void CellComponentsCreated()
        {
            MapCreatorComponent mapCreatorCreator = Context.Root.AddComponent<MapCreatorComponent>();

            CustomMap map = Context.Root.GetComponent<CustomMap>();
            Assert.NotNull(map);

            Assert.AreEqual(map.Entity.Children.Count, map.Height * map.Width);

            CustomCell walkableCell = map.GetAllCells().First(cell => cell.IsWalkable);

            IEntity player = Context.CreateEntity(walkableCell.Entity, entity => { entity.AddComponent<Position>(); });

            Assert.That(player.Parent, Is.EqualTo(walkableCell.Entity));
            Assert.That(player.GetComponent<Position>().Pos.X, Is.EqualTo(walkableCell.X));
            Assert.That(player.GetComponent<Position>().Pos.Z, Is.EqualTo(walkableCell.Y));
        }
    }
}