using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    [SerializeField]
    private static float _maxAngularVelocity = 5.5f;

    public static float MaxAngularVelocity => _maxAngularVelocity;

    [SerializeField]
    private float _normalLength = 0.95f;

    public float NormalLength => _normalLength;


    public readonly float MaxSpeed = 14.1f;
    public readonly float IntermediateSpeed = 14.0f;
    public readonly float MaxBoostSpeed = 23f;

    public const float BREAK_STRENGTH = 35;

    public const float BrakeAcceleration = 5.25f;

    public const float wheelsRevolutionSpeed = 13f;


    //boost
    public const float BoostForce = 9.91f;

    public const float BoostForceMultiplier = 1f;

    //particles
    public const int SupersonicThreshold = 22;


    //Jumping
    public const float JumpForceMultiplier = 1f;
    public const int UpForce = 3;
    public const int UpTorque = 50;

    public const float InitalJumpTorque = 2.92f;
    public const float MidJumpTorque = 14.58f;


    //AIR
    public const float Torque_Roll = 36.07956616966136f;
    public const float Torque_Pitch = 12.14599781908070f;
    public const float Torque_Yaw = 8.91962804287785f;

    public const float Drag_Roll = -4.47166302201591f;
    public const float Drag_Pitch = -2.798194258050845f;
    public const float Drag_Yaw = -1.886491900437232f;


    public static Constants Instance;

    private void Awake()
    {
        Instance = this;
    }

}
