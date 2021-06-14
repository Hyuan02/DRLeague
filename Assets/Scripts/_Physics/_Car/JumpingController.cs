using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingController : MonoBehaviour
{
    Rigidbody _rBody;
    CarManager _instance;

    float _jumpTimer = 0;

    private void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _instance = this.GetComponent<CarManager>();
    }

    private void FixedUpdate()
    {
        ControlJump();
        CheckAirStatus();
    }


    private void ControlJump()
    {
        if(_instance.GetJumpSignal() && _instance.stats.canFirstJump)
        {
            _rBody.AddForce(transform.up * Constants.InitalJumpTorque * Constants.JumpForceMultiplier, ForceMode.VelocityChange);
            _instance.stats.canKeepJumping = true;
            _instance.stats.canFirstJump = true;
            _instance.stats.isJumping = true;

            _jumpTimer += Time.fixedDeltaTime;
        }

        if(_instance.GetHeldJumpSignal() && _instance.stats.isJumping && _instance.stats.canKeepJumping && _jumpTimer <= 0.2f)
        {
            _rBody.AddForce(transform.up * Constants.MidJumpTorque * Constants.JumpForceMultiplier, ForceMode.Acceleration);
            _jumpTimer += Time.fixedDeltaTime;
        }

        if (!_instance.GetHeldJumpSignal())
            _instance.stats.canKeepJumping = false;

    }


    private void CheckAirStatus()
    {
        if (_instance.stats.isAllWheelsSurface)
        {
            if (_jumpTimer >= 0.1f)
                _instance.stats.isJumping = false;
            _jumpTimer = 0f;
            _instance.stats.canFirstJump = true;
        }
        else
        {
            _instance.stats.canFirstJump = false;
        }
    }
}
