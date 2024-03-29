using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AerialManager : RuleManager
{

    [SerializeField]
    CarManager _carInstance;

    [SerializeField]
    BallManager _ballInstance;

    [Header("Ball Params")]
    [SerializeField]
    float ballZMin = -5f;
    [SerializeField]
    float ballZMax = -5f;
    [SerializeField]
    float timeToWaitBeforeDrop = 2f;
    float _timeWaited = 0f;

    uint _numberOfGoals = 0;

    bool _waitingToDrop = false;


    public System.Action onRestartGame;
    public System.Action onGoalReceived;
    public System.Action onValidGoal;

    [SerializeField]
    private Text _scoreText;


    private void Start()
    {
        StartCondition();
        onGoalHappened += ReceiveGoal;
        UpdateScoreText();
    }

    private void FixedUpdate()
    {
        CountTimeToDrop();
    }


    public override void StartCondition()
    {
        onRestartGame?.Invoke();
        _carInstance.ResetCarState();
        _carInstance.SetToPositionAndRotation(null,Quaternion.Euler(0, 90, 0));
        _ballInstance.ResetBall();
        RestartTimeCounter();
        ThrowBall();
        onGameStarted?.Invoke();
        _ballInstance.FreezeBall();
        _waitingToDrop = true;
    }

    public override void EndCondition()
    {
        onGameFinished?.Invoke();
    }

    private void ThrowBall()
    {
        Vector3 newPosition = GenerateBallAirPosition();
        _ballInstance.transform.localPosition = newPosition;
        _ballInstance.FreezeBall();
    }

    protected override void ReceiveGoal(TeamInfo info, GoalInfo goal)
    {
        onGoalReceived?.Invoke();
    }

    public void OnGoalAnalyzed(bool validGoal)
    {
        if (validGoal)
        {
            _numberOfGoals++;
            onValidGoal?.Invoke();
            UpdateScoreText();
        }
        else
        {
            EndCondition();
        }
        StartCondition();
    }

    private void CountTimeToDrop()
    {
        if (_waitingToDrop)
        {
            _timeWaited += Time.fixedDeltaTime;
            if(_timeWaited > timeToWaitBeforeDrop)
            {
                _timeWaited = 0;
                _ballInstance.UnFreezeBall();
                _waitingToDrop = false;
            }
        }
    }


    private void RestartTimeCounter()
    {
        _waitingToDrop = false;
        _timeWaited = 0;
    }

    public void OnStoppedBall()
    {
        EndCondition();
        StartCondition();
    }

    private void UpdateScoreText()
    {
        if (_scoreText)
        {
            _scoreText.text = $"Goal: {_numberOfGoals}";
        }
    }
    private Vector3 GenerateBallAirPosition() => new Vector3(26, 1f, Random.Range(ballZMin, ballZMax));

    protected override void StartRoutine()
    {
        throw new System.NotImplementedException();
    }
}
