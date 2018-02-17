using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public ChangeStack<SwapAction> swapStack;
    public ChangeStack<CreateAction> createStack;
    public ChangeStack<RemoveAction> removeStack;

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

        swapStack = new ChangeStack<SwapAction>(SwapResolved);
        createStack = new ChangeStack<CreateAction>(CreateResolved);
        removeStack = new ChangeStack<RemoveAction>(RemoveResolved);

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
                createStack.Add(new CreateAction(this, x, y, tileType));
            }
        }
        createStack.End();
    }

    void RefillBoard()
    {
        createStack.Begin();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(GetTileType(x,y) == TileTypes.None)
                {
                    TileTypes tileType = GetRandomTileType();
                    createStack.Add(new CreateAction(this, x, y, tileType));
                }
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

    public bool IsEmpty(int x, int y)
    {
        return GetTileType(x, y) == TileTypes.None;
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

    public void SwapResolved(List<SwapAction> history)
    {
        // Do after swap stuff
        // If not filled
        // Refill
        // Else \/
        //Vector2Int[] matches = FindMatches();

        List<Tile> matches = new List<Tile>();

        foreach (SwapInfo swapInfo in history)
        {
            var matchesOnTile1 = FindMatchesAt(swapInfo.p1.x, swapInfo.p1.y, 3);
            var matchesOnTile2 = FindMatchesAt(swapInfo.p2.x, swapInfo.p2.y, 3);

            matches = matchesOnTile1.Union(matchesOnTile2).ToList();
        }

        removeStack.Begin();
        foreach (Tile tile in matches)
        {
            removeStack.Add(new RemoveAction(this, pos.x, pos.y));
        }
        removeStack.End();
    }
    public void CreateResolved(List<CreateAction> history)
    {
        // Do after create stuff
        Debug.Log("Create Done");
    }
    public void RemoveResolved(List<RemoveAction> history)
    {
        // Do after remove stuff
        Debug.Log("Remove Done");
        FallGems();
    }

    public void FallGems()
    {
        swapStack.Begin();
        for (int x = 0; x < width; x++)
        {
            int emptyY = -1;
            for (int y = 0; y < height; y++)
            {
                if (IsEmpty(x, y) && emptyY < 0)
                {
                    emptyY = y;
                    continue;
                }
                if (!IsEmpty(x, y) && emptyY >= 0)
                {
                    swapStack.Add(new SwapAction(this, 
                        new Vector2Int(x, y), 
                        new Vector2Int(x, emptyY)
                    ));
                }
            }
        }
        swapStack.End();
    }

    /*public Vector2Int[] FindMatches()
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
                if (GetTileType(x, y) == previousType && previousType != TileTypes.None)
                {
                    horizontalMatches.Add(new Vector2Int(x, y));
                }
                //If we found horizontal matches (And previous type not same as current), add to foundMatches list and empty horizontal match, add the new type to the horizontal matches
                else if (horizontalMatches.Count >= 3)
                {
                    for (int i = 0; i < horizontalMatches.Count; i++)
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
                        horizontalMatches.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

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


        for (int x = 0; x < width; x++)
        {
            //Checks if we found vertical match, needed when the last tile is within a match.
            if (verticalMatches.Count >= 3)
            {
                for (int i = 0; i < verticalMatches.Count; i++)
                {
                    if (!allFoundMatches.Contains(verticalMatches[i]))
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
                        verticalMatches.Add(new Vector2Int(x, y));
                    }
                }
            }
        }


        //Checks if we found horizontal match, needed when the last tile is within a match.
        if (verticalMatches.Count >= 3)
        {
            for (int i = 0; i < verticalMatches.Count; i++)
            {
                if (!allFoundMatches.Contains(verticalMatches[i]))
                {
                    allFoundMatches.Add(verticalMatches[i]);
                }
            }
        }

        Debug.Log("Found matches is: " + allFoundMatches.Count);
        return allFoundMatches.ToArray();
    }*/

    private List<Tile> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        List<Tile> matches = new List<Tile>();
        Tile startTile = null;

        if (IsWithinBounds(startX, startY))
        {
            startTile = tiles[startY, startX];
        }

        if(startTile != null)
        {
            matches.Add(startTile);
        }
        else
        {
            return null;
        }

        int nextX;
        int nextY;

        int maxValue = (width>height) ? width: height;

        for (int i=1; i<maxValue-1;i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            //Check if we're outside of the bounds
            if(!IsWithinBounds(nextX,nextY))
            {
                break;
            }
            Tile nextTile = tiles[nextY, nextX];
            if (nextTile == null)
            {
                break;
            }
            else
            {
                var startType = GetTileType(startX, startY);
                var nextType = GetTileType(nextX, nextY);
                if (startType == nextType)
                {
                    matches.Add(nextTile);
                }
                else
                {
                    break;
                }
            }
        }

        if(matches.Count >= minLength)
        {
            return matches;
        }

        return new List<Tile>();
    }

    private List<Tile> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<Tile> rightMatches = FindMatches(startX, startY, new Vector2(1, 0), 2);
        List<Tile> leftMatches = FindMatches(startX, startY, new Vector2(-1, 0), 2);

        var combinedMatches = rightMatches.Union(leftMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : new List<Tile>();
    }

    private List<Tile> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<Tile> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1), 2);
        List<Tile> downwardMatches = FindMatches(startX, startY, new Vector2(0, -1), 2);

        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : new List<Tile>();
    }

    private List<Tile> FindMatchesAt(int x, int y, int minLength = 3)
    {
        List<Tile> horizontalMatches = FindHorizontalMatches(x, y, minLength);
        List<Tile> verticalMatches = FindVerticalMatches(x, y, minLength);

        var combinedMatches = horizontalMatches.Union(verticalMatches).ToList();
        return combinedMatches;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }
}