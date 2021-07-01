using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TeamInfo {
    public Teams team;
    public uint id;
}

[System.Serializable]
public struct GoalInfo
{
    public Teams team;
}

public enum Teams 
{
   RED,
   BLUE
}