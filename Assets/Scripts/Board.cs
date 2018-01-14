using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes
{
    Sword,
    Shield,
    Potion,
    Red,
    Green,
    Blue,
    None
}

public struct SwapInfo
{
    public Tile tile;
    public Tile tile2;

    public SwapInfo(Tile tile, Tile tile2)
    {
        this.tile = tile;
        this.tile2 = tile2;
    }
}

public class Board : MonoBehaviour
{
    public BoardSettings settings;
    int width { get { return settings.width; } }
    int height { get { return settings.height; } }
    float tileWidth { get { return settings.tileWidth; } }
    float tileHeight { get { return settings.tileHeight; } }
    Tile[] tilePrefabs { get { return settings.tilePrefabs; } }

    Tile[,] tiles;
    TileTypes[,] currentState;

    void Start()
    {
        Init();
    }
    
    void OnDrawGizmos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 tilePos = GetTileLocalPosition(x, y) + transform.position;
                Gizmos.DrawWireCube(tilePos, new Vector3(tileWidth, tileHeight));
            }
        }
    }

    public void Init()
    {
        tiles = new Tile[height, width];
        currentState = new TileTypes[height, width];
        SetupTiles();
    }

    void SetupTiles()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                TileTypes tileType = GetRandomTileType();

                int tileLeft = x > 0 ? (int)GetTileType(x - 1, y) : -1;
                int tileDown = y > 0 ? (int)GetTileType(x, y - 1) : -1;

                while ((int)tileType == tileLeft || (int)tileType == tileDown)
                {
                    tileType = GetRandomTileType();
                }
                SetTileType(x, y, tileType);
            }
        }
    }

    public Vector3 GetTileLocalPosition(int x, int y)
    {
        return new Vector3(((x + 0.5f) - width * 0.5f) * tileWidth, ((y + 0.5f) - height * 0.5f) * tileHeight);
    }

    public TileTypes GetTileType(int x, int y)
    {
        return currentState[y, x];
    }

    public TileTypes GetTileType(Tile tile)
    {
        return currentState[tile.y, tile.x];
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[y, x];
    }

    public TileTypes GetRandomTileType()
    {
        return (TileTypes)Random.Range(0, System.Enum.GetNames(typeof(TileTypes)).Length - 1);
    }

    public void SetTileType(int x, int y, TileTypes tileType)
    {
        currentState[y, x] = tileType;
        UpdateTile(x, y, tileType);
    }
    public void SetTileType(Tile tile, TileTypes tileType)
    {
        SetTileType(tile.x, tile.y, tileType);
    }

    public void UpdateTile(int x, int y, TileTypes tileType)
    {
        if (GetTile(x, y))
        {
            //Debug.Log("Deleted: " + GetTileType(x,y).ToString());
            Destroy(GetTile(x, y).gameObject);
        }
        if (tileType != TileTypes.None)
        {
            Tile newTile = Instantiate<Tile>(tilePrefabs[(int)tileType], transform);
            newTile.transform.localPosition = GetTileLocalPosition(x, y);
            newTile.Init(x, y, this);
            tiles[y, x] = newTile;
            //Debug.Log("Created: " + GetTileType(x, y).ToString());
        }
    }

    //Simply checks if two tiles are adjacent
    public bool Adjacent(Tile t1, Tile t2)
    {
        int dx = System.Math.Abs(t2.x - t1.x);
        int dy = System.Math.Abs(t2.y - t1.y);
        return dx * dx + dy * dy == 1;
    }

    /*
    //Call when trying to make a move with two tiles, returns true if a match is found and swaps the tiles
    public bool TryMakeMove(Tile t1, Tile t2)
    {
        var foundMatch = false;
        if (Adjacent(t1, t2))
        {
            //Tries to swap and waits 0.5f seconds before checking if it's a match
            
            if (CheckMatch(t2, GetTileType(t1)))
            {
                foundMatch = true;
            }
            if (CheckMatch(t1, GetTileType(t2)))
            {
                foundMatch = true;
            }

            //If no match is found, swap back to previous
            if (!foundMatch)
            {
                Debug.Log("No match m8");
            }
        }
        return foundMatch;
    }*/
    
    public void SwapTiles(SwapInfo swapperino)
    {
        var tile = swapperino.tile;
        var tile2 = swapperino.tile2;
        var previousTilePos = new Vector2Int(tile.x, tile.y);
        var previousTile2Pos = new Vector2Int(tile2.x, tile2.y);
        var previousTileType = GetTileType(tile);
        var previousTile2Type = GetTileType(tile2);

        tile.x = previousTile2Pos.x;
        tile.y = previousTile2Pos.y;
        tile2.x = previousTilePos.x;
        tile2.y = previousTilePos.y;
        SetTileType(tile, previousTileType);
        SetTileType(tile2, previousTile2Type);
    }

    public void RemoveMatches()
    {

    }
}