using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class AgentSignalSetup : InputSignalSetup
{
    [SerializeField]
    CarAgent _agentToDeliver;

    protected override void SetupInputSignals(SignalClient clientToSetup)
    {
        AgentInputSignal newSignal = new AgentInputSignal();
        clientToSetup.SetControllerInterface(newSignal);
        _agentToDeliver.InputSignal = newSignal;
    }
}
