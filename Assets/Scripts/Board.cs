using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public int height;

    public int boarderSize;

    public GameObject tilePrefab;

    Tile[,] m_allTiles;


	// Use this for initialization
	void Start ()
    {
        m_allTiles = new Tile[width, height];
        SetupTiles();
        //SetupCamera();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void SetupTiles()
    {
        for(int i=0; i < width; i++)
        {
            for(int j=0;j<height;j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;

                tile.name = "Tile (" + i + "," + j + ")";

                m_allTiles[i, j] = tile.GetComponent<Tile>();

                tile.transform.parent = transform;
            }
        }
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
