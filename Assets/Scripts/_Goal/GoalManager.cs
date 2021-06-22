using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField]
    GoalInteractor[] _interactors;

    [SerializeField]
    BallManager _ball;

    GameManager _instance;

    private void Start()
    {
        foreach (GoalInteractor _interactor in _interactors) {
            _interactor.onGoalHappened += AttributeGoal;

        }
        _instance = this.GetComponent<GameManager>();
    }


    void AttributeGoal(TeamInfo info) {
        _instance.onGoalHappened.Invoke(info);
        _ball.ResetBall();
    } 
}
