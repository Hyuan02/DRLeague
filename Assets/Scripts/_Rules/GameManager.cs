using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : RuleManager
{
    const float TIME_ON_GAME = 300f;

    public GameStats mainStats;

    private void Awake()
    {
        StartCondition();
    }

    private void LateUpdate()
    {
        DecrementTime();
    }

    private void DecrementTime()
    {
        if (mainStats.TimeSpent > 0)
            mainStats.TimeSpent -= Time.unscaledDeltaTime;
    }

    protected override void ReceiveGoal(TeamInfo info, GoalInfo goal) {
        mainStats.goalScore[(int)goal.team] += 1;
    }

    public override void StartCondition()
    {
        mainStats.TimeSpent = TIME_ON_GAME;
        mainStats.goalScore = new uint[Enum.GetNames(typeof(Teams)).Length];
        onGoalHappened += ReceiveGoal;
    }

    public override void EndCondition()
    {
        throw new NotImplementedException();
    }

    protected override void StartRoutine()
    {
        throw new NotImplementedException();
    }
}

public struct GameStats {
    public float TimeSpent { get; set; }
    public uint[] goalScore;
}