using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField]
    GoalInteractor[] _interactors;

    [SerializeField]
    BallManager _ball;

    RuleManager _instance;

    private void Start()
    {
        foreach (GoalInteractor _interactor in _interactors) {
            _interactor.onGoalHappened += AttributeGoal;

        }
        _instance = this.GetComponent<RuleManager>();
    }


    void AttributeGoal(TeamInfo info, GoalInfo goal) {
        _instance.onGoalHappened.Invoke(info, goal);
        Debug.Log("Goal!");
        _ball.ResetBall();
    } 
}
