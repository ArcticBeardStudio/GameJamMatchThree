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

public class SwapInfo : ChangeInfo
{
    public Tile tile;
    public Tile tile2;

    public SwapInfo(Board board, Tile tile, Tile tile2)
        :base(board)
    {
        this.tile = tile;
        this.tile2 = tile2;
    }

    override public IEnumerator ChangeRoutine(System.Action callback)
    {
        yield return new WaitForSeconds(0.1f);

        var previousTilePos = new Vector2Int(tile.x, tile.y);
        var previousTile2Pos = new Vector2Int(tile2.x, tile2.y);
        var previousTileType = board.GetTileType(tile);
        var previousTile2Type = board.GetTileType(tile2);

        tile.x = previousTile2Pos.x;
        tile.y = previousTile2Pos.y;
        tile2.x = previousTilePos.x;
        tile2.y = previousTilePos.y;
        board.SetTileType(tile, previousTileType);
        board.SetTileType(tile2, previousTile2Type);
        board.FindMatches();

        isComplete = true;
        callback();        
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

    public ChangeStack<SwapInfo> swapStack;

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
        swapStack = new ChangeStack<SwapInfo>(SwapResolved);

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

    public void SwapResolved()
    {
        // Do after swap stuff
    }

    public List<Vector2Int> FindMatches()
    {
        var horizontalMatches = new List<Vector2Int>();
        var verticalMatches = new List<Vector2Int>();
        var allFoundMatches = new List<Vector2Int>();
        
        //Check horizontal matches
        for (int y = 0; y < height; y++)
        {
            //Checks if we found horizontal match, needed when the last tile is within a match.
            if (horizontalMatches.Count >= 3)
            {
                for (int i = 0; i < horizontalMatches.Count; i++)
                {
                    if (!allFoundMatches.Contains(horizontalMatches[i]))
                    {
                        allFoundMatches.Add(horizontalMatches[i]);
                    }
                }
            }

            //Clear the horizontalMatches and sets previous type to the first type of the row.
            horizontalMatches.Clear();
            TileTypes previousType = GetTileType(0, y);
            if (previousType != TileTypes.None)
            {
                horizontalMatches.Add(new Vector2Int(0, y));
            }
            //Loops through the row, starts at one (The first item already added if not none)
            for (int x = 1; x < width; x++)
            {
                //If the current type is same as previous and not none, add to matches
                if(GetTileType(x,y) == previousType && previousType != TileTypes.None)
                {
                    horizontalMatches.Add(new Vector2Int(x, y));
                }
                //If we found horizontal matches (And previous type not same as current), add to foundMatches list and empty horizontal match, add the new type to the horizontal matches
                else if(horizontalMatches.Count >= 3)
                {
                    for(int i=0;i< horizontalMatches.Count; i++)
                    {
                        if (!allFoundMatches.Contains(horizontalMatches[i]))
                        {
                            allFoundMatches.Add(horizontalMatches[i]);
                        }
                    }

                    horizontalMatches.Clear();
                    previousType = GetTileType(x, y);
                    if (previousType != TileTypes.None)
                    {
                        horizontalMatches.Add(new Vector2Int(x, y));
                    }
                }
                //If we found a new type, empty matches and add to matches if not none type
                else
                {
                    previousType = GetTileType(x, y);
                    horizontalMatches.Clear();
                    if (previousType != TileTypes.None)
                    {
                        horizontalMatches.Add(new Vector2Int(0, y));
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            //Checks if we found horizontal match, needed when the last tile is within a match.
            if (verticalMatches.Count >= 3)
            {
                for (int i = 0; i < verticalMatches.Count; i++)
                {
                    if(!allFoundMatches.Contains(verticalMatches[i]))
                    {
                        allFoundMatches.Add(verticalMatches[i]);
                    }
                }
            }

            //Clear the verticalMatches and sets previous type to the first type of the row.
            verticalMatches.Clear();
            TileTypes previousType = GetTileType(x, 0);
            if (previousType != TileTypes.None)
            {
                verticalMatches.Add(new Vector2Int(x, 0));
            }

            //Loops through the row, starts at one (The first item already added if not none)
            for (int y = 1; y < height; y++)
            {
                //If the current type is same as previous and not none, add to matches
                if (GetTileType(x, y) == previousType && previousType != TileTypes.None)
                {
                    verticalMatches.Add(new Vector2Int(x, y));
                }
                //If we found vertical matches (And previous type not same as current), add to foundMatches list and empty vertical match, add the new type to the vertical matches
                else if (verticalMatches.Count >= 3)
                {
                    for (int i = 0; i < verticalMatches.Count; i++)
                    {
                        if (!allFoundMatches.Contains(verticalMatches[i]))
                        {
                            allFoundMatches.Add(verticalMatches[i]);
                        }
                    }

                    verticalMatches.Clear();
                    previousType = GetTileType(x, y);
                    if (previousType != TileTypes.None)
                    {
                        verticalMatches.Add(new Vector2Int(x, y));
                    }
                }
                //If we found a new type, empty matches and add to matches if not none type
                else
                {
                    previousType = GetTileType(x, y);
                    verticalMatches.Clear();
                    if (previousType != TileTypes.None)
                    {
                        verticalMatches.Add(new Vector2Int(0, y));
                    }
                }
            }
        }


        Debug.Log("Found matches is: " + allFoundMatches.Count);
        return allFoundMatches;
    }

    //Handles what happens when you remove a tile
    public void RemoveTile(int x, int y)
    {
        var tile = GetTile(x, y);
        tile.Pop();
        Destroy(tile.gameObject);
    }
}