using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AerialManager))]
public class AerialWatcher : MonoBehaviour
{
    [Header("Indicators")]
    bool _hasJumped = false;

    public bool aerialMade { get; private set; } = false;

    uint _jumpQuantity = 0;

    [SerializeField]
    private CarManager _carInstance;

    [SerializeField]
    private BallCollider _colliderInstance;

    private AerialManager _manager;

    private void Start()
    {
        _manager = this.GetComponent<AerialManager>();
        _manager.onRestartGame += ResetWatchStats;
        _manager.onGoalReceived += AnalyzeGoal;
    }

    private void FixedUpdate()
    {
        WatchJumpQuantity();
        if (_colliderInstance.IsBeingTouched)
        {
            WatchGroundHeight();
        }
       
    }
    private void WatchJumpQuantity()
    {
        if (_carInstance.stats.isJumping)
        {
            if (!_hasJumped)
            {
                _hasJumped = true;
                ++_jumpQuantity;
                Debug.Log($"Has jumped {_jumpQuantity} times.");
            } 
        }
        else
        {
            _hasJumped = false;
        }
    }

    private void WatchGroundHeight()
    {
        Debug.DrawRay(_carInstance.transform.localPosition, Vector3.up * -1, Color.red);
        if (!Physics.Raycast(_carInstance.transform.localPosition, Vector3.up * -1, 4.0f))
        {
            aerialMade = true;
        }
    }
    
    private void ResetWatchStats()
    {
        _hasJumped = false;
        _jumpQuantity = 0;
        aerialMade = false;
    }

    private void AnalyzeGoal()
    {
        _manager.OnGoalAnalyzed(_jumpQuantity > 0 && aerialMade);
    }
}
