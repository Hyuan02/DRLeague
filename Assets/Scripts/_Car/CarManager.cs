using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class CarManager : MonoBehaviour
{
    internal Rigidbody rBody { get; private set; } 

    [SerializeField]
    internal Transform cogLow;

    [SerializeField]
    internal CarStats stats;

    [SerializeField]
    SphereColliders[] colliders;

    [SerializeField]
    internal TeamInfo info;

    [SerializeField]
    BodyCollider bCollider;

    [SerializeField]
    internal SignalClient signalClient;

    private Vector3 _defaultPosition;
    private Quaternion _defaultRotation;


    // Start is called before the first frame update
    void Awake()
    {
        rBody = this.GetComponent<Rigidbody>();
        rBody.centerOfMass = cogLow.localPosition;
        rBody.maxAngularVelocity = Constants.MaxAngularVelocity;

        _defaultPosition = this.transform.localPosition;
        _defaultRotation = this.transform.localRotation;
        stats.boostQuantity = Constants.InitialBoostQuantity;
    }


    private void FixedUpdate()
    {
        UpdateStats();
    }


    void DetermineCarState()
    {
        stats.wheelsSurface = colliders.Count(e => e.isTouchingSurface);
        stats.isBodySurface = bCollider.IsOnGround;

        if (stats.isAllWheelsSurface)
        {
            stats.CarState = CarState.AllWheelsSurface;
        }

        if (!stats.isAllWheelsSurface && !stats.isBodySurface)
        {
            stats.CarState = CarState.SomeWheelsSurface;
        }

        if (stats.isBodySurface && !stats.isAllWheelsSurface)
        {
            stats.CarState = CarState.BodySideGround;
        }

        if (stats.isAllWheelsSurface && Vector3.Dot(Vector3.up, transform.up) > Constants.Instance.NormalLength)
        {
            stats.CarState = CarState.AllWheelsGround;
        }

        if (stats.isBodySurface && Vector3.Dot(Vector3.up, transform.up) < -Constants.Instance.NormalLength)
        {
            stats.CarState = CarState.BodyGroundDead;
        }

        if(!stats.isBodySurface && stats.wheelsSurface == 0)
        {
            stats.CarState = CarState.Air;
        }

        stats.isCanDrive = stats.CarState == CarState.AllWheelsSurface || stats.CarState == CarState.AllWheelsGround;
    }

    private void UpdateStats()
    {
        DetermineCarState();
        DetermineForwardSpeed();
    }


    private void DetermineForwardSpeed()
    {
        float pureForwardSpeed = Vector3.Dot(rBody.velocity, transform.forward);
        stats.forwardSpeed = (float)System.Math.Round(pureForwardSpeed, 2);
    }


    private void SetupCarStats()
    {
        stats.forwardSpeed = 0;
        stats.forwardAcceleration = 0;
        stats.boostQuantity = 100;
        stats.canFirstJump = true;
        stats.canDoubleJump = false;
        stats.canKeepJumping = false;
        stats.hasDoubleJump = false;
        stats.isJumping = false;
    }

    public void ResetCarState()
    {
        rBody.velocity = Vector3.zero;
        rBody.angularVelocity = Vector3.zero;
        transform.localPosition = _defaultPosition;
        transform.localRotation = _defaultRotation;
        SetupCarStats();
    }

    public void SetToPositionAndRotation(Vector3? position, Quaternion? rotation)
    {
        if(position.HasValue)
        {
            transform.localPosition = position.Value;
        }
        if (rotation.HasValue)
        {
            transform.localRotation = rotation.Value;
        }
    }
}
