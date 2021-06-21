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
    private bool _isTouchedGround = false;
    private Rigidbody _rBody;

    private void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car")) {
            ApplyBehaviourWithCar(collision);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            ApplyBehaviourWithGround();
        }
    }


    private void ApplyBehaviourWithCar(Collision collision)
    {
        float forceToApply = _initialFactor + collision.rigidbody.velocity.magnitude * _collisionFactor;
        Vector3 dirOfCollision = (transform.position - collision.transform.position).normalized;
        _rBody.AddForce(dirOfCollision * forceToApply);
    }

    private void ApplyBehaviourWithGround()
    {
        _isTouchedGround = true;
    }
}
