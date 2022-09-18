using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RuleManager : MonoBehaviour
{
    public Action<TeamInfo, GoalInfo> onGoalHappened;
    public Action onGameFinished, onGameStarted;

    [SerializeField]
    private float _timeToWaitBeforeRestart = 15f;
    
    private float _timeWaitedToRestart = 0f;

    public bool MlMode = false;

    public virtual void StartCondition()
    {
        _timeWaitedToRestart = 0f;
    }
    public abstract void EndCondition();
    protected abstract void ReceiveGoal(TeamInfo info, GoalInfo goal);

    [SerializeField]
    private GoalInteractor[] _interactors;
    protected abstract void StartRoutine();

    private void Start()
    {
        StartRoutine();
    }

    private void FixedUpdate()
    {
        CountTimeToRestart();
    }


    private void CountTimeToRestart()
    {
        _timeWaitedToRestart += Time.fixedDeltaTime;
        if (_timeWaitedToRestart >= _timeToWaitBeforeRestart)
        {
            EndCondition();
            StartCondition();
        }

    }
}
