using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileTypes
{
    None,
    Sword,
    Shield,
    Potion,
    Red,
    Green,
    Blue,
}

public class Board : MonoBehaviour
{
    public bool debug;
    public BoardSettings settings;
    public SearchPattern pattern;
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

    void OnGUI()
    {
        if (!debug)
        {
            return;
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                TileTypes tileType = GetTileType(x, y);
                GUI.Label(new Rect(new Vector2(x, height - 1 - y) * 64, Vector2.one * 64), tileType.ToString());
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
        RefillBoard();
    }

    void RefillBoard()
    {
        createStack.Begin();
        Debug.Log("Refill the board");
        var iterator = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (GetTileType(x, y) != TileTypes.None)
                {
                    continue;
                }
                iterator++;
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
        Debug.Log("Added this many new elements: " + iterator);
        createStack.End();
    }

    private void DestroyBoard()
    {
        removeStack.Begin();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                removeStack.Add(new RemoveAction(this, x, y));
            }
        }
        removeStack.End();
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
        return (TileTypes)(Random.Range(1, System.Enum.GetNames(typeof(TileTypes)).Length));
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[y, x];
    }

    public bool IsEmpty(int x, int y)
    {
        return GetTileType(x, y) == TileTypes.None;
    }

    public bool IsNull(int x, int y)
    {
        return GetTile(x, y) == null ? true : false;
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
        Debug.Log("Swap resolved");
        List<Tile> matches = new List<Tile>();

        foreach (SwapAction swapInfo in history)
        {
            matches = matches.Union(GetAllMatches(swapInfo)).ToList();
        }
        Debug.Log("Matches found: " + matches.Count);
        //Enter here if the player made the swap and there's no match, swap back the tiles
        if(history.Count == 1 && history[0].swapReason == SwapReason.PlayerInput && matches.Count < 3)
        {
            swapStack.Begin();
            Debug.Log("Didnt find a match, swap back");
            swapStack.Add(new SwapAction(this,
                                    history[0].p1,
                                    history[0].p2));
            swapStack.End();
        }
        //If we found a match, remove all matches
        if(matches.Count >= 3)
        {
            removeStack.Begin();
            foreach (Tile tile in matches)
            {
                removeStack.Add(new RemoveAction(this, tile.x, tile.y));
            }
            removeStack.End();
        }
        if(matches.Count < 3 && NeedsToRefill())
        {
            RefillBoard();
        }
    }

    public void CreateResolved(List<CreateAction> history)
    {
        // Do after create stuff
        if(!AnyPossibleMatch())
        {
            Debug.Log("No possible match, destroy board");
            DestroyBoard();
        }
        else
        {
            Debug.Log("Found moves");
        }
        //Debug.Log("Create Done");
    }
    public void RemoveResolved(List<RemoveAction> history)
    {
        // Do after remove stuff
        Debug.Log("Remove Done");
        List<int> columnsToRemove = new List<int>();
        foreach (RemoveAction removeAction in history)
        {
            if (!columnsToRemove.Contains(removeAction.x))
            {
                columnsToRemove.Add(removeAction.x);
            }
        }

        CollapseColumns(columnsToRemove);
    }

    private void CollapseColumns(List<int> columns, float collapseTime = 0.1f)
    {
        swapStack.Begin();
        foreach (int column in columns)
        {
            for (int i = 0; i < height - 1; i++)
            {
                if (IsEmpty(column, i))
                {
                    for (int j = i + 1; j < height; j++)
                    {
                        if (!IsEmpty(column, j))
                        {
                            swapStack.Add(new SwapAction(this,
                                          new Vector2Int(column, i),
                                          new Vector2Int(column, j)));
                            break;
                        }
                    }
                }
            }
        }
        //If we didnt need to swap, then we refill the board
        if(swapStack.length <= 0)
        {
            Debug.Log("Didnt need to swap, refill board");
            RefillBoard();
        }
        else
        {
            Debug.Log("Collapse columns");
        }
        swapStack.End();
    }

    private List<Tile> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        List<Tile> matches = new List<Tile>();
        Tile startTile = null;

        if (IsWithinBounds(startX, startY))
        {
            startTile = tiles[startY, startX];
        }

        if (startTile != null)
        {
            matches.Add(startTile);
        }
        else
        {
            return new List<Tile>();
        }

        int nextX;
        int nextY;
        var startType = GetTileType(startX, startY);
        int maxValue = (width > height) ? width : height;

        for (int i = 1; i < maxValue - 1; i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            //Check if we're outside of the bounds
            if (!IsWithinBounds(nextX, nextY))
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

        if (matches.Count >= minLength)
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

    private List<Tile> GetAllMatches(SwapAction swapInfo)
    {
        List<Tile> matches = new List<Tile>();

        var matchesOnTile1 = FindMatchesAt(swapInfo.p1.x, swapInfo.p1.y, 3);
        var matchesOnTile2 = FindMatchesAt(swapInfo.p2.x, swapInfo.p2.y, 3);

        matches = matchesOnTile1.Union(matchesOnTile2).ToList();
        return matches;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    public bool CheckAdjacent(Tile t1, Tile t2)
    {
        if(Mathf.Abs(t1.x-t2.x) == 1 && Mathf.Abs(t1.y - t2.y) == 0)
        {
            return true;
        }
        else if(Mathf.Abs(t1.x - t2.x) == 0 && Mathf.Abs(t1.y - t2.y) == 1)
        {
            return true;
        }

        return false;
    }

    private bool NeedsToRefill()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(IsEmpty(x,y))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool AnyPossibleMatch()
    {        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tileType = GetTileType(x, y);
                if(PossibleHorizontalMatches(x,y,tileType) || PossibleVerticalMatches(x,y,tileType) || PossibleMiddleMatch(x,y,tileType))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool PossibleHorizontalMatches(int x, int y, TileTypes tileType)
    {
        //S = StartTile
        //0 = Not checked
        //1 = Check these for the same tileType as S
        
        //If we're within bounds three to the right, check for right possible matches S,0,1,1
        if (IsWithinBounds(x+3,y))
        {
            if(GetTileType(x+2,y) == tileType && GetTileType(x+3,y) == tileType)
            {
                return true;
            }
        }
        //If we're within bounds three to the left, check for left possible matches 1,1,0,S
        if (IsWithinBounds(x-3,y))
        {
            if (GetTileType(x - 2, y) == tileType && GetTileType(x - 3, y) == tileType)
            {
                return true;
            }
        }

        return false;
    }

    private bool PossibleVerticalMatches(int x, int y, TileTypes tileType)
    {
        //S = StartTile
        //0 = Not checked
        //1 = Check these for the same tileType as S

        //If we're within bounds three upwards, check for upwards possible matches 
        //1
        //1
        //0
        //S
        if (IsWithinBounds(x, y+3))
        {
            if (GetTileType(x, y+2) == tileType && GetTileType(x, y+3) == tileType)
            {
                return true;
            }
        }

        //If we're within bounds three downwards, check for downwards possible matches 
        //S
        //0
        //1
        //1
        if (IsWithinBounds(x, y-3))
        {
            if (GetTileType(x, y-2) == tileType && GetTileType(x, y-3) == tileType)
            {
                return true;
            }
        }

        return false;
    }

    private bool PossibleMiddleMatch(int x, int y, TileTypes tileType)
    {
        //Check for upwards middlematch
        //1 0 1
        //0 S 0
        if (IsWithinBounds(x - 1, y + 1) && IsWithinBounds(x + 1, y + 1))
        {
            if (GetTileType(x - 1, y + 1) == tileType && GetTileType(x + 1, y + 1) == tileType)
            {
                return true;
            }
        }

        //Check for downwards middlematch
        //0 S 0
        //1 0 1
        if (IsWithinBounds(x - 1, y - 1) && IsWithinBounds(x + 1, y - 1))
        {
            if (GetTileType(x - 1, y - 1) == tileType && GetTileType(x + 1, y - 1) == tileType)
            {
                return true;
            }
        }

        //Check for left middlematch
        //1 0
        //0 S
        //1 0
        if (IsWithinBounds(x - 1, y - 1) && IsWithinBounds(x - 1, y + 1))
        {
            if (GetTileType(x - 1, y - 1) == tileType && GetTileType(x - 1, y + 1) == tileType)
            {
                return true;
            }
        }

        //Check for right middlematch
        //0 1
        //S 0
        //0 1
        if (IsWithinBounds(x + 1, y - 1) && IsWithinBounds(x + 1, y + 1))
        {
            if (GetTileType(x + 1, y - 1) == tileType && GetTileType(x + 1, y + 1) == tileType)
            {
                return true;
            }
        }
        return false;
    }
}