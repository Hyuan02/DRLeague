using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSignal : IInputSignals
{
    public float GetForwardSignal() => InputController.forwardInput;

    public float GetTurnSignal() => InputController.turnInput;

    public bool GetBoostSignal() => InputController.boostInput;
    
    public bool GetJumpSignal() => InputController.jumpInput;

    public bool GetDriftSignal() => InputController.GetDriftInput;
}
