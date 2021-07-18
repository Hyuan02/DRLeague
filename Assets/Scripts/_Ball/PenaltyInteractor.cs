using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenaltyInteractor : MonoBehaviour
{
    private enum BallPenaltyState
    {
        PRISTINE,
        TOUCHED,
        ON_COUNT
    }


    [SerializeField]
    PenaltyManager _instance;

    [SerializeField]
    Rigidbody _rBody;

    private BallPenaltyState state;

    bool _touched;

    bool _initiateCount;

    uint secondsToCount = 7;

    float _secondsCounted = 0;

    private void Start()
    {
        _rBody = this.GetComponentInParent<Rigidbody>();
        state = BallPenaltyState.PRISTINE;
        _instance.onGoalHappened += ResetBallState;
        _instance.onGameFinished += ResetBallState;
    }

    private void FixedUpdate()
    {
        if (state == BallPenaltyState.TOUCHED)
            AnalyzeBallSpeed();
        else if (state == BallPenaltyState.ON_COUNT)
            CountTimeBeforeReset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BodyCollider") || other.CompareTag("SphereCollider"))
        {
            _instance.OnTouchedBall();
            state = BallPenaltyState.TOUCHED;
        }
    }

    void AnalyzeBallSpeed()
    {
        float velocity = _rBody.velocity.magnitude;
        if(velocity < 5)
        {
            state = BallPenaltyState.ON_COUNT;
        }
    }

    void CountTimeBeforeReset()
    {
        _secondsCounted += Time.fixedDeltaTime;
        if(_secondsCounted > secondsToCount)
        {
            _secondsCounted = 0;
            state = BallPenaltyState.PRISTINE;
            _instance.OnStoppedBall();
            
        }
        
    }

    void ResetBallState(TeamInfo info, GoalInfo infoGoal)
    {
        state = BallPenaltyState.PRISTINE;
        _secondsCounted = 0;
    }

    void ResetBallState()
    {
        state = BallPenaltyState.PRISTINE;
        _secondsCounted = 0;
    }
}
