using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private Vector3 _defaultBallPosition;
    private Rigidbody _rBody;
    public BallStates State
    {
        get;
        private set;
    }

    private void Awake()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _defaultBallPosition = this.transform.localPosition;
    }

    public void ResetBall()
    {
        transform.localPosition = _defaultBallPosition;
        transform.localRotation = Quaternion.identity;
        _rBody.velocity = Vector3.zero;
        _rBody.angularVelocity = Vector3.zero;
    }

    public void SetBallOnPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void FreezeBall()
    {
        _rBody.isKinematic = true;
        _rBody.Sleep();
        State = BallStates.FREEZE;
    }

    public void UnFreezeBall()
    {
        _rBody.WakeUp();
        _rBody.isKinematic = false;
        State = BallStates.IN_MOVEMENT;
    }


    public void SetToPositionAndRotation(Vector3? position, Quaternion? rotation)
    {
        if (position.HasValue)
        {
           transform.localPosition = position.Value;
        }
        if (rotation.HasValue)
        {
            transform.localRotation = rotation.Value;
        }
    }
}
