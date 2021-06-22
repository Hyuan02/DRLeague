using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private Vector3 _defaultBallPosition;
    private Rigidbody _rBody;

    private void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _defaultBallPosition = this.transform.position;
    }

    public void ResetBall()
    {
        transform.SetPositionAndRotation(_defaultBallPosition, Quaternion.identity);
        _rBody.velocity = Vector3.zero;
        _rBody.angularVelocity = Vector3.zero;
    }
}
