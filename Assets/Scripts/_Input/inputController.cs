using UnityEngine;

public static class InputController
{
    public static float forwardController =>
        Input.GetAxis("Vertical");
    public static float turnController =>
        Input.GetAxis("Horizontal");
}
