public interface IInputSignals
{
    public float GetForwardSignal();
    public float GetTurnSignal();
    public bool GetBoostSignal();
    public bool GetJumpSignal();
    public bool GetHeldJumpSignal();

    public bool GetDriftSignal();
}
