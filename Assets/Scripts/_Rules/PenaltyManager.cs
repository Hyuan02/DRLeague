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



    void Start()
    {
        onGoalHappened += ReceiveGoal;
        StartCondition();
    }

    
    public override void StartCondition()
    {
        RandomizeCarPosition();
        RandomizeBallPosition();
    }

    public override void EndCondition()
    {
        onGameFinished?.Invoke();
    }

    void ReceiveGoal(TeamInfo info, GoalInfo goal)
    {
        Debug.Log("GOAAAAAAAAAAAL!");
        StartCondition();
    }

    void RandomizeCarPosition()
    {
        _carAgent.ResetCarState();
        Vector3 newPosition = GenerateRandomCarPosition();
        newPosition.y = -7.1f;
        _carAgent.SetToPositionAndRotation(newPosition, Quaternion.Euler(0,90,0));
        _carAgent.canMove = true;
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
        _carAgent.canMove = false;
    }

    public void OnStoppedBall()
    {
        EndCondition();
        StartCondition();
    }

    private Vector3 GenerateRandomCarPosition() => new Vector3(UnityEngine.Random.Range(minCarXRange, maxCarXRange), 0, UnityEngine.Random.Range(minCarZRange, maxCarZRange));


    private Vector3 GenerateRandomBallPosition() => new Vector3(0, 0, UnityEngine.Random.Range(minBallZRange, maxBallZRange));
}
