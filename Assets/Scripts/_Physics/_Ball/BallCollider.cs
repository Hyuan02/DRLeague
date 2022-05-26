using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BallCollider : MonoBehaviour
{

    [SerializeField]
    private float _initialFactor = 400;
    [SerializeField]
    private float _collisionFactor = 50;
    private Rigidbody _rBody;

    public bool IsBeingTouched { private set; get; } = false;

    private void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car")) {
            ApplyBehaviourWithCar(collision);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            IsBeingTouched = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            IsBeingTouched = false;
        }
    }

    private void ApplyBehaviourWithCar(Collision collision)
    {
        float forceToApply = _initialFactor + collision.rigidbody.velocity.magnitude * _collisionFactor;
        Vector3 dirOfCollision = (transform.position - collision.transform.position).normalized;
        _rBody.AddForce(dirOfCollision * forceToApply);
    }
}
