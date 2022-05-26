using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarManager))]
public class AirController : MonoBehaviour
{
    private Rigidbody _rBody;
    private CarManager _instance;
    [SerializeField]
    private bool _useDamperTorque = true;


    private void Start()
    {
        _instance = this.GetComponent<CarManager>();
        _rBody = _instance.rBody;
    }

    private void FixedUpdate()
    {
        if (!_instance.stats.isAllWheelsSurface)
        {
            DoRotation(_instance.signalClient.GetTurnSignal(), _instance.signalClient.GetForwardSignal());
        }
    }

    void DoRotation(float yawInput, float pitchInput)
    {
        //pitch
        _rBody.AddTorque(EnvironmentConstants.Torque_Pitch * pitchInput * transform.right, ForceMode.Acceleration);
        if (_useDamperTorque)
            _rBody.AddTorque(transform.right * EnvironmentConstants.Drag_Pitch * (1 - Mathf.Abs(pitchInput)) * transform.InverseTransformDirection(_rBody.angularVelocity).x
                , ForceMode.Acceleration);

        //yaw
        _rBody.AddTorque(EnvironmentConstants.Torque_Yaw * yawInput * transform.up, ForceMode.Acceleration);
        if (_useDamperTorque)
            _rBody.AddTorque(transform.up * EnvironmentConstants.Drag_Yaw * (1 - Mathf.Abs(yawInput)) * transform.InverseTransformDirection(_rBody.angularVelocity).y,
                ForceMode.Acceleration);

    }
}
