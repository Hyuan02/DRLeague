using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const float TIME_ON_GAME = 300f;

    public GameStats mainStats;

    public Action<TeamInfo, GoalInfo> onGoalHappened;

    private void Awake()
    {
        mainStats.TimeSpent = TIME_ON_GAME;
        mainStats.goalScore = new uint[Enum.GetNames(typeof(Teams)).Length];
        onGoalHappened += UpdateTeamScore;
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

    private void UpdateTeamScore(TeamInfo info, GoalInfo goal) {
        mainStats.goalScore[(int)goal.team] += 1;
    }


}

public struct GameStats {
    public float TimeSpent { get; set; }
    public uint[] goalScore;
}