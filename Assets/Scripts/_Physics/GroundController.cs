using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class GroundController : MonoBehaviour
{

    private Rigidbody _rBody;
    private CarManager _instance;
    private void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _instance = this.GetComponent<CarManager>();
    }

    private void FixedUpdate()
    {
        ApplyDownForce();
    }

    private void ApplyDownForce()
    {
        if (_instance.carState == CarStates.AllWheelsSurface || _instance.carState == CarStates.AllWheelsGround)
            _rBody.AddForce(-transform.up * 5, ForceMode.Acceleration);
    }
}
