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

public class Board : MonoBehaviour
{
    public BoardSettings settings;
    int width { get { return settings.width; } }
    int height { get { return settings.height; } }
    float tileWidth { get { return settings.tileWidth; } }
    float tileHeight { get { return settings.tileHeight; } }
    Tile[] tilePrefabs { get { return settings.tilePrefabs; } }

    public ChangeStack<SwapInfo> swapStack;
    public ChangeStack<CreateInfo> createStack;

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

        swapStack = new ChangeStack<SwapInfo>(SwapResolved);
        createStack = new ChangeStack<CreateInfo>(CreateResolved);

        SetupTiles();
    }

    void SetupTiles()
    {
        createStack.Begin();
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
                createStack.Add(new CreateInfo(this, x, y, tileType));
            }
        }
        createStack.End();
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
        return GetTileType(tile.x, tile.y);
    }
    
    public TileTypes GetRandomTileType()
    {
        return (TileTypes)Random.Range(0, System.Enum.GetNames(typeof(TileTypes)).Length - 1);
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[y, x];
    }

    public void SetTileType(int x, int y, TileTypes tileType)
    {
        currentState[y, x] = tileType;
    }
    public void SetTileType(Tile tile, TileTypes tileType)
    {
        SetTileType(tile.x, tile.y, tileType);
    }
    
    public void SetTile(int x, int y, Tile tile)
    {
        tiles[y, x] = tile;
    }

    //Simply checks if two tiles are adjacent
    public bool Adjacent(Tile t1, Tile t2)
    {
        int dx = System.Math.Abs(t2.x - t1.x);
        int dy = System.Math.Abs(t2.y - t1.y);
        return dx * dx + dy * dy == 1;
    }

    public void SwapResolved()
    {
        // Do after swap stuff
        Debug.Log("Swap Done");
    }
    public void CreateResolved()
    {
        // Do after create stuff
        Debug.Log("Create Done");
    }

    public void FindMatches(Vector2Int origin, TileTypes tileType)
    {
        var horizontalMatches = new List<Vector2Int>();
        var verticalMatches = new List<Vector2Int>();

        var allFoundMatches = new List<Vector2Int>();

        //Check horizontal matches
        for (int i=0;i<width;i++)
        {
            if(GetTileType(i,origin.y) == tileType)
            {
                horizontalMatches.Add(new Vector2Int(i,origin.y));
            }
        }

        if(horizontalMatches.Count >= 3)
        {
            Debug.Log("Found horizontal match from" + horizontalMatches[0] + " to " + horizontalMatches[horizontalMatches.Count - 1]);
            for (int i = 0; i < horizontalMatches.Count; i++)
            {
            }
        }

        //Check vertical matches
        for (int i = 0; i < height; i++)
        {
            if (GetTileType(origin.x,i) == tileType)
            {
                verticalMatches.Add(new Vector2Int(origin.x, i));
            }
        }

        if (verticalMatches.Count >= 3)
        {
            Debug.Log("Found vertical match from" + verticalMatches[0] + " to " + verticalMatches[verticalMatches.Count - 1]);
            for(int i=0;i<verticalMatches.Count;i++)
            {
            }
        }
    }

    //Handles what happens when you remove a tile
    public void RemoveTile(int x, int y)
    {
        var tile = GetTile(x, y);
        tile.Pop();
        Destroy(tile.gameObject);
    }
}