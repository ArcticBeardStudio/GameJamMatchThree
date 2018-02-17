using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileSettings settings;
    public int x;
    public int y;

    public Board board { get; protected set; }

    new public BoxCollider collider { get; protected set; }
    public Vector2Int boardPos { get { return new Vector2Int(x, y); } }

    public void Init(int x, int y, Board board)
    {
        this.x = x;
        this.y = y;
        this.board = board;

        transform.localPosition = board.GetTileLocalPosition(x, y);

        collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(1, 1, 0.1f);
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }

    private void OnDrawGizmosSelected()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;

        Handles.BeginGUI();
        Vector3 pos = transform.position;
        Vector2 pos2D = HandleUtility.WorldToGUIPoint(pos);
        //GUI.Label(new Rect(pos2D.x, pos2D.y, 100, 100), x + "," + y + board.GetTile(x, y).name + "/" + board.GetTileType(x, y), style);
        GUI.Label(new Rect(pos2D.x, pos2D.y, 100, 100), board.GetTileType(x,y).ToString() + " " + board.IsNull(x,y).ToString(), style);
        Handles.EndGUI();
    }
}
