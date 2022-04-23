using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BarrierAgent : CarAgent
{
    [SerializeField]
    RuleManager _gameManager;
    [SerializeField]
    CarManager _carInstance;
    [SerializeField]
    BallManager _ballInstance;
    [SerializeField]
    Transform _barrierInstance;
    [SerializeField]
    Transform _goalpost;


   
    [SerializeField]
    float timeToWaitBeforeRestart = 15f;
    float _timeWaitedToRestart = 0f;

    void Start()
    {
        _gameManager.onGoalHappened += RewardCondition;
        _gameManager.onGameStarted += StartRoutine;
        _gameManager.onGameFinished += BadEndRoutine;
    }

    void FixedUpdate()
    {
        CountTimeToRestart();
    }

    public override void OnEpisodeBegin()
    {
        //Debug.Log("Begin episode!");
        _timeWaitedToRestart = 0;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Debug.Log("Getting actions!");
        base.OnActionReceived(actionBuffers);

        this.AddReward(-0.01f);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        TransmitObservations(sensor);
    }

    


    private void TransmitObservations(VectorSensor sensor)
    {
        if (_carInstance)
        {
            //car spacial stats 
            sensor.AddObservation(_carInstance.stats.isCanDrive);
            sensor.AddObservation(_carInstance.stats.isBodySurface);
            sensor.AddObservation(_carInstance.stats.wheelsSurface/4);
            // sensor.AddObservation(_carInstance.canMove);
            //car jump stats
            sensor.AddObservation(_carInstance.stats.isJumping);
            //car boost stats
            sensor.AddObservation(_carInstance.stats.boostQuantity/100f);
            sensor.AddObservation(_carInstance.stats.isBoosting);
            //car move stats
            sensor.AddObservation((_carInstance.stats.forwardSpeedSign + 1)/2);
            sensor.AddObservation((_carInstance.stats.forwardSpeed) / (Constants.Instance.MaxSpeed));
            sensor.AddObservation(_carInstance.stats.currentSteerAngle/34.5f);

            //car transform stats
            sensor.AddObservation(_carInstance.transform.localEulerAngles/180.0f - Vector3.one);
        }

        if (_ballInstance)
        {
            sensor.AddObservation((_carInstance.transform.position - _ballInstance.transform.position)/65f);
            sensor.AddObservation(_ballInstance.BallVelocity.normalized);
        }

        if (_barrierInstance)
        {
            sensor.AddObservation((_ballInstance.transform.position - _barrierInstance.position)/65f);
        }

        if (_goalpost)
        {
            sensor.AddObservation((_ballInstance.transform.position - _goalpost.position)/65f);
        }

    }

    void RewardCondition(TeamInfo team, GoalInfo goal)
    {
        this.AddReward(10f);
        this.EndEpisode();
    }

    void StartRoutine()
    {
        OnEpisodeBegin();
    }

    void BadEndRoutine()
    {
        this.AddReward(-10f);
        EndEpisode();
    }

    private void CountTimeToRestart()
    {
        _timeWaitedToRestart += Time.fixedDeltaTime;
        if (_timeWaitedToRestart >= timeToWaitBeforeRestart)
        {
            _gameManager.EndCondition();
            _gameManager.StartCondition();
        }

    }

}
