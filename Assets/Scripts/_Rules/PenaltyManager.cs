using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PenaltyManager : RuleManager, IPenaltyInteractions
{
    [SerializeField]
    CarManager _carAgent;

    [SerializeField]
    BallManager _ball;

    [SerializeField]
    bool stopMove = true;



    [Header ("Car Random Atributtes")]
    [SerializeField]
    private float minCarXRange = 30.0f;
    [SerializeField]
    private float maxCarXRange = 35.0f;
    [SerializeField]
    private float minCarZRange = 0f;
    [SerializeField]
    private float maxCarZRange = 5f;
    
    [Header ("Ball Random Atributtes")]
    [SerializeField]
    private float minBallZRange = -5f;
    [SerializeField]
    private float maxBallZRange = 5f;



    protected override void StartRoutine()
    {
        onGoalHappened += ReceiveGoal;
        StartCondition();
    }

    
    public override void StartCondition()
    {
        base.StartCondition();
        RandomizeCarPosition();
        RandomizeBallPosition();
        onGameStarted?.Invoke();
    }

    public override void EndCondition()
    {
        onGameFinished?.Invoke();
    }

    protected override void ReceiveGoal(TeamInfo info, GoalInfo goal)
    {
        Debug.Log("GOAAAAAAAAAAAL!");
        StartCondition();
    }

    void RandomizeCarPosition()
    {
        _carAgent.ResetCarState();
        Vector3 newPosition = GenerateRandomCarPosition();
        newPosition.y = -7.1f;
        _carAgent.SetToPositionAndRotation(newPosition, null);
        _carAgent.signalClient.CanEmitSignals = true;
    }

    void RandomizeBallPosition()
    {
        _ball.ResetBall();
        Vector3 newPosition = GenerateRandomBallPosition();
        newPosition.x = _ball.transform.localPosition.x;
        newPosition.y = _ball.transform.localPosition.y;
        _ball.SetBallOnPosition(newPosition);
    }

    public void OnTouchedBall()
    {
        if(stopMove)
            _carAgent.signalClient.CanEmitSignals = false;
    }

    public void OnStoppedBall()
    {
        EndCondition();
        StartCondition();
    }

    private Vector3 GenerateRandomCarPosition() => new Vector3(minCarXRange, 0, UnityEngine.Random.Range(minCarZRange, maxCarZRange));


    private Vector3 GenerateRandomBallPosition() => new Vector3(0, 0, UnityEngine.Random.Range(minBallZRange, maxBallZRange));
}
