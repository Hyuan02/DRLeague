using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParticleSystem : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _windParticles, _boostParticles, _fireParticles;
    private CarManager _instance;

    private TrailRenderer[] _trails;
    bool _isBoosting = false;


    private void Start()
    {
        _instance = GetComponentInParent<CarManager>();
        _trails = GetComponentsInChildren<TrailRenderer>();
        _trails[0].time = _trails[1].time = 0;
        _fireParticles.gameObject.SetActive(false);

    }

    private void LateUpdate()
    {
        HandleAnimationBoost();
    }

    private void FixedUpdate()
    {
        HandleParticleFX();
    }
    private void HandleAnimationBoost()
    {
        if (_instance.signalClient.GetBoostSignal() && _instance.stats.boostQuantity > 0) {
            if (!_isBoosting)
            {
                _boostParticles.Play();
                _fireParticles.gameObject.SetActive(true);
                _isBoosting = true;
            }
        }
        else
        {
            _boostParticles.Stop();
            _fireParticles.gameObject.SetActive(false);
            _isBoosting = false;
        }
    }

    private void HandleParticleFX()
    {
        if(_instance.stats.forwardSpeed >= Constants.SupersonicThreshold)
        {
            _windParticles.Play();

            if (_instance.stats.isAllWheelsSurface)
                _trails[0].time = _trails[1].time = Mathf.Lerp(_trails[1].time, 0.075f, Time.fixedDeltaTime * 5);
            else
                _trails[0].time = _trails[1].time = 0;
        }

        else
        {
            _windParticles.Stop();
            _trails[0].time = _trails[1].time = Mathf.Lerp(_trails[1].time, 0.029f, Time.fixedDeltaTime * 6);

            if (_trails[0].time <= 0.03f)
                _trails[0].time = _trails[1].time = 0;
        }
    }
}
