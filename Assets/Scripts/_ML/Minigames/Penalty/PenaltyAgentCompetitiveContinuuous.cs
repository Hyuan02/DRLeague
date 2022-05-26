using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PenaltyAgentCompetitiveContinuuous : CarAgent
{

    [SerializeField]
    RuleManager _gameManager;
    [SerializeField]
    CarManager _carInstance;
    [SerializeField]
    BallManager _ballInstance;
    [SerializeField]
    Transform _goalpost;

    float previousDistance = float.MaxValue;
    
    void Start()
    {
        _gameManager.onGameStarted += StartRoutine;
    }

    public override void OnEpisodeBegin()
    {
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        InputSignal.UpdateSignals(actionBuffers);
        var actualDistance = ExtractDistanceOfPoints();
        if (previousDistance > actualDistance)
        {
            this.AddReward(0.05f);
        }
        previousDistance = actualDistance;
        this.AddReward(-0.01f);
    }

    public override void TransmitObservations(VectorSensor sensor)
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
            sensor.AddObservation(_carInstance.transform.position - _ballInstance.transform.position);
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

    void TouchBall() {
        this.AddReward(0.2f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        var continousActions = actionsOut.ContinuousActions;

        continousActions[0] = InputController.forwardInput;
        continousActions[1] = InputController.turnInput;

        var discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = InputController.jumpInput ? 1 : 0;
        discreteActions[1] = InputController.boostInput ? 1 : 0;
        discreteActions[2] = InputController.GetDriftInput ? 1 : 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BallInteractor"))
        {
            TouchBall();
        }
    }

    public float ExtractDistanceOfPoints()
    {
        return Vector3.Distance(_ballInstance.transform.position, _goalpost.position);
    }

}
