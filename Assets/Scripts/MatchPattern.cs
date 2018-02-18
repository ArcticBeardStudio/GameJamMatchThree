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
    public string pattern = "";
    public char[] compiled;

    bool isCompiled = false;

    public void Compile()
    {
        Debug.Log(compiled);
    }
}

public static class SearchRules
{

}