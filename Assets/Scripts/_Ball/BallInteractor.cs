using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInteractor : MonoBehaviour
{
    [SerializeField]
    public TeamInfo _lastTouchedInfo { get; private set; }
   

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("BodyCollider") || other.CompareTag("SphereCollider")) {
            Debug.Log("Entered");
            CarManager actualCar = other.GetComponentInParent<CarManager>();
            CollisionWithCar(actualCar);
        }
    }

    void CollisionWithCar(CarManager car) {
        _lastTouchedInfo = car.info;
    }
}
