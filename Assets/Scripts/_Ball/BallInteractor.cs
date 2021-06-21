using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInteractor : MonoBehaviour
{
    [SerializeField]
    private uint _lastTouchedId;
    [SerializeField]
    private Teams _lastTouchedTeam;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("BodyCollider") || other.CompareTag("SphereCollider")) {
            Debug.Log("Entered");
            CarManager actualCar = other.GetComponentInParent<CarManager>();
            CollisionWithCar(actualCar);
        }
    }

    void CollisionWithCar(CarManager car) {
        _lastTouchedId = car.id;
        _lastTouchedTeam = car._team;
    }
}
