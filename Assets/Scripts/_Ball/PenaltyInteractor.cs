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
    private RuleManager _instance;

    [SerializeField]
    private IPenaltyInteractions _interactor;

    [SerializeField]
    private Rigidbody _rBody;

    private BallPenaltyState _state;

    [Range(0,10)]
    [SerializeField]
    uint _secondsToCount = 7;

    float _secondsCounted = 0;

    private void Awake()
    {
        if (_instance)
        {
            _interactor = _instance.GetComponent<IPenaltyInteractions>();
        }
    }
    private void Start()
    {
        _rBody = this.GetComponentInParent<Rigidbody>();
        _state = BallPenaltyState.PRISTINE;
        _instance.onGoalHappened += ResetBallState;
        _instance.onGameFinished += ResetBallState;
    }

    private void FixedUpdate()
    {
        if (_state == BallPenaltyState.TOUCHED)
            AnalyzeBallSpeed();
        else if (_state == BallPenaltyState.ON_COUNT)
            CountTimeBeforeReset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BodyCollider") || other.CompareTag("SphereCollider"))
        {
            _interactor.OnTouchedBall();
            _state = BallPenaltyState.TOUCHED;
        }
    }

    void AnalyzeBallSpeed()
    {
        float velocity = _rBody.velocity.magnitude;
        if(velocity < 5)
        {
            _state = BallPenaltyState.ON_COUNT;
        }
    }

    void CountTimeBeforeReset()
    {
        _secondsCounted += Time.fixedDeltaTime;
        if(_secondsCounted > _secondsToCount)
        {
            _secondsCounted = 0;
            _state = BallPenaltyState.PRISTINE;
            _interactor.OnStoppedBall();
            
        }
        
    }

    void ResetBallState(TeamInfo info, GoalInfo infoGoal)
    {
        _state = BallPenaltyState.PRISTINE;
        _secondsCounted = 0;
    }

    void ResetBallState()
    {
        _state = BallPenaltyState.PRISTINE;
        _secondsCounted = 0;
    }
}
