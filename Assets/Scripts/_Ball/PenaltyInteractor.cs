using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenaltyInteractor : MonoBehaviour
{
    [SerializeField]
    PenaltyManager _instance;

    [SerializeField]
    Rigidbody _rBody;

    bool _touched;

    private void Start()
    {
        _rBody = this.GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_touched)
            AnalyzeBallSpeed();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BodyCollider") || other.CompareTag("SphereCollider"))
        {
            _instance.OnTouchedBall();
            _touched = true;
        }
    }

    void AnalyzeBallSpeed()
    {
        float velocity = _rBody.velocity.magnitude;
        Debug.Log(velocity);
        if(velocity < 5)
        {
            _instance.OnStoppedBall();
            _touched = false;
        }
    }
}
