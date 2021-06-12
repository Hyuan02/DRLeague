using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WheelPosition
{
    FL,
    FR,
    RL,
    RR
}


public class WheelController : MonoBehaviour
{

    [SerializeField]
    private GroundController _controller;
    [SerializeField]
    private CarManager _manager;
    private Rigidbody _rBody;

    [SerializeField]
    private Transform _wheelMesh;

    [SerializeField]
    private WheelPosition _wheelPosition;

    private float _meshRevolutionAngle;

    private float _forceCurve = 0;


    private float _wheelRadius, _wheelForwardVelocity, _wheelLateralVelocity;
    Vector3 _wheelVelocity, _lastWheelVelocity, _wheelAcceleration, _wheelContactPoint, _lateralForcePosition = Vector3.zero;

    private void Start()
    {
        _rBody = this.GetComponentInParent<Rigidbody>();
        _manager = this.GetComponentInParent<CarManager>();
        _controller = this.GetComponentInParent<GroundController>();
        _wheelRadius = transform.localScale.z / 2;
    }

    private void FixedUpdate()
    {
        UpdateWheelState();
        if (!_manager.stats.isCanDrive) return;

        ApplyForwardForce(_manager.stats.forwardAcceleration/4);
        RotateWheels(_manager.stats.currentSteerAngle);
        ApplySideForce();
        SimulateDrag();
    }

    internal void RotateWheels(float steerAngle)
    {

        Debug.Log("Steer angle is: " + steerAngle);
        if(WheelPosition.FL == _wheelPosition || WheelPosition.FR == _wheelPosition)
        {

            Debug.Log("Applying force");
            transform.localRotation = Quaternion.Euler(Vector3.up * steerAngle);
        }

        if (_wheelMesh)
        {
            Debug.Log("Applying rot");
            Debug.Log(transform.localEulerAngles);
            _wheelMesh.localRotation = transform.localRotation;
            Debug.Log("Rot mesh" + _wheelMesh.localEulerAngles);
            _meshRevolutionAngle += (Time.deltaTime * transform.InverseTransformDirection(_wheelVelocity).z) /
              (2 * Mathf.PI * _wheelRadius) * 360;

            _wheelMesh.Rotate(Vector3.right, _meshRevolutionAngle * 1.3f);
        }
    }

    private void ApplyForwardForce(float force)
    {
        _rBody.AddForce(force * transform.forward, ForceMode.Acceleration);

        if (0 == force && 0.1f > _manager.stats.forwardSpeedAbs)
        {
            _rBody.velocity = new Vector3(_rBody.velocity.x, _rBody.velocity.y, 0);
        }
    }

    private void ApplySideForce()
    {
        _forceCurve = _wheelLateralVelocity * _manager.stats.currentWheelSideFriction;
        _lateralForcePosition = transform.localPosition;
        _lateralForcePosition.y = _manager.cogLow.localPosition.y;
        _lateralForcePosition = _manager.transform.TransformPoint(_lateralForcePosition);
        _rBody.AddForceAtPosition(-_forceCurve * transform.right, _lateralForcePosition, ForceMode.Acceleration);
    }

    private void SimulateDrag()
    {
        if ((_manager.stats.forwardSpeedAbs >= 0.1f)) return;
        AddDragForce();
    }

    private void AddDragForce()
    {
        float dragForce = Constants.BrakeAcceleration / 4 * _manager.stats.forwardSpeedSign * (1 - _manager.GetForwardSignal());
        _rBody.AddForce(-dragForce * transform.forward, ForceMode.Acceleration);
    }

    private void UpdateWheelState()
    {
        _wheelContactPoint = transform.position - transform.up * _wheelRadius;
        _wheelVelocity = _rBody.GetPointVelocity(_wheelContactPoint);
        _wheelForwardVelocity = Vector3.Dot(_wheelVelocity, transform.forward);
        _wheelLateralVelocity = Vector3.Dot(_wheelVelocity, transform.right);

        _wheelAcceleration = (_wheelVelocity - _lastWheelVelocity) * Time.fixedTime;
        _lastWheelVelocity = _wheelVelocity;
    }

}
