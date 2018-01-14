﻿using System.Collections;
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

    void UpdateTile(int x, int y, TileTypes tileType)
    {
        if (GetTile(x, y))
        {
            Destroy(GetTile(x, y).gameObject);
            RefillBoard();
        }
        if (tileType != TileTypes.None)
        {
            Tile newTile = Instantiate<Tile>(tilePrefabs[(int)tileType], transform);
            newTile.transform.localPosition = GetTileLocalPosition(x, y);
            newTile.Init(x, y, this);
            tiles[y, x] = newTile;
        }
    }

    //Simply checks if two tiles are adjacent
    public bool Adjacent(Tile t1, Tile t2)
    {
        int dx = System.Math.Abs(t2.x - t1.x);
        int dy = System.Math.Abs(t2.y - t1.y);
        return dx * dx + dy * dy == 1;
    }

    //Call when trying to make a move with two tiles, returns true if a match is found and swaps the tiles
    public bool TryMakeMove(Tile t1, Tile t2)
    {
        var foundMatch = false;
        if (Adjacent(t1, t2))
        {
            //Tries to swap and waits 0.5f seconds before checking if it's a match
            TileTypes tile1 = GetTileType(t1);
            TileTypes tile2 = GetTileType(t2);
            SetTileType(t1, tile2);
            SetTileType(t2, tile1);

            StartCoroutine(pause());

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
                SetTileType(t1, tile1);
                SetTileType(t2, tile2);
            }
        }
        return foundMatch;
    }

    //Called when trying to make a move with two adjacent tiles
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
        while (xIndex + 1 < width)
        {
            xIndex += 1;
            if (GetTileType(xIndex, yIndex) == tileType)
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
        while (xIndex - 1 >= 0)
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

        if (horizontalMatches >= 2)
        {
            Debug.Log("FOUND HORIZONTAL MATCH BETWEEN: " + horizontalLeftX + "," + tile.y + "and " + horizontalRightX + "," + tile.y);
            RemoveFromIndexTo(horizontalLeftX, tile.y, horizontalRightX, tile.y);
            foundMatch = true;
        }

        if (verticalMatches >= 2)
        {
            Debug.Log("FOUND VERTICAL MATCH BETWEEN: " + tile.x + "," + verticalBotY + "and " + tile.x + "," + verticalTopY);
            RemoveFromIndexTo(tile.x, verticalBotY, tile.x, verticalTopY);
            foundMatch = true;
        }

        return foundMatch;
    }

    //When a match is found, remove each tiles from x1:y1 to x2:y2
    public void RemoveFromIndexTo(int x1, int y1, int x2, int y2)
    {
        //Remove vertical
        if (x1 == x2)
        {
            var yIndex = y1;
            while (yIndex <= y2)
            {
                GetTile(x1, yIndex).Pop();
                yIndex++;
            }
        }
        //Remove horizontal
        else
        {
            var xIndex = x1;
            while (xIndex <= x2)
            {
                GetTile(xIndex, y1).Pop();
                xIndex++;
            }
        }
        RefillBoard();
    }

    //Refills all the empty tiles with random tiles
    public void RefillBoard()
    {
        for (int y = 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!GetTile(x, y - 1))
                {
                    MoveDownRow(x, y);
                }
            }
        }

        for (int y = 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (GetTileType(x, y) == TileTypes.None)
                {
                    TileTypes tileType = (TileTypes)Random.Range(0, System.Enum.GetNames(typeof(TileTypes)).Length - 1);
                    SetTileType(x, y, tileType);
                }
            }
        }
        CheckNewMatch();
    }

    //Moves down an entire row when a match is found
    void MoveDownRow(int x, int y)
    {
        var yIndex = y;
        while (yIndex < height)
        {
            GetTile(x, yIndex).y--;
            TileTypes tile1 = GetTileType(x, yIndex);
            TileTypes tile2 = GetTileType(x, yIndex - 1);
            SetTileType(x, yIndex, tile2);
            SetTileType(x, yIndex - 1, tile1);
            yIndex++;
        }
    }

    //After the board is refilled, CheckNewMatch is called.
    public void CheckNewMatch()
    {
        TileTypes previousTile = TileTypes.None;
        for (int y = 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var currentType = GetTileType(x, y);
                if (previousTile != currentType)
                {
                    if (CheckMatch(GetTile(x, y), GetTileType(x, y)))
                    {
                        return;
                    }
                }
                previousTile = currentType;
            }
        }
    }

    public void CheckPossibleMatches()
    {

    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(0.5f);
    }

}
