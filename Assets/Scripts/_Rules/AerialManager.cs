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

    [SerializeField]
    float timeToWaitBeforeRestart = 10f;
    float _timeWaitedToRestart = 0f;

    uint _numberOfGoals = 0;

    bool _waitingToDrop = false;
    bool _waitingToRestart = false;


    public System.Action onRestartGame;
    public System.Action onGoalReceived;

    private AerialWatcher _watcher;

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
        CountTimeToRestart();
    }


    public override void StartCondition()
    {
        Debug.Log("Restart");
        onRestartGame?.Invoke();
        _carInstance.ResetCarState();
        _carInstance.SetToPositionAndRotation(null,Quaternion.Euler(0, 90, 0));
        _ballInstance.ResetBall();
        RestartTimeCounter();
        ThrowBall();
        _ballInstance.FreezeBall();
        _waitingToDrop = true;
    }

    public override void EndCondition()
    {
        throw new System.NotImplementedException();
    }

    private void ThrowBall()
    {
        Vector3 newPosition = GenerateBallAirPosition();
        _ballInstance.transform.localPosition = newPosition;
        _ballInstance.FreezeBall();
    }

    void ReceiveGoal(TeamInfo info, GoalInfo goal)
    {
        onGoalReceived?.Invoke();
    }

    public void OnGoalAnalyzed(bool validGoal)
    {
        if (validGoal)
        {
            Debug.Log("GOAAAAAAAAAAAAL with aerial!");
            _numberOfGoals++;
            UpdateScoreText();
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
                _waitingToRestart = true;
            }
        }
    }

    private void CountTimeToRestart()
    {
        if (_waitingToRestart)
        {
            _timeWaitedToRestart += Time.fixedDeltaTime;
            if(_timeWaitedToRestart >= timeToWaitBeforeRestart)
            {
                StartCondition();
            }
        }
    }

    private void RestartTimeCounter()
    {
        _timeWaitedToRestart = 0;
        _waitingToRestart = false;
        _waitingToDrop = false;
        _timeWaited = 0;
    }

    public void OnStoppedBall()
    {
        StartCondition();
    }

    private void UpdateScoreText()
    {
        if (_scoreText)
        {
            _scoreText.text = $"Goal: {_numberOfGoals}";
        }
    }
    private Vector3 GenerateBallAirPosition() => new Vector3(26, -0.5f, Random.Range(ballZMin, ballZMax));
}
