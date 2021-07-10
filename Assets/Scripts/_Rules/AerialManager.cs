using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialManager : RuleManager
{

    [SerializeField]
    CarManager _carInstance;

    [SerializeField]
    BallManager _ballInstance;

    [Header("Ball Params")]
    [SerializeField]
    float ballZMin = -5f;
    [SerializeField]
    float ballZMax = -5f;
    [SerializeField]
    float timeToWaitBeforeDrop = 2f;
    float _timeWaited = 0f;

    [SerializeField]
    float timeToWaitBeforeRestart = 10f;
    float _timeWaitedToRestart = 0f;

    bool _waitingToDrop = false;
    bool _waitingToRestart = false;


    private void Start()
    {
        StartCondition();
        onGoalHappened += ReceiveGoal;
    }

    private void FixedUpdate()
    {
        CountTimeToDrop();
        CountTimeToRestart();
    }


    public override void StartCondition()
    {
        Debug.Log("Restart");
        _carInstance.ResetCarState();
        _carInstance.SetToPositionAndRotation(null,Quaternion.Euler(0, 90, 0));
        _ballInstance.ResetBall();
        RestartTimeCounter();
        ThrowBall();
        _ballInstance.FreezeBall();
        _waitingToDrop = true;
        
    }

    public override void EndCondition()
    {
        throw new System.NotImplementedException();
    }

    private void ThrowBall()
    {
        Vector3 newPosition = GenerateBallAirPosition();
        _ballInstance.transform.localPosition = newPosition;
        _ballInstance.FreezeBall();
    }

    void ReceiveGoal(TeamInfo info, GoalInfo goal)
    {
        Debug.Log("GOAAAAAAAAAAAL!");
        StartCondition();
    }


    private void CountTimeToDrop()
    {
        if (_waitingToDrop)
        {
            _timeWaited += Time.fixedDeltaTime;
            if(_timeWaited > timeToWaitBeforeDrop)
            {
                _timeWaited = 0;
                _ballInstance.UnFreezeBall();
                _waitingToDrop = false;
                _waitingToRestart = true;
            }
        }
    }

    private void CountTimeToRestart()
    {
        if (_waitingToRestart)
        {
            _timeWaitedToRestart += Time.fixedDeltaTime;
            if(_timeWaitedToRestart >= timeToWaitBeforeRestart)
            {
                StartCondition();
            }
        }
    }

    private void RestartTimeCounter()
    {
        _timeWaitedToRestart = 0;
        _waitingToRestart = false;
        _waitingToDrop = false;
        _timeWaited = 0;
    }

    public void OnStoppedBall()
    {
        StartCondition();
    }
    private Vector3 GenerateBallAirPosition() => new Vector3(26, -0.5f, Random.Range(ballZMin, ballZMax));
}
