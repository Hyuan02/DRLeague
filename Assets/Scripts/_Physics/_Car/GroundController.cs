using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rocket.Utils;


[RequireComponent(typeof(CarManager))]
public class GroundController : MonoBehaviour
{

    private Rigidbody _rBody;
    private CarManager _instance;
    private WheelController[] _wheels;

    private void Start()
    {
        _instance = this.GetComponent<CarManager>();
        _rBody = _instance.rBody;
        _wheels = this.GetComponentsInChildren<WheelController>();
    }

    private void FixedUpdate()
    {
        _instance.stats.currentWheelSideFriction = CalculateDriftDrag(_instance.signalClient.GetDriftSignal()); ;
        _instance.stats.forwardAcceleration = CalcForwardForce(_instance.signalClient.GetForwardSignal(), _instance.signalClient.GetBoostSignal());
        _instance.stats.currentSteerAngle = CalculateSteerAngle(_instance.signalClient.GetTurnSignal());
    }

    private float CalculateDriftDrag(bool driftInput)
    {
        float currentDriftDrag = driftInput ? _instance.stats.wheelSideFrictionDrift : _instance.stats.wheelSideFriction;
        return Mathf.MoveTowards(_instance.stats.currentWheelSideFriction, currentDriftDrag, Time.deltaTime * _instance.stats.driftTime);
    }

    private float CalcForwardForce(float throttleInput, bool boostInput)
    {
        // Throttle

        float forwardAcceleration = (boostInput ? 1 : throttleInput) * _instance.stats.forwardSpeedAbs.GetForwardAcceleration(_instance.carData);

        if (_instance.stats.forwardSpeedSign != Mathf.Sign(throttleInput) && throttleInput != 0)
        {
            forwardAcceleration += -1 * _instance.stats.forwardSpeedSign * _instance.carData.BreakStrength;
        }

        return forwardAcceleration;
    }
    

    private float CalculateSteerAngle(float turnInput)
    {
        float curvature = 1 / _instance.stats.forwardSpeedAbs.GetTurnRadius();
        return turnInput * curvature * _instance.stats.turnRadiusCoefficient;
    }


}
