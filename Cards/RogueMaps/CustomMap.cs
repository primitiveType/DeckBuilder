using System;
using System.Collections.Generic;
using Api;
using RogueSharp;

namespace RogueMaps
{
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

        public IEnumerable<CustomCell> GetAdjacentCells(int xCenter, int yCenter)
        {
            return GetAdjacentCells(xCenter, yCenter, false);
        }

        public IEnumerable<CustomCell> GetAdjacentCells(int xCenter, int yCenter, bool includeDiagonals)
        {
            int topY = yCenter - 1;
            int bottomY = yCenter + 1;
            int leftX = xCenter - 1;
            int rightX = xCenter + 1;
            if (topY >= 0)
            {
                yield return GetCell(xCenter, topY);
            }

            if (leftX >= 0)
            {
                yield return GetCell(leftX, yCenter);
            }

            if (bottomY < Height)
            {
                yield return GetCell(xCenter, bottomY);
            }

            if (rightX < Width)
            {
                yield return GetCell(rightX, yCenter);
            }

            if (includeDiagonals)
            {
                if (rightX < Width && topY >= 0)
                {
                    yield return GetCell(rightX, topY);
                }

                if (rightX < Width && bottomY < Height)
                {
                    yield return GetCell(rightX, bottomY);
                }

                if (leftX >= 0 && topY >= 0)
                {
                    yield return GetCell(leftX, topY);
                }

                if (leftX >= 0 && bottomY < Height)
                {
                    yield return GetCell(leftX, bottomY);
                }
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
}
