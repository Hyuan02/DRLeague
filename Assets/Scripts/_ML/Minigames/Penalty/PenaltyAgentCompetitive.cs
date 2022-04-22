using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PenaltyAgentCompetitive : Agent, IInputSignals
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

    



    void Start()
    {
        //_gameManager.onGoalHappened += RewardCondition;
        _gameManager.onGameStarted += StartRoutine;
        //_gameManager.onGameFinished += BadEndRoutine;
    }

    public override void OnEpisodeBegin()
    {
        //Debug.Log("Begin episode!");
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
    public float GetForwardSignal() {
        if (currentDiscreteActions.Length > 0) {
            if (currentDiscreteActions[0] == 1)
            {
                return 0;

            }
            else if (currentDiscreteActions[0] == 2)
            {

                return 1;
            }
            else if (currentDiscreteActions[0] == 3) {

                return -1;
            }
        }
            return 0;
    
    }
    public float GetTurnSignal() {

        if (currentDiscreteActions.Length > 0) {
            if (currentDiscreteActions[1] == 1) {

                return 0;
            
            }

            else if (currentDiscreteActions[1] == 2)
            {

                return 1;

            }

            else if (currentDiscreteActions[1] == 3)
            {

                return -1;

            }
        }

        return 0;
    
    }
    public bool GetJumpSignal() => currentDiscreteActions.Length > 0? (currentDiscreteActions[2] > 0?  true : false) : false;
    public bool GetBoostSignal() => currentDiscreteActions.Length > 0 ? (currentDiscreteActions[3] > 0 ? true : false) : false;
    public bool GetDriftSignal() => currentDiscreteActions.Length > 0 ? (currentDiscreteActions[4] > 0 ? true : false) : false;
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
            sensor.AddObservation((_carInstance.stats.forwardSpeedAbs - 0) / (Constants.Instance.MaxSpeed));
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

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);

        //var continousActions = actionsOut.ContinuousActions;


        var discreteActions = actionsOut.DiscreteActions;

        if (InputController.forwardInput > 0.2)
        {
            discreteActions[0] = 2;
        }
        else if (InputController.forwardInput < -0.2)
        {
            discreteActions[0] = 3;
        }
        else {
            discreteActions[0] = 1;

        }

        if (InputController.turnInput > 0.2)
        {
            discreteActions[1] = 2;
        }
        else if (InputController.turnInput < -0.2)
        {
            discreteActions[1] = 3;
        }
        else
        {
            discreteActions[1] = 1;

        }
        discreteActions[2] = InputController.jumpInput ? 1 : 0;
        discreteActions[3] = InputController.boostInput ? 1 : 0;
        discreteActions[4] = InputController.GetDriftInput ? 1 : 0;
    }

}
