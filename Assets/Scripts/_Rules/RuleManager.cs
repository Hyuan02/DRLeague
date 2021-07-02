using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RuleManager : MonoBehaviour
{
    public Action<TeamInfo, GoalInfo> onGoalHappened;

    public abstract void StartCondition();
    public abstract void EndCondition();
}
