using UnityEngine;

public static class InputController
{
    public static float forwardInput =>
        Input.GetAxis("Vertical");
    public static float turnInput =>
        Input.GetAxis("Horizontal");
    public static bool boostInput =>
        Input.GetMouseButton(0);
    public static bool jumpInput =>
        Input.GetMouseButton(1);
    public static bool HeldJumpInput =>
        Input.GetMouseButton(1);

    public static bool GetDriftInput =>
        Input.GetKey(KeyCode.Space);
}
