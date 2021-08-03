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
            sensor.AddObservation(_carInstance.stats.isCanDrive); // 0 ou 1
            sensor.AddObservation(_carInstance.stats.isBodySurface); // 0 ou 1
            sensor.AddObservation(_carInstance.stats.wheelsSurface/4); // 0 ate 4 
           // sensor.AddObservation(_carInstance.canMove); // 0 ou 1
            ///car jump stats
            //sensor.AddObservation(_carInstance.stats.canFirstJump);
            //sensor.AddObservation(_carInstance.stats.canKeepJumping);
            sensor.AddObservation(_carInstance.stats.isJumping); // 0 ou 1 
            //sensor.AddObservation(_carInstance.stats.canDoubleJump);
            //sensor.AddObservation(_carInstance.stats.hasDoubleJump);
            //car boost stats
            sensor.AddObservation(_carInstance.stats.boostQuantity/100f); // 0 a 100
            sensor.AddObservation(_carInstance.stats.isBoosting); // 0 a 1 
            //car move stats
            sensor.AddObservation((_carInstance.stats.forwardSpeedSign + 1)/2); 
            sensor.AddObservation((_carInstance.stats.forwardSpeed)/(Constants.Instance.MaxSpeed));
            sensor.AddObservation((_carInstance.stats.currentSteerAngle)/34.5f); // -34.5 a 34.5

            ///car transform stats
            sensor.AddObservation(_carInstance.transform.localEulerAngles/180.0f - Vector3.one);
        }

        if (_ballInstance)
        {

            ///ball related to agent
            sensor.AddObservation((_carInstance.transform.position - _ballInstance.transform.position)/65f);

            sensor.AddObservation(_ballInstance.BallVelocity.normalized);
            sensor.AddOneHotObservation((int)_ballInstance.State, 2);

        }

        if (_goalpost)
        {
            ///ball related to goal
            sensor.AddObservation((_goalpost.transform.position - _ballInstance.transform.position)/65f);
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
