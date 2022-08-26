using System.Collections.Specialized;
using System.Numerics;
using Api;
using CardsAndPiles.Components;
using RogueSharp;

namespace RogueMaps
{
    public class CustomCell : Component, ICell, IParentConstraint, IVisual, IPosition
    {
        public bool Equals(ICell other)
        {
            return other?.X == X && other.Y == Y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public bool IsTransparent { get; set; }

        public bool IsWalkable
        {
            get => Entity.GetComponentInChildren<IBlocksMovement>() == null;
            set
            {
                if (!value)
                {
                    Context.CreateEntity(Entity, entity =>
                    {
                        entity.AddComponent<BlocksMovement>();
                        entity.AddComponent<Position>();
                    });
                }
                else
                {
                    foreach (IBlocksMovement componentsInChild in Entity.GetComponentsInChildren<IBlocksMovement>())
                    {
                        componentsInChild.Entity.Destroy();
                    }
                }
            }
        }

        public bool AcceptsParent(IEntity parent)
        {
            return true;
        }

        public bool AcceptsChild(IEntity child)
        {
            return IsWalkable;
        }

        public Vector3 Pos
        {
            get => new Vector3(X, Y, 0);
            set
            {
                X = (int)value.X;
                Y = (int)value.Y;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntity eNewItem in e.NewItems)
                {
                    eNewItem.GetOrAddComponent<Position>().Pos = new Vector3(X, Y, 0);
                }
            }
        }


        public override void Terminate()
        {
            base.Terminate();
            Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }
    }
}
