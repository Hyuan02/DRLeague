using Unity.MLAgents.Actuators;

public class AgentInputSignal: IInputSignals
{

    ActionSegment<float> _currentContinousActions = ActionSegment<float>.Empty;
    ActionSegment<int> _currentDiscreteActions = ActionSegment<int>.Empty;

    public void UpdateSignals(ActionBuffers currentBuffer)
    {
        _currentContinousActions = currentBuffer.ContinuousActions;
        _currentDiscreteActions = currentBuffer.DiscreteActions;
    }

    public float GetForwardSignal() => _currentContinousActions.Length > 0 ? _currentContinousActions[0] : 0;
    public float GetTurnSignal() => _currentContinousActions.Length > 1 ? _currentContinousActions[1] : 0;
    public bool GetJumpSignal() => _currentDiscreteActions.Length > 0 ? (_currentDiscreteActions[0] > 0 ? true : false) : false;
    public bool GetBoostSignal() => _currentDiscreteActions.Length > 1 ? (_currentDiscreteActions[1] > 0 ? true : false) : false;
    public bool GetDriftSignal() => _currentDiscreteActions.Length > 2 ? (_currentDiscreteActions[2] > 0 ? true : false) : false;
}
