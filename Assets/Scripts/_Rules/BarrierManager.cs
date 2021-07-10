using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : PenaltyManager
{

    [Header("Barrier Attributes")]
    [SerializeField]
    private float _minZBarrier;
    [SerializeField]
    private float _maxZBarrier;

    private float yBarrierValue
    {
        get
        {
            return Random.Range(0f, 1f) > 0.5f ? -3.5f : -7.36f;
        }
    }


    [SerializeField]
    private Transform _barrier;
    public override void StartCondition()
    {
        base.StartCondition();
        SetRandomPositionOnBarrier();
    }

    public override void EndCondition()
    {
        base.EndCondition();
    }

    private void SetRandomPositionOnBarrier()
    {
        Vector3 newPosition = GenerateRandomBarrierPosition();
        newPosition.x = _barrier.localPosition.x;
        newPosition.y = yBarrierValue;
        _barrier.localPosition = newPosition;
    }

    private Vector3 GenerateRandomBarrierPosition() => new Vector3(0, 0, UnityEngine.Random.Range(_minZBarrier, _maxZBarrier));


}
