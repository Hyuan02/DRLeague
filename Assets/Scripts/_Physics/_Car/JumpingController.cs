using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingController : MonoBehaviour
{
    Rigidbody _rBody;
    CarManager _instance;

    float _jumpTimer = 0.2f;
    bool turningCarEffect = false;



    private void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _instance = this.GetComponent<CarManager>();
    }

    private void FixedUpdate()
    {
        if (!turningCarEffect)
        {
            CheckAirStatus();
            ControlJump();
        }
        else
        {
            TurningCarToGround();
        }
    }


    private void ControlJump()
    {
        if(_instance.GetJumpSignal() && _instance.stats.canFirstJump)
        {
            _instance.stats.canFirstJump = false;
            _rBody.AddForce(transform.up * Constants.InitalJumpTorque * Constants.JumpForceMultiplier, ForceMode.VelocityChange);
            _instance.stats.canKeepJumping = true;
            _instance.stats.isJumping = true;
        }

        if (_instance.GetHeldJumpSignal() && _instance.stats.isJumping && _instance.stats.canKeepJumping && _jumpTimer >= 0.05f && _jumpTimer <= 0.14f)
        {
            _rBody.AddForce(transform.up * Constants.MidJumpTorque * Constants.JumpForceMultiplier, ForceMode.Acceleration);
        }

        if (_instance.GetJumpSignal() && _instance.stats.isJumping && _jumpTimer >= 0.1f) {
            _rBody.AddForce(transform.up * Constants.InitalJumpTorque * Constants.JumpForceMultiplier, ForceMode.VelocityChange);
        }

        if (_instance.GetJumpSignal() && _instance.carState.Equals(CarStates.BodyGroundDead)) {
            turningCarEffect = true;

        }


        if (!_instance.GetHeldJumpSignal())
            _instance.stats.canKeepJumping = false;

    }


    private void CheckAirStatus()
    {
        if (_instance.stats.wheelsSurface > 2)
        {
            if (_jumpTimer >= 0.1f)
            {
                _instance.stats.isJumping = false;
                _jumpTimer = 0f;
                _instance.stats.canFirstJump = true;
            }
                
        }
        else
        {
            _instance.stats.canFirstJump = false;
            _jumpTimer += Time.fixedDeltaTime;
        }
    }

    private void TurningCarToGround()
    {
        Vector3 projection = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Quaternion rotationToGround = Quaternion.LookRotation(projection, Vector3.up);

        _rBody.MoveRotation(Quaternion.Lerp(_rBody.rotation, rotationToGround, Time.fixedDeltaTime * 5f));

        if((transform.up - Vector3.up).magnitude < 0.1f)
        {
            turningCarEffect = false;
        }
    }
}
