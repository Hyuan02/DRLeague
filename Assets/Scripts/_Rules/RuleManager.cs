using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RuleManager : MonoBehaviour
{
    public Action<TeamInfo, GoalInfo> onGoalHappened;
    public Action onGameFinished, onGameStarted;


    public bool _mlMode = false;

    public abstract void StartCondition();
    public abstract void EndCondition();
}
