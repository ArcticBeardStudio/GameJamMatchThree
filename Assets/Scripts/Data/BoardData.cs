using System.Collections;
using System.Collections.Generic;

namespace Data
{
    public enum TileTypes
    {
        Sword,
        Shield,
        Potion,
        Red,
        Green,
        Blue
    }

    public delegate void TileUpdate(int x, int y, TileTypes tileType);

    public class BoardData
    {
        public int width { get; protected set; }
        public int height { get; protected set; }
        public event TileUpdate TileUpdated;

        TileTypes[,] tiles;
        System.Random random;

        public BoardData(int width, int height, TileUpdate del)
        {
            this.width = width;
            this.height = height;
            TileUpdated = del;

            random = new System.Random();
            SetupTiles();
        }

        void SetupTiles()
        {
            tiles = new TileTypes[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    TileTypes tileType = (TileTypes)random.Next(System.Enum.GetNames(typeof(TileTypes)).Length);

                    int tileLeft = x > 0 ? (int)GetTileType(x - 1, y) : -1;
                    int tileDown = y > 0 ? (int)GetTileType(x, y - 1) : -1;

                    while ((int)tileType == tileLeft || (int)tileType == tileDown)
                    {
                        tileType = (TileTypes)random.Next(System.Enum.GetNames(typeof(TileTypes)).Length);
                    }
                    SetTileType(x, y, tileType);
                }
            }
        }

        public TileTypes GetTileType(int x, int y)
        {
            return tiles[y, x];
        }

        public void SetTileType(int x, int y, TileTypes tileType)
        {
            tiles[y, x] = tileType;
            TileUpdated(x, y, tiles[y, x]);
        }

        public bool SwapTiles(int x1, int y1, int x2, int y2)
        {
            if (CanMove(x1, y1, x2, y2))
            {
                TileTypes t1 = GetTileType(x1, y1);
                TileTypes t2 = GetTileType(x2, y2);
                SetTileType(x1, y1, t2);
                SetTileType(x2, y2, t1);
                return true;
            }
            return false;
        }

        public bool CanMove(int x1, int y1, int x2, int y2) {
            int dx = System.Math.Abs(x2 - x1);
            int dy = System.Math.Abs(y2 - y1);
            return dx * dx + dy * dy == 1;
        }
    }
}