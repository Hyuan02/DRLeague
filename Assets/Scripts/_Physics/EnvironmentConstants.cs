using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentConstants : MonoBehaviour
{
   
    //particles
    public const int SupersonicThreshold = 22;

    //AIR
    public const float Torque_Roll = 36.07956616966136f;
    public const float Torque_Pitch = 12.14599781908070f;
    public const float Torque_Yaw = 8.91962804287785f;

    public const float Drag_Roll = -4.47166302201591f;
    public const float Drag_Pitch = -2.798194258050845f;
    public const float Drag_Yaw = -1.886491900437232f;


    public static EnvironmentConstants Instance;

    private void Awake()
    {
        Instance = this;
    }

}
