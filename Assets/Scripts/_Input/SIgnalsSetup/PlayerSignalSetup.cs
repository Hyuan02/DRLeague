using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSignalSetup: InputSignalSetup
{
    [SerializeField]
    private CarManager _managerToDeliver;
    protected override void SetupInputSignals()
    {
        _managerToDeliver.SetControllerInterface(new PlayerInputSignal());
    }

}
