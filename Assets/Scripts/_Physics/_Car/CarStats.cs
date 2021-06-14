using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CarStats
{
    public float forwardSpeed;
    public float forwardSpeedAbs => Mathf.Abs(forwardSpeed);
    public float forwardSpeedSign => Mathf.Sign(forwardSpeed);
    public float forwardAcceleration;

    public int wheelsSurface;

    public bool isBodySurface;
    public bool isCanDrive;
    public bool isAllWheelsSurface => wheelsSurface > 3;


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

}