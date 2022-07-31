using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Numerics;
using Api;
using CardsAndPiles.Components;
using RogueSharp;
using RogueSharp.MapCreation;

namespace RogueMaps
{
    public class DebugMapComponent : Component
    {
        public Map Map { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Map = RogueSharp.Map.Create(new BorderOnlyMapCreationStrategy<Map>(17, 10));
            var customMap = Entity.AddComponent<CustomMap>();
            customMap.Initialize(Map.Width, Map.Height);
            foreach (Cell cell in Map.GetAllCells())
            {
                Context.CreateEntity(Entity, entity =>
                {
                    CustomCell newCell = entity.AddComponent<CustomCell>();
                    newCell.X = cell.X;
                    newCell.Y = cell.Y;
                    newCell.IsWalkable = cell.IsWalkable;
                    newCell.IsTransparent = cell.IsTransparent;
                    customMap[newCell.X, newCell.Y] = newCell;
                });
            }

            Entity.RemoveComponent(this);
        }
    }

    public class MapCreatorComponent : Component
    {
        public Map Map { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Map = Map.Create(new RandomRoomsMapCreationStrategy<Map>(17, 10, 30, 5, 3));
            var customMap = Entity.AddComponent<CustomMap>();
            customMap.Initialize(Map.Width, Map.Height);
            foreach (Cell cell in Map.GetAllCells())
            {
                Context.CreateEntity(Entity, entity =>
                {
                    CustomCell newCell = entity.AddComponent<CustomCell>();
                    newCell.X = cell.X;
                    newCell.Y = cell.Y;
                    newCell.IsWalkable = cell.IsWalkable;
                    newCell.IsTransparent = cell.IsTransparent;
                    customMap[newCell.X, newCell.Y] = newCell;
                });
            }

            Entity.RemoveComponent(this);
        }
    }

    public class CustomMap : Component, IMap<CustomCell>
    {
        private Map<CustomCell> test;
        private CustomCell[,] Cells { get; set; }

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new CustomCell[width, height];
        }

        public bool IsTransparent(int x, int y)
        {
            return true;
        }

        public bool IsWalkable(int x, int y)
        {
            return Cells[x, y].IsWalkable;
        }

        public void SetCellProperties(int x, int y, bool isTransparent, bool isWalkable)
        {
            Cells[x, y].IsWalkable = isWalkable;
            Cells[x, y].IsTransparent = isTransparent;
        }

        public void Clear()
        {
            Clear(true, true);
        }

        public void Clear(bool isTransparent, bool isWalkable)
        {
            foreach (CustomCell customCell in Cells)
            {
                customCell.IsWalkable = isWalkable;
                customCell.IsTransparent = isTransparent;
            }
        }

        public TMap Clone<TMap>() where TMap : IMap<CustomCell>, new()
        {
            throw new NotImplementedException();
        }

        public void Copy(IMap<CustomCell> sourceMap)
        {
            throw new NotImplementedException();
        }

        public void Copy(IMap<CustomCell> sourceMap, int left, int top)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetAllCells()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    yield return GetCell(x, y);
                }
            }
        }

        public IEnumerable<CustomCell> GetCellsAlongLine(int xOrigin, int yOrigin, int xDestination, int yDestination)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetCellsInCircle(int xCenter, int yCenter, int radius)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetCellsInDiamond(int xCenter, int yCenter, int distance)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetCellsInSquare(int xCenter, int yCenter, int distance)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetCellsInRectangle(int top, int left, int width, int height)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetBorderCellsInCircle(int xCenter, int yCenter, int radius)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetBorderCellsInDiamond(int xCenter, int yCenter, int distance)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetBorderCellsInSquare(int xCenter, int yCenter, int distance)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetCellsInRows(params int[] rowNumbers)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetCellsInColumns(params int[] columnNumbers)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomCell> GetAdjacentCells(int xCenter, int yCenter) => GetAdjacentCells(xCenter, yCenter, false);

        public IEnumerable<CustomCell> GetAdjacentCells(int xCenter, int yCenter, bool includeDiagonals)
        {
            int topY = yCenter - 1;
            int bottomY = yCenter + 1;
            int leftX = xCenter - 1;
            int rightX = xCenter + 1;
            if (topY >= 0)
                yield return this.GetCell(xCenter, topY);
            if (leftX >= 0)
                yield return this.GetCell(leftX, yCenter);
            if (bottomY < this.Height)
                yield return this.GetCell(xCenter, bottomY);
            if (rightX < this.Width)
                yield return this.GetCell(rightX, yCenter);
            if (includeDiagonals)
            {
                if (rightX < this.Width && topY >= 0)
                    yield return this.GetCell(rightX, topY);
                if (rightX < this.Width && bottomY < this.Height)
                    yield return this.GetCell(rightX, bottomY);
                if (leftX >= 0 && topY >= 0)
                    yield return this.GetCell(leftX, topY);
                if (leftX >= 0 && bottomY < this.Height)
                    yield return this.GetCell(leftX, bottomY);
            }
        }

        public CustomCell GetCell(int x, int y)
        {
            return Cells[x, y];
        }

        public MapState Save()
        {
            throw new NotImplementedException();
        }

        public void Restore(MapState state)
        {
            throw new NotImplementedException();
        }

        public CustomCell CellFor(int index)
        {
            throw new NotImplementedException();
        }

        public int IndexFor(int x, int y)
        {
            throw new NotImplementedException();
        }

        public int IndexFor(CustomCell cell)
        {
            throw new NotImplementedException();
        }

        public CustomCell this[int x, int y]
        {
            get => Cells[x, y];
            set => Cells[x, y] = value;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
    }

    public class CustomCell : Component, ICell, IParentConstraint, IVisual, IPosition
    {
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
    }

    public class BlocksMovement : Component, IBlocksMovement, IVisual
    {
    }

    public interface IBlocksMovement : IComponent

    {
    }
}
