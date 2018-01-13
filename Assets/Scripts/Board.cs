using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Board : MonoBehaviour
{

    public int width;
    public int height;

    public int boarderSize;

    public Tile[] tilePrefabs;

    Tile[,] tiles;
    BoardData boardData;

    // Use this for initialization
    void Start()
    {
        //SetupCamera();
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTileUpdate(int x, int y, TileTypes tileType)
    {
        if (tiles[y, x] != null)
        {
            Destroy(tiles[y, x].gameObject);
        }

        Tile newTile = Instantiate<Tile>(tilePrefabs[(int)tileType], transform);
        newTile.Init(x, y, this);
        tiles[y, x] = newTile;
    }

    void Init()
    {
        tiles = new Tile[height, width];
        boardData = new BoardData(width, height, new TileUpdate(OnTileUpdate));
    }

    public bool SwapTiles(Tile t1, Tile t2)
    {
        return boardData.SwapTiles(t1.x, t1.y, t2.x, t2.y);
    }

    /*void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float)(width-1) / 2f, (float)(height-1) / 2f, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float verticalSize = (float)height / 2f + (float)boarderSize;

        float horizontalSize = ((float)width / 2f + (float)boarderSize) / aspectRatio;
        
        if (verticalSize > horizontalSize)
        {
            Camera.main.orthographicSize = verticalSize;
        }
        else
        {
            Camera.main.orthographicSize = horizontalSize;
        }
    }*/
}
