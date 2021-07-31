using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BarrierAgent : Agent, IInputSignals
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


    ActionSegment<float> currentContinousActions = ActionSegment<float>.Empty;
    ActionSegment<int> currentDiscreteActions = ActionSegment<int>.Empty;

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
        currentContinousActions = actionBuffers.ContinuousActions;
        currentDiscreteActions = actionBuffers.DiscreteActions;

        this.AddReward(-0.01f);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        TransmitObservations(sensor);
    }

    #region INPUTS_IMPLEMENTATIONS
    public float GetForwardSignal() => currentContinousActions.Length > 0 ? currentContinousActions[0] : 0;
    public float GetTurnSignal() => currentContinousActions.Length > 0 ? currentContinousActions[1] : 0;
    public bool GetJumpSignal() => currentDiscreteActions.Length > 0 ? (currentDiscreteActions[0] > 0 ? true : false) : false;
    public bool GetBoostSignal() => currentDiscreteActions.Length > 0 ? (currentDiscreteActions[1] > 0 ? true : false) : false;
    public bool GetDriftSignal() => currentDiscreteActions.Length > 0 ? (currentDiscreteActions[2] > 0 ? true : false) : false;
    #endregion


    private void TransmitObservations(VectorSensor sensor)
    {
        if (_carInstance)
        {
            //car spacial stats 
            sensor.AddObservation(_carInstance.stats.isCanDrive);
            sensor.AddObservation(_carInstance.stats.isBodySurface);
            sensor.AddObservation(_carInstance.stats.wheelsSurface);
            sensor.AddObservation(_carInstance.canMove);
            //car jump stats
            sensor.AddObservation(_carInstance.stats.isJumping);
            //car boost stats
            sensor.AddObservation(_carInstance.stats.boostQuantity);
            sensor.AddObservation(_carInstance.stats.isBoosting);
            //car move stats
            sensor.AddObservation(_carInstance.stats.forwardSpeedSign);
            sensor.AddObservation((_carInstance.stats.forwardSpeedAbs - 0) / (Constants.Instance.MaxSpeed));
            sensor.AddObservation(_carInstance.stats.currentSteerAngle);

            //car transform stats
            sensor.AddObservation(_carInstance.transform.localEulerAngles/360.0f);
        }

        if (_ballInstance)
        {
            sensor.AddObservation(_carInstance.transform.position - _ballInstance.transform.position);
            sensor.AddObservation(_ballInstance.BallVelocity);
        }

        if (_barrierInstance)
        {
            sensor.AddObservation(_ballInstance.transform.position - _barrierInstance.position);
        }

        if (_goalpost)
        {
            sensor.AddObservation(_ballInstance.transform.position - _goalpost.position);
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
