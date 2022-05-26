using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public abstract class CarAgent : Agent
{
    
    public AgentInputSignal InputSignal;
    [SerializeField]
    protected CarManager carInstance;
    [SerializeField]
    protected BallManager ballInstance;
    [SerializeField]
    protected Transform goalpost;
    public override void OnActionReceived(ActionBuffers actions)
    {
        InputSignal.UpdateSignals(actions);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        TransmitObservations(sensor);
    }

    public abstract void TransmitObservations(VectorSensor sensor);
}
