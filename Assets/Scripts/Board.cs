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
    Blue
}

public class Board : MonoBehaviour
{
    public int height;
    public int width;

    Tile[,] tiles;
    TileTypes[,] tileTypes;

    public Tile[] tilePrefabs;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        tiles = new Tile[height, width];
        tileTypes = new TileTypes[height, width];
        SetupTiles();
    }

    void SetupTiles()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                TileTypes tileType = (TileTypes)Random.Range(0,System.Enum.GetNames(typeof(TileTypes)).Length);

                int tileLeft = x > 0 ? (int)GetTileType(x - 1, y) : -1;
                int tileDown = y > 0 ? (int)GetTileType(x, y - 1) : -1;

                while ((int)tileType == tileLeft || (int)tileType == tileDown)
                {
                    tileType = (TileTypes)Random.Range(0, System.Enum.GetNames(typeof(TileTypes)).Length);
                }
                SetTileType(x, y, tileType);
            }
        }
    }

    public TileTypes GetTileType(int x, int y)
    {
        return tileTypes[y, x];
    }

    public TileTypes GetTileType(Tile tile)
    {
        return tileTypes[tile.y, tile.x];
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[y, x];
    }

    public void SetTileType(int x, int y, TileTypes tileType)
    {
        tileTypes[y, x] = tileType;
        TileUpdated(x, y, tileTypes[y, x]);
    }

    public void SetTileType(Tile tile, TileTypes tileType)
    {
        tileTypes[tile.y, tile.x] = tileType;
        TileUpdated(tile.x, tile.y, tileType);
    }

    void TileUpdated(int x, int y, TileTypes tileType)
    {
        if (tiles[y, x] != null)
        {
            Destroy(tiles[y, x].gameObject);
        }
        if((int)tileType <= tilePrefabs.Length || tilePrefabs.Length != 0)
        {
            Tile newTile = Instantiate<Tile>(tilePrefabs[(int)tileType], transform);
            newTile.Init(x, y, this);
            tiles[y, x] = newTile;
        }
        else
        {
            Debug.LogWarning("TileUpdated error: tilePrefabs length 0 or tileType > tilePrefabs");
        }
    }

    public bool Adjacent(Tile t1, Tile t2)
    {
        int dx = System.Math.Abs(t2.x - t1.x);
        int dy = System.Math.Abs(t2.y - t1.y);
        return dx * dx + dy * dy == 1;
    }

    public bool TryMakeMove(Tile t1, Tile t2)
    {
        var foundMatch = false;
        if (Adjacent(t1, t2))
        {
            if (CheckMatch(t2, GetTileType(t1)))
            {
                foundMatch = true;
            }
            if (CheckMatch(t1, GetTileType(t2)))
            {
                foundMatch = true;
            }

            if(foundMatch)
            {
                TileTypes tile1 = GetTileType(t1);
                TileTypes tile2 = GetTileType(t2);
                SetTileType(t1, tile2);
                SetTileType(t2, tile1);
            }
        }
        return foundMatch;
    }

    public bool CheckMatch(Tile tile, TileTypes tileType)
    {
        var foundMatch = false;
        var horizontalMatches = 0;
        var verticalMatches = 0;

        var horizontalRightX = 0;
        var horizontalLeftX = 0;
        var verticalTopY = 0;
        var verticalBotY = 0;


        int xIndex = tile.x;
        int yIndex = tile.y;

        //Check matches to the right
        while (xIndex+1 < width)
        {
            xIndex += 1;
            if(GetTileType(xIndex,yIndex) == tileType)
            {
                horizontalMatches++;
            }
            else
            {
                horizontalRightX = xIndex - 1;
                break;
            }
        }
        xIndex = tile.x;

        //Check matches to the left
        while (xIndex-1 >= 0)
        {
            xIndex -= 1;
            if (GetTileType(xIndex, yIndex) == tileType)
            {
                horizontalMatches++;
            }
            else
            {
                horizontalLeftX = xIndex + 1;
                break;
            }
        }

        if(horizontalMatches >= 2)
        {
            Debug.Log("FOUND HORIZONTAL MATCH BETWEEN: " + horizontalLeftX + "," + tile.y + "and " + horizontalRightX + "," + tile.y);
            foundMatch = true;
        }

        xIndex = tile.x;
        yIndex = tile.y;

        //Check matches up
        while (yIndex + 1 < height)
        {
            yIndex += 1;
            if (GetTileType(xIndex, yIndex) == tileType)
            {
                verticalMatches++;
            }
            else
            {
                verticalTopY = yIndex - 1;
                break;
            }
        }
        yIndex = tile.y;

        //Check matches down
        while (yIndex - 1 >= 0)
        {
            yIndex -= 1;
            if (GetTileType(xIndex, yIndex) == tileType)
            {
                verticalMatches++;
            }
            else
            {
                verticalBotY = yIndex + 1;
                break;
            }
        }

        if (verticalMatches >= 2)
        {
            Debug.Log("FOUND VERTICAL MATCH BETWEEN: " + tile.x + "," + verticalBotY + "and " + tile.x + "," + verticalTopY);
            foundMatch = true;
        }

        return foundMatch;
    }
    
}
