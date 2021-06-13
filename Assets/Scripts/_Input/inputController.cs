using UnityEngine;

public static class InputController
{
    public static float forwardInput =>
        Input.GetAxis("Vertical");
    public static float turnInput =>
        Input.GetAxis("Horizontal");
    public static bool boostInput =>
        Input.GetMouseButton(0);
}
