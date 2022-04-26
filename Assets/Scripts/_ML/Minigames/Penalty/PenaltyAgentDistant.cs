using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PenaltyAgentDistant : Agent, IInputSignals
{

    [SerializeField]
    RuleManager _gameManager;
    [SerializeField]
    CarManager _carInstance;
    [SerializeField]
    BallManager _ballInstance;
    [SerializeField]
    Transform _goalpost;

    ActionSegment<float> currentContinousActions = ActionSegment<float>.Empty;
    ActionSegment<int> currentDiscreteActions = ActionSegment<int>.Empty;

    [SerializeField]
    float timeToWaitBeforeRestart = 15f;
    float _timeWaitedToRestart = 0f;
    float previousDistance = float.MaxValue;



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
        var actualDistance = ExtractDistanceOfPoints();
        if (previousDistance > actualDistance) {
            this.AddReward(0.05f);
        }
        previousDistance = actualDistance;


        this.AddReward(-0.01f);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        TransmitObservations(sensor);
    }

    #region INPUTS_IMPLEMENTATIONS
    public float GetForwardSignal() => currentContinousActions.Length > 0? currentContinousActions[0] : 0;
    public float GetTurnSignal() => currentContinousActions.Length > 0? currentContinousActions[1] : 0;
    public bool GetJumpSignal() => currentDiscreteActions.Length > 0? (currentDiscreteActions[0] > 0?  true : false) : false;
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
            //sensor.AddObservation(_carInstance.stats.isAllWheelsSurface);
            sensor.AddObservation(_carInstance.stats.wheelsSurface);
            //sensor.AddObservation(_carInstance.canMove);
            //car jump stats
            //sensor.AddObservation(_carInstance.stats.canFirstJump);
            //sensor.AddObservation(_carInstance.stats.canKeepJumping);
            sensor.AddObservation(_carInstance.stats.isJumping);
            //car boost stats
            sensor.AddObservation(_carInstance.stats.boostQuantity);
            sensor.AddObservation(_carInstance.stats.isBoosting);
            //car move stats
            sensor.AddObservation(_carInstance.stats.forwardSpeedSign);
            sensor.AddObservation((_carInstance.stats.forwardSpeedAbs - 0) / (_carInstance.carData.MaxSpeed));
            //sensor.AddObservation(_carInstance.stats.forwardSpeed);
            sensor.AddObservation(_carInstance.stats.currentSteerAngle);

            //car transform stats
            //sensor.AddObservation(_carInstance.transform.localPosition);
            sensor.AddObservation(_carInstance.transform.eulerAngles / 360.0f);
        }

        if (_ballInstance)
        {
            sensor.AddObservation(Vector3.Distance(_carInstance.transform.position, _ballInstance.transform.position));
            //sensor.AddObservation(_carInstance.transform.position - _ballInstance.transform.position);
        }

        if (_goalpost)
        {
            sensor.AddObservation(Vector3.Distance(_ballInstance.transform.position, _goalpost.position));
            //sensor.AddObservation(_ballInstance.transform.position - _goalpost.position);
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

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);

        var continousActions = actionsOut.ContinuousActions;

        continousActions[0] = InputController.forwardInput;
        continousActions[1] = InputController.turnInput;

        var discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = InputController.jumpInput ? 1 : 0;
        discreteActions[1] = InputController.boostInput ? 1 : 0;
        discreteActions[2] = InputController.GetDriftInput ? 1 : 0;
    }


    public float ExtractDistanceOfPoints() {
        return Vector3.Distance(_ballInstance.transform.position, _goalpost.position);
    }
}
