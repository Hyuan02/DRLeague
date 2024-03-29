using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CarStats
{
    [Header("General")]
    public int wheelsSurface;
    public bool isBodySurface;
    public bool isCanDrive;
    public bool isAllWheelsSurface => wheelsSurface > 3;
    public CarState CarState;
    
    [Header("Speed")]
    public float forwardSpeed;
    public float forwardSpeedAbs => Mathf.Abs(forwardSpeed);
    public float forwardSpeedSign => Mathf.Sign(forwardSpeed);
    public float forwardAcceleration;

    [Header("Drift")]
    public float driftTime;
    public float currentWheelSideFriction;
    public float wheelSideFriction;
    public float wheelSideFrictionDrift;

    [Header("Steer")]
    public float turnRadiusCoefficient;
    public float currentSteerAngle;

    [Header("Jump")]
    public bool canFirstJump;
    public bool isJumping;
    public bool canKeepJumping;
    public bool canDoubleJump;
    public bool hasDoubleJump;

    [Header("Boost")]
    public float boostQuantity;
    public bool isBoosting;

}