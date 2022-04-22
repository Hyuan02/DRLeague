using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class CarManager : MonoBehaviour
{

    Rigidbody _rBody;

    [SerializeField]
    internal Transform cogLow;

    [SerializeField]
    internal CarStates carState;

    [SerializeField]
    internal CarStats stats;

    [SerializeField]
    SphereColliders[] colliders;

    [SerializeField]
    internal TeamInfo info;

    [SerializeField]
    BodyCollider bCollider;

    [SerializeField]
    internal bool canMove = true;

    private IInputSignals _controllerInterface;

    Vector3 _defaultPosition;
    Quaternion _defaultRotation;


    // Start is called before the first frame update
    void Awake()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _rBody.centerOfMass = cogLow.localPosition;
        _rBody.maxAngularVelocity = Constants.MaxAngularVelocity;

        _defaultPosition = this.transform.localPosition;
        _defaultRotation = this.transform.localRotation;


        stats.boostQuantity = 100f;

        _controllerInterface = this.GetComponent<IInputSignals>();
        Debug.Log(_controllerInterface);
    }


    private void FixedUpdate()
    {
        DetermineCarState();
        UpdateStats();
    }


    void DetermineCarState()
    {
        stats.wheelsSurface = colliders.Count(e => e.isTouchingSurface);
        stats.isBodySurface = bCollider.IsOnGround;

        if (stats.isAllWheelsSurface)
        {
            carState = CarStates.AllWheelsSurface;
        }

        if (!stats.isAllWheelsSurface && !stats.isBodySurface)
        {
            carState = CarStates.SomeWheelsSurface;
        }

        if (stats.isBodySurface && !stats.isAllWheelsSurface)
        {
            carState = CarStates.BodySideGround;
        }

        if (stats.isAllWheelsSurface && Vector3.Dot(Vector3.up, transform.up) > Constants.Instance.NormalLength)
        {
            carState = CarStates.AllWheelsGround;
        }

        if (stats.isBodySurface && Vector3.Dot(Vector3.up, transform.up) < -Constants.Instance.NormalLength)
        {
            carState = CarStates.BodyGroundDead;
        }

        if(!stats.isBodySurface && stats.wheelsSurface == 0)
        {
            carState = CarStates.Air;
        }

        stats.isCanDrive = carState == CarStates.AllWheelsSurface || carState == CarStates.AllWheelsGround;
    }

    internal float GetForwardSignal() => canMove ? _controllerInterface.GetForwardSignal() : 0;

    internal float GetTurnSignal() => canMove? _controllerInterface.GetTurnSignal() : 0;

    internal bool GetBoostSignal() => canMove? _controllerInterface.GetBoostSignal() : false;

    internal bool GetJumpSignal() => canMove? _controllerInterface.GetJumpSignal() : false;

    internal bool GetDriftSignal() => canMove ? _controllerInterface.GetDriftSignal() : false;

    private void UpdateStats()
    {
        stats.forwardSpeed = Vector3.Dot(_rBody.velocity, transform.forward);
        stats.forwardSpeed = (float)System.Math.Round(stats.forwardSpeed, 2);
    }


    public void ResetCarState()
    {
        _rBody.velocity = Vector3.zero;
        _rBody.angularVelocity = Vector3.zero;

        transform.localPosition = _defaultPosition;
        transform.localRotation = _defaultRotation;
        stats.forwardSpeed = 0;
        stats.forwardAcceleration = 0;
        stats.boostQuantity = 100;

        stats.canFirstJump = true;
        stats.canDoubleJump = false;
        stats.canKeepJumping = false;
        stats.hasDoubleJump = false;
        stats.isJumping = false;
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
