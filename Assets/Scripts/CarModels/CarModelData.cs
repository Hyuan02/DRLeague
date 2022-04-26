using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Car Model Data")]
public class CarModelData : ScriptableObject
{
    [Header("General Settings")]
    public float WheelsRevolutionSpeed = 13f;
    
    public float NormalLength = 0.95f;
    
    [Header("Speed Settings")]
    public float MaxAngularVelocity = 5.5f;

    public float MaxSpeed = 14.1f;
    
    public float IntermediateSpeed = 14.0f;

    public float MaxBoostSpeed = 23f;

    [Header("Break Settings")]
    public float BreakStrength = 35;

    public float BrakeAcceleration = 5.25f;


    [Header("Boost Settings")]
    public float BoostForce = 9.91f;

    public float BoostForceMultiplier = 1f;

    public float InitialBoostQuantity = 100f;

    public float BoostConsumingRate = 5.0f;

    public float BoostRecoveringRate = 2.0f;

    [Header("Jump Settings")]
    public float JumpForceMultiplier = 1f;

    public int UpForce = 3;

    public int UpTorque = 50;

    public float InitalJumpTorque = 2.92f;

    public float MidJumpTorque = 14.58f;
}
