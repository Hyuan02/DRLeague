using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalInteractor : MonoBehaviour
{
    public Action<TeamInfo> onGoalHappened;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) {
            BallInteractor inter = other.GetComponent<BallInteractor>();
            DetectGoalHit(inter._lastTouchedInfo);
        }
    }


    private void DetectGoalHit(TeamInfo info)
    {
        onGoalHappened.Invoke(info);
    }
}
