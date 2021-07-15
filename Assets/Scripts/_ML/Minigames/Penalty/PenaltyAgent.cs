using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PenaltyAgent : Agent, IInputSignals
{

    [SerializeField]
    RuleManager _gameManager;
    [SerializeField]
    CarManager _carInstance;
    [SerializeField]
    BallManager _ballInstance;

    ActionSegment<float> currentContinousActions = ActionSegment<float>.Empty;
    ActionSegment<int> currentDiscreteActions = ActionSegment<int>.Empty;


    void Start()
    {
        _gameManager.onGoalHappened += RewardCondition;
        _gameManager.onGameStarted += StartRoutine;
        _gameManager.onGameFinished += BadEndRoutine;
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        currentContinousActions = actionBuffers.ContinuousActions;
        currentDiscreteActions = actionBuffers.DiscreteActions;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        TransmitObservations(sensor);
    }

    #region INPUTS_IMPLEMENTATIONS
    public float GetForwardSignal() => currentContinousActions.Length > 0? currentContinousActions[0] : 0;
    public float GetTurnSignal() => currentContinousActions.Length > 0? currentContinousActions[1] : 0;
    public bool GetJumpSignal() => currentDiscreteActions.Length > 0? (currentDiscreteActions[0] > 0?  true : false) : false;
    public bool GetHeldJumpSignal() => currentDiscreteActions.Length > 0 ? (currentDiscreteActions[1] > 0 ? true : false) : false;
    public bool GetBoostSignal() => currentDiscreteActions.Length > 0 ? (currentDiscreteActions[2] > 0 ? true : false) : false;
    #endregion


    private void TransmitObservations(VectorSensor sensor)
    {
        if (_carInstance)
        {
            //car spacial stats 
            sensor.AddObservation(_carInstance.stats.isCanDrive);
            sensor.AddObservation(_carInstance.stats.isBodySurface);
            sensor.AddObservation(_carInstance.stats.isAllWheelsSurface);
            sensor.AddObservation(_carInstance.stats.wheelsSurface);
            sensor.AddObservation(_carInstance.canMove);
            //car jump stats
            sensor.AddObservation(_carInstance.stats.canFirstJump);
            sensor.AddObservation(_carInstance.stats.canKeepJumping);
            sensor.AddObservation(_carInstance.stats.isJumping);
            //car boost stats
            sensor.AddObservation(_carInstance.stats.boostQuantity);
            //car move stats
            sensor.AddObservation(_carInstance.stats.forwardAcceleration);
            sensor.AddObservation(_carInstance.stats.forwardSpeed);
            sensor.AddObservation(_carInstance.stats.currentSteerAngle);

            //car transform stats
            sensor.AddObservation(_carInstance.transform.localPosition);
            sensor.AddObservation(_carInstance.transform.localRotation);
        }

        if (_ballInstance)
        {
            sensor.AddObservation(_ballInstance.transform.localPosition);
        }
    }

    void RewardCondition(TeamInfo team, GoalInfo goal)
    {
        this.AddReward(1000f);
        this.EndEpisode();
    }

    void StartRoutine()
    {
        OnEpisodeBegin();
    }

    void BadEndRoutine()
    {
        this.AddReward(-1f);
    }
}
