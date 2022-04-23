using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CarManager))]
public class BoostingController : MonoBehaviour
{

    CarManager _instance;
    Rigidbody _rBody;
    [SerializeField]
    float _boostConsumingRate = 5.0f;

    [SerializeField]
    float _boostRecoveringRate = 2.0f;

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
        if(_instance.stats.forwardSpeed < Constants.Instance.MaxBoostSpeed && _instance.stats.boostQuantity > 0)
        {
            _rBody.AddForce(Constants.BoostForce * Constants.BoostForceMultiplier * this.transform.forward, ForceMode.Acceleration);
            _instance.stats.boostQuantity = Mathf.Clamp(_instance.stats.boostQuantity - _boostConsumingRate * Time.fixedDeltaTime, 0, 100);
            _instance.stats.isBoosting = true;
        }
    }

    void BoostRecovering() {
        _instance.stats.isBoosting = false;
        _instance.stats.boostQuantity = Mathf.Clamp(_instance.stats.boostQuantity + _boostRecoveringRate * Time.fixedDeltaTime, 0, 100);
    }
}
