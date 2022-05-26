using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using Unity.MLAgents;

public class PenaltyCompetitive : RuleManager, IPenaltyInteractions
{
    [SerializeField]
    List<CarManager> _carAgents = new List<CarManager>();

    private SimpleMultiAgentGroup blueAgents;
    private SimpleMultiAgentGroup redAgents;

    [SerializeField]
    BallManager _ball;

    [SerializeField]
    new CinemachineVirtualCamera camera;

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
    
    [SerializeField]
    private float minCarZRange2 = 0f;
    [SerializeField]
    private float maxCarZRange2 = 5f;

    [Header("Ball Random Atributtes")]
    [SerializeField]
    private float minBallZRange = -5f;
    [SerializeField]
    private float maxBallZRange = 5f;


    [SerializeField]
    float timeToWaitBeforeRestart = 15f;
    float _timeWaitedToRestart = 0f;

    void Start()
    {
        RegisterTeams();
        onGoalHappened += ReceiveGoal;
        StartCondition();
    }


    void FixedUpdate()
    {
        CountTimeToRestart();
    }

    void RegisterTeams()
    {
        blueAgents = new SimpleMultiAgentGroup();
        redAgents = new SimpleMultiAgentGroup();

        foreach(CarManager a in _carAgents)
        {
            Agent agentRef = a.GetComponent<Agent>();
            if (a.info.team == Teams.RED)
            {
                if(agentRef)
                    redAgents.RegisterAgent(agentRef);
            }
            else
            {
                if(agentRef)
                    blueAgents.RegisterAgent(agentRef);
            }
        }
    }


    public override void StartCondition()
    {
        //this.ChoosePlayer();
        RandomizeCarPosition();
        RandomizeBallPosition();
        onGameStarted?.Invoke();
        _timeWaitedToRestart = 0;
    }

    public override void EndCondition()
    {
        onGameFinished?.Invoke();
        AttributeBadReward();
        
    }


    void AttributeBadReward()
    {
        blueAgents.AddGroupReward(-5f);
        redAgents.AddGroupReward(-5f);
        blueAgents.EndGroupEpisode();
        redAgents.EndGroupEpisode();
    }

    protected override void ReceiveGoal(TeamInfo info, GoalInfo goal)
    {
        Debug.Log("GOAAAAAAAAAAAL!");
        goalscore[(int)info.team] += 1;
        Debug.Log(goalscore[(int)info.team]);
        AttributeScoreReward(info);
        blueAgents.EndGroupEpisode();
        redAgents.EndGroupEpisode();
        StartCondition();
    }

    void AttributeScoreReward(TeamInfo info)
    {
        if (info.team == Teams.BLUE)
        {
            blueAgents.AddGroupReward(10.0f);
            foreach (Agent a in blueAgents.GetRegisteredAgents()) {
                a.AddReward(10.0f);
            }

            redAgents.AddGroupReward(-10.0f);
            foreach (Agent a in redAgents.GetRegisteredAgents())
            {
                a.AddReward(-10.0f);
            }
        }
        else
        {
            blueAgents.AddGroupReward(-10.0f);
            foreach (Agent a in blueAgents.GetRegisteredAgents())
            {
                a.AddReward(-10.0f);
            }
            redAgents.AddGroupReward(10.0f);
            foreach (Agent a in redAgents.GetRegisteredAgents())
            {
                a.AddReward(10.0f);
            }
        }
    }

    void RandomizeCarPosition()
    {

        _carAgents.ForEach(e =>
        {
            int index = 0;
            e.ResetCarState();
            e.signalClient.CanEmitSignals = true;
            Vector3 newPosition = GenerateRandomCarPosition(index);
            newPosition.y = -7.1f;
            e.SetToPositionAndRotation(newPosition, Quaternion.Euler(0, 90, 0));
            index++;
        });
        
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
        _carAgents.ForEach(e=>e.signalClient.CanEmitSignals = false);
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

    private Vector3 GenerateRandomCarPosition(int index) {
        
        
        return new Vector3(minCarXRange, 0, UnityEngine.Random.Range(index > 0 ? minCarZRange : minCarZRange2, index > 0 ? maxCarZRange : maxCarZRange2));
            }


    private Vector3 GenerateRandomBallPosition() => new Vector3(0, 0, UnityEngine.Random.Range(minBallZRange, maxBallZRange));


    private void CountTimeToRestart()
    {
        _timeWaitedToRestart += Time.fixedDeltaTime;
        if (_timeWaitedToRestart >= timeToWaitBeforeRestart)
        {
            EndCondition();
            StartCondition();
        }

    }

    protected override void StartRoutine()
    {
        throw new NotImplementedException();
    }
}
