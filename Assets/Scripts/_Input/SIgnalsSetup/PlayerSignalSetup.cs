using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSignalSetup: InputSignalSetup
{
    protected override void SetupInputSignals(SignalClient signalToSetup)
    {
        signalToSetup.SetControllerInterface(new PlayerInputSignal());
    }

}
