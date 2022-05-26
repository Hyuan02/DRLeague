using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalClient : MonoBehaviour
{
    private IInputSignals _controllerInterface;

    private bool _canControl = true;
    public bool CanEmitSignals
    {
        set
        {
            Debug.Log("Setting control");
            _canControl = value;
        }
    }

    public void SetControllerInterface(IInputSignals controller)
    {
        _controllerInterface = controller;
    }

    internal float GetForwardSignal() => _canControl ? _controllerInterface.GetForwardSignal() : 0;

    internal float GetTurnSignal() => _canControl ? _controllerInterface.GetTurnSignal() : 0;

    internal bool GetBoostSignal() => _canControl ? _controllerInterface.GetBoostSignal() : false;

    internal bool GetJumpSignal() => _canControl ? _controllerInterface.GetJumpSignal() : false;

    internal bool GetDriftSignal() => _canControl ? _controllerInterface.GetDriftSignal() : false;
}
