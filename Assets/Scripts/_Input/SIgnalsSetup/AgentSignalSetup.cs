using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class AgentSignalSetup : InputSignalSetup
{
    [SerializeField]
    CarAgent _agentToDeliver;

    [SerializeField]
    CarManager _managerToDeliver;

    protected override void SetupInputSignals()
    {
        AgentInputSignal newSignal = new AgentInputSignal();
        _agentToDeliver.InputSignal = newSignal;
        _managerToDeliver.SetControllerInterface(newSignal);
        
    }
}
