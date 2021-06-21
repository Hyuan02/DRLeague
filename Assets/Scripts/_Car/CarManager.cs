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

    internal CarStates carState;

    [SerializeField]
    internal CarStats stats;

    [SerializeField]
    SphereColliders[] colliders;

    [SerializeField]
    internal Teams _team;

    [SerializeField]
    internal uint id;

    // Start is called before the first frame update
    void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _rBody.centerOfMass = cogLow.localPosition;
        _rBody.maxAngularVelocity = Constants.Instance.MaxAngularVelocity;
    }


    private void FixedUpdate()
    {
        DetermineCarState();
        UpdateStats();
    }


    void DetermineCarState()
    {
        stats.wheelsSurface = colliders.Count(e => e.isTouchingSurface);

        if (stats.isAllWheelsSurface)
        {
            carState = CarStates.AllWheelsSurface;
        }

        if(!stats.isAllWheelsSurface && !stats.isBodySurface)
        {
            carState = CarStates.SomeWheelsSurface;
        }

        if (stats.isBodySurface && !stats.isAllWheelsSurface)
        {
            carState = CarStates.BodySideGround;
        }
            
        if(stats.isAllWheelsSurface && Vector3.Dot(Vector3.up, transform.up) > Constants.Instance.NormalLength)
        {
            carState = CarStates.AllWheelsGround;
        }

        if (stats.isAllWheelsSurface && Vector3.Dot(Vector3.up, transform.up) < -Constants.Instance.NormalLength)
        {
            carState = CarStates.AllWheelsGround;
        }

        //if (!isBodySurface && numWheelsSurface == 0)
        //{
        //    carState = CarStates.Air;
        //}

        stats.isCanDrive = carState == CarStates.AllWheelsSurface || carState == CarStates.AllWheelsGround;
    }

    internal float GetForwardSignal()
    {
        Debug.Log("Got forward signal!");
        return InputController.forwardInput;
    }

    internal float GetTurnSignal() => InputController.turnInput;

    internal bool GetBoostSignal() => InputController.boostInput;

    internal bool GetJumpSignal() => InputController.jumpInput;

    internal bool GetHeldJumpSignal() => InputController.HeldJumpInput;

    private void UpdateStats()
    {
        stats.forwardSpeed = Vector3.Dot(_rBody.velocity, transform.forward);
        stats.forwardSpeed = (float)System.Math.Round(stats.forwardSpeed, 2);
    }
}
