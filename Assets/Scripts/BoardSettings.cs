using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BoardSettings : ScriptableObject
{
    public int width;
    public int height;

    public float tileWidth;
    public float tileHeight;

    public Tile[] tilePrefabs;
}
