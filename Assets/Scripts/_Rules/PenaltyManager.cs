using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PenaltyManager : RuleManager
{
    [SerializeField]
    CarManager _carAgent;

    [SerializeField]
    BallManager _ball;

    void Start()
    {
        onGoalHappened += ReceiveGoal;
    }

    void Update()
    {
        
    }

    
    public override void StartCondition()
    {
        RandomizeCarPosition();
        RandomizeBallPosition();
    }

    public override void EndCondition()
    {
        _carAgent.canMove = false;
    }

    void ReceiveGoal(TeamInfo info, GoalInfo goal)
    {
        Debug.Log("GOAAAAAAAAAAAL!");
        StartCondition();
    }

    void RandomizeCarPosition()
    {
        _carAgent.ResetCarState();
        _carAgent.canMove = true;
    }

    void RandomizeBallPosition()
    {
        _ball.ResetBall();
    }

    public void OnTouchedBall()
    {
        EndCondition();
    }

    public void OnStoppedBall()
    {
        StartCondition();
    }
}
