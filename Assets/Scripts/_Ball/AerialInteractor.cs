using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialInteractor : MonoBehaviour
{
    private enum AerialKickState
    {
        PRISTINE,
        ON_COUNT
    }

    private AerialKickState state;

    private float _secondsCounted = 0;
    [SerializeField]
    private uint secondsToCountBeforeReset = 5;
    [SerializeField]
    private float minimalBallVelocity = 3;
    private Rigidbody _rBody;


    [SerializeField]
    AerialManager _instance;


    private void Start()
    {
        _rBody = this.GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckVelocity();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BodyCollider") || other.CompareTag("SphereCollider"))
        {
            state = AerialKickState.ON_COUNT;
        }
    }

    private void CheckVelocity()
    {
        float velocity = _rBody.velocity.magnitude;

        if (velocity < minimalBallVelocity)
        {
            _secondsCounted += Time.fixedDeltaTime;
            if(_secondsCounted >= secondsToCountBeforeReset)
            {
                _secondsCounted = 0;
                state = AerialKickState.PRISTINE;
                _instance.OnStoppedBall();
            }
        }
        else
        {
            _secondsCounted = 0;
        }


    }
}
