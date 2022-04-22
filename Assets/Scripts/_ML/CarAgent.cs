using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public abstract class CarAgent : Agent
{
    public AgentInputSignal InputSignal;


    public override void OnActionReceived(ActionBuffers actions)
    {
        InputSignal.UpdateSignals(actions);
    }
}
