using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SignalClient))]
public abstract class InputSignalSetup: MonoBehaviour
{
    private SignalClient _clientToSetup;

    private void Awake()
    {
        _clientToSetup = GetComponent<SignalClient>();
        SetupInputSignals(_clientToSetup);
    }
    protected abstract void SetupInputSignals(SignalClient clientToSetup);
}
