using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AerialAgent : Agent, IInputSignals
{
    [SerializeField]
    AerialManager _gameManager;
    [SerializeField]
    CarManager _carInstance;
    [SerializeField]
    BallManager _ballInstance;
    [SerializeField]
    AerialWatcher _watcher;
    [SerializeField]
    AerialInteractor _aerialInteractor;
    [SerializeField]
    Transform _goalpost;
    

    ActionSegment<float> currentContinousActions = ActionSegment<float>.Empty;
    ActionSegment<int> currentDiscreteActions = ActionSegment<int>.Empty;



    void Start()
    {
        _gameManager.onValidGoal += RewardCondition;
        _gameManager.onGameStarted += StartRoutine;
        _gameManager.onGameFinished += BadEndRoutine;
    }

    void FixedUpdate()
    {
    }

    public override void OnEpisodeBegin()
    {
        
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
            ///car spacial stats 
            sensor.AddObservation(_carInstance.stats.isCanDrive);
            sensor.AddObservation(_carInstance.stats.isBodySurface);
            sensor.AddObservation(_carInstance.stats.wheelsSurface);
            sensor.AddObservation(_carInstance.canMove);
            ///car jump stats
            //sensor.AddObservation(_carInstance.stats.canFirstJump);
            //sensor.AddObservation(_carInstance.stats.canKeepJumping);
            sensor.AddObservation(_carInstance.stats.isJumping);
            //sensor.AddObservation(_carInstance.stats.canDoubleJump);
            //sensor.AddObservation(_carInstance.stats.hasDoubleJump);
            //car boost stats
            sensor.AddObservation(_carInstance.stats.boostQuantity);
            sensor.AddObservation(_carInstance.stats.isBoosting);
            //car move stats
            sensor.AddObservation(_carInstance.stats.forwardSpeedSign);
            sensor.AddObservation((_carInstance.stats.forwardSpeedAbs - 0)/(Constants.Instance.MaxSpeed));
            sensor.AddObservation(_carInstance.stats.currentSteerAngle);

            ///car transform stats
            sensor.AddObservation(_carInstance.transform.localEulerAngles/360.0f);
        }

        if (_ballInstance)
        {

            ///ball related to agent
            sensor.AddObservation(_carInstance.transform.position - _ballInstance.transform.position);

            sensor.AddObservation(_ballInstance.BallVelocity);
            sensor.AddOneHotObservation((int)_ballInstance.State, 2);

        }

        if (_goalpost)
        {
            ///ball related to goal
            sensor.AddObservation(_goalpost.transform.position - _ballInstance.transform.position);
        }

        if (_watcher)
            sensor.AddObservation(_watcher.aerialMade);

        if (_aerialInteractor)
        {
            sensor.AddOneHotObservation((int)_aerialInteractor.state, 3);
        }
    }

    void RewardCondition()
    {
        Debug.Log("Goaaaaaaaaal!");
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


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);

        var continousActions = actionsOut.ContinuousActions;

        continousActions[0] = InputController.forwardInput;
        continousActions[1] = InputController.turnInput;

        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = InputController.jumpInput? 1 : 0;
        discreteActions[1] = InputController.boostInput ? 1 : 0;
        discreteActions[2] = InputController.GetDriftInput ? 1 : 0;
    }

}
