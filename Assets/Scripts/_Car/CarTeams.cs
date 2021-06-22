using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TeamInfo {
    public Teams team;
    public uint id;
}

public enum Teams 
{
   RED,
   BLUE
}