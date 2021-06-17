using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostingController : MonoBehaviour
{

    CarManager _instance;
    Rigidbody _rBody;

    private void Start()
    {
        _instance = this.GetComponent<CarManager>();
        _rBody = this.GetComponent<Rigidbody>();


    }

    private void FixedUpdate()
    {
        UseBoost();
    }


    void UseBoost()
    {
        if(_instance.GetBoostSignal() && _instance.stats.forwardSpeed < Constants.Instance.MaxBoostSpeed)
        {
            _rBody.AddForce(Constants.BoostForce * Constants.BoostForceMultiplier * this.transform.forward, ForceMode.Acceleration);
        }
    }
}
