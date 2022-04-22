using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputSignalSetup: MonoBehaviour
{
    private void Awake()
    {
        SetupInputSignals();
    }
    protected abstract void SetupInputSignals();
}
