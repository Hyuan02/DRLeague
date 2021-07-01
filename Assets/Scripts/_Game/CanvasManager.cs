using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private List<Text> textElements = new List<Text>();
    [SerializeField]
    private Text _scorePrefab;
    [SerializeField]
    private Transform _scoreboard;
    private GameManager _manager;


    private void Start()
    {
        _manager = this.GetComponent<GameManager>();
        _manager.onGoalHappened += ChangeScores;
        SpawnScores();
    }

    private void SpawnScores()
    {
        for (int i = 0; i < _manager.mainStats.goalScore.Length; i++)
        {
            textElements.Add(Instantiate(_scorePrefab, _scoreboard));
        }
    }

    private void ChangeScores(TeamInfo info, GoalInfo goal)
    {
        textElements[((int)goal.team)].text = _manager.mainStats.goalScore[((int)goal.team)].ToString();
    }
}
