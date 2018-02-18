using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MatchPattern : ScriptableObject
{
    public SearchPattern[] patterns;
}

/// <summary>
/// Describes a gem pattern that can be searched for on the board
/// </summary>
[System.Serializable]
public class SearchPattern
{
    public string name = "New Pattern";
    public char[,] pattern = new char[1, 1] { { 'S' } };
    public int[] size = new int[2] { 1, 1 };

    public void Resize(int w, int h)
    {
        pattern = new char[h, w];
        size = new int[] { GetLength(0), GetLength(1) };
    }

    public int GetLength(int dimension)
    {
        switch (dimension) {
            case 0: return pattern.GetLength(1);
            case 1: return pattern.GetLength(0);
            default: return -1;
        }
    }

    // Indexer for fast interfacing
    // NOTE: X and Y are switched in the getter and setter
    public char this[int x, int y]
    {
        get { return pattern[y, x]; }
        set { pattern[y, x] = value; }
    }
}

public static class SearchRules
{

}