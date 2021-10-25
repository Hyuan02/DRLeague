using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Penalty2Players : RuleManager, IPenaltyInteractions
{
    [SerializeField]
    List<CarManager> _carAgents = new List<CarManager>();

    [SerializeField]
    BallManager _ball;

    [SerializeField]
    CinemachineVirtualCamera camera;

    uint[] goalscore = new uint[2];

    TeamInfo currentTeamInfo;

    int playerIndex = 0;



    [Header("Car Random Atributtes")]
    [SerializeField]
    private float minCarXRange = 30.0f;
    [SerializeField]
    private float maxCarXRange = 35.0f;
    [SerializeField]
    private float minCarZRange = 0f;
    [SerializeField]
    private float maxCarZRange = 5f;

    [Header("Ball Random Atributtes")]
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
        this.ChoosePlayer();
        RandomizeCarPosition();
        RandomizeBallPosition();
        onGameStarted?.Invoke();
    }

    public override void EndCondition()
    {
        onGameFinished?.Invoke();
    }

    void ReceiveGoal(TeamInfo info, GoalInfo goal)
    {
        Debug.Log("GOAAAAAAAAAAAL!");
        goalscore[(int)currentTeamInfo.team] += 1;
        Debug.Log(goalscore[(int)currentTeamInfo.team]);
        StartCondition();
    }

    void RandomizeCarPosition()
    {
        _carAgents[playerIndex].ResetCarState();
        Vector3 newPosition = GenerateRandomCarPosition();
        newPosition.y = -7.1f;
        _carAgents[playerIndex].SetToPositionAndRotation(newPosition, Quaternion.Euler(0, 90, 0));
        _carAgents[playerIndex].canMove = true;
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
        _carAgents[playerIndex].canMove = false;
    }

    public void OnStoppedBall()
    {
        EndCondition();
        StartCondition();
    }
    public void ChoosePlayer()
    {
        _carAgents[playerIndex].gameObject.SetActive(false);
        playerIndex++;
        if (playerIndex > _carAgents.Count - 1)
        {
            playerIndex = 0;
        }
        _carAgents[playerIndex].gameObject.SetActive(true);
        this.SetCamera(_carAgents[playerIndex].gameObject.transform);
        currentTeamInfo = _carAgents[playerIndex].info;

    }

    void SetCamera(Transform toSet)
    {
        if (!camera)
            return;

        camera.m_Follow = toSet;
        camera.m_LookAt = toSet;
    }

    private Vector3 GenerateRandomCarPosition() => new Vector3(minCarXRange, 0, UnityEngine.Random.Range(minCarZRange, maxCarZRange));


    private Vector3 GenerateRandomBallPosition() => new Vector3(0, 0, UnityEngine.Random.Range(minBallZRange, maxBallZRange));
}