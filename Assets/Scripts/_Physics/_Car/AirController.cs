using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirController : MonoBehaviour
{
    private Rigidbody _rBody;
    private CarManager _instance;

    bool useDamperTorque = true;


    private void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _instance = this.GetComponent<CarManager>();

    }

    private void FixedUpdate()
    {
        if (!_instance.stats.isAllWheelsSurface)
        {
            DoRotation(_instance.GetTurnSignal(), _instance.GetForwardSignal());
        }
    }

    void DoRotation(float yawInput, float pitchInput)
    {
        //pitch
        _rBody.AddTorque(Constants.Torque_Pitch * pitchInput * transform.right, ForceMode.Acceleration);
        if (useDamperTorque)
            _rBody.AddTorque(transform.right * Constants.Drag_Pitch * (1 - Mathf.Abs(pitchInput)) * transform.InverseTransformDirection(_rBody.angularVelocity).x
                , ForceMode.Acceleration);

        //yaw
        _rBody.AddTorque(Constants.Torque_Yaw * yawInput * transform.up, ForceMode.Acceleration);
        if (useDamperTorque)
            _rBody.AddTorque(transform.up * Constants.Drag_Yaw * (1 - Mathf.Abs(yawInput)) * transform.InverseTransformDirection(_rBody.angularVelocity).y,
                ForceMode.Acceleration);

    }
}
