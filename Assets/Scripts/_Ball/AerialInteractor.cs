using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AerialInteractor : MonoBehaviour
{

    public AerialKickState state { private set; get; }

    private float _secondsCounted = 0;
    [SerializeField]
    private uint secondsToCountBeforeReset = 5;
    [SerializeField]
    private float minimalBallVelocity = 3;
    private Rigidbody _rBody;


    [SerializeField]
    AerialManager _instance;

    [SerializeField]
    BallManager _manager;



    private void Start()
    {
        _rBody = this.GetComponentInParent<Rigidbody>();
        _manager.onBallDropped += ChangeBallStateToDrop;
        _manager.onFirstTouched += ChangeBallStateToTouch;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case AerialKickState.ON_COUNT:
                CountTimeOnDrop();
                break;
            case AerialKickState.TOUCHED:
                CheckVelocity();
                break;
        }
    }


    void ChangeBallStateToDrop()
    {
        state = AerialKickState.ON_COUNT;
    }
    
    void ChangeBallStateToTouch()
    {
        state = AerialKickState.TOUCHED;
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


    private void CountTimeOnDrop()
    {

        _secondsCounted += Time.fixedDeltaTime;
        if (_secondsCounted >= secondsToCountBeforeReset)
        {
            _secondsCounted = 0;
            state = AerialKickState.PRISTINE;
            _instance.OnStoppedBall();
        }
    }


}


public enum AerialKickState
{
    PRISTINE,
    ON_COUNT,
    TOUCHED
}