using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalInteractor : MonoBehaviour
{
    public Action<TeamInfo, GoalInfo> onGoalHappened;

    [SerializeField]
    private GoalInfo _detail;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BallInteractor")) {
            BallInteractor inter = other.GetComponent<BallInteractor>();
            DetectGoalHit(inter._lastTouchedInfo, _detail);
        }
    }


    private void DetectGoalHit(TeamInfo info, GoalInfo goal)
    {
        if (_detail.team == info.team) { 
            onGoalHappened.Invoke(info, goal);
        }
    }
}
