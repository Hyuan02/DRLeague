using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PenaltyAgent : CarAgent
{

    [SerializeField]
    RuleManager _gameManager;

    void Start()
    {
        _gameManager.onGoalHappened += RewardCondition;
        _gameManager.onGameStarted += StartRoutine;
        _gameManager.onGameFinished += BadEndRoutine;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Debug.Log("Getting actions!");
        base.OnActionReceived(actionBuffers);
        this.AddReward(-0.01f);
    }

    public override void TransmitObservations(VectorSensor sensor)
    {
        if (carInstance)
        {
            //car spacial stats 
            sensor.AddObservation(carInstance.stats.isCanDrive);
            sensor.AddObservation(carInstance.stats.isBodySurface);
            sensor.AddObservation(carInstance.stats.wheelsSurface);
            //car jump stats
            sensor.AddObservation(carInstance.stats.isJumping);
            //car boost stats
            sensor.AddObservation(carInstance.stats.boostQuantity);
            sensor.AddObservation(carInstance.stats.isBoosting);
            //car move stats
            sensor.AddObservation(carInstance.stats.forwardSpeedSign);
            sensor.AddObservation((carInstance.stats.forwardSpeedAbs - 0) / (+carInstance.carData.MaxSpeed));
            sensor.AddObservation(carInstance.stats.currentSteerAngle);
            //car transform stats
            sensor.AddObservation(carInstance.transform.eulerAngles / 360.0f);
        }

        if (ballInstance)
        {
            sensor.AddObservation(carInstance.transform.position - ballInstance.transform.position);
        }

        if (goalpost)
        {
            sensor.AddObservation(ballInstance.transform.position - goalpost.position);
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
}
