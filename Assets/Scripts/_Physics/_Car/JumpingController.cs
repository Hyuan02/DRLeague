using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarManager))]
public class JumpingController : MonoBehaviour
{
    private Rigidbody _rBody;
    private CarManager _instance;
    private float _jumpTimer = 0.2f;
    private bool _turningCarEffect = false;

    private void Start()
    {
        _instance = this.GetComponent<CarManager>();
        _rBody = _instance.rBody;
    }

    private void FixedUpdate()
    {
        if (!_turningCarEffect)
        {
            CheckAirStatus();
            ControlJump(_instance.signalClient.GetJumpSignal());
        }
        else
        {
            TurningCarToGround();
        }
    }


    private void ControlJump(bool hasJumpingInput)
    {
        if(hasJumpingInput && _instance.stats.canFirstJump)
        {
            _instance.stats.canFirstJump = false;
            _rBody.AddForce(transform.up * _instance.carData.InitalJumpTorque * _instance.carData.JumpForceMultiplier, ForceMode.VelocityChange);
            _instance.stats.canKeepJumping = true;
            _instance.stats.isJumping = true;
        }

        if (hasJumpingInput && _instance.stats.isJumping && _instance.stats.canKeepJumping && _jumpTimer >= 0.05f && _jumpTimer <= 0.14f)
        {
            _rBody.AddForce(transform.up * _instance.carData.MidJumpTorque * _instance.carData.JumpForceMultiplier, ForceMode.Acceleration);
        }

        if (hasJumpingInput && _instance.stats.isJumping && _jumpTimer >= 0.2f && _instance.stats.canDoubleJump && !_instance.stats.hasDoubleJump) {
            _rBody.AddForce(transform.up * _instance.carData.InitalJumpTorque * _instance.carData.JumpForceMultiplier, ForceMode.VelocityChange);
            _instance.stats.canDoubleJump = false;
            _instance.stats.hasDoubleJump = true;
        }

        if (hasJumpingInput && _instance.stats.CarState.Equals(CarState.BodyGroundDead)) {
            _turningCarEffect = true;
        }

        if (_instance.stats.isJumping && !hasJumpingInput)
        {
            _instance.stats.canDoubleJump = true;
        }
        else if (!hasJumpingInput)
        {
            _instance.stats.canKeepJumping = false;
        }
    }


    private void CheckAirStatus()
    {
        if (_instance.stats.wheelsSurface > 2)
        {
            if (_jumpTimer >= 0.1f)
            {
                _instance.stats.isJumping = false;
                _instance.stats.canDoubleJump = false;
                _instance.stats.hasDoubleJump = false;
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
            _turningCarEffect = false;
        }
    }
}
