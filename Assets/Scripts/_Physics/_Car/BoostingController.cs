using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CarManager))]
public class BoostingController : MonoBehaviour
{

    private CarManager _instance;
    private Rigidbody _rBody;

    private void Start()
    {
        _instance = this.GetComponent<CarManager>();
        _rBody = _instance.rBody;
    }

    private void FixedUpdate()
    {
        BoostRoutine(_instance.signalClient.GetBoostSignal());
    }

    void BoostRoutine(bool hasBoostingInput) {
        if (hasBoostingInput)
        {
            UseBoost();
        }
        else
        {
            BoostRecovering();
        }
    }


    void UseBoost()
    {
        if(_instance.stats.forwardSpeed < _instance.carData.MaxBoostSpeed && _instance.stats.boostQuantity > 0)
        {
            _rBody.AddForce(_instance.carData.BoostForce * _instance.carData.BoostForceMultiplier * this.transform.forward, ForceMode.Acceleration);
            _instance.stats.boostQuantity = Mathf.Clamp(_instance.stats.boostQuantity - _instance.carData.BoostConsumingRate * Time.fixedDeltaTime, 0, 100);
            _instance.stats.isBoosting = true;
        }
    }

    void BoostRecovering() {
        _instance.stats.isBoosting = false;
        _instance.stats.boostQuantity = Mathf.Clamp(_instance.stats.boostQuantity + _instance.carData.BoostRecoveringRate * Time.fixedDeltaTime, 0, 100);
    }
}
