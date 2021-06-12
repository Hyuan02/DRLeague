using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _targetToFollow;

    [SerializeField]
    [Range(2,20)]
    private float _smoothSpeed = 12.5f;
    [SerializeField]
    private Vector3 _offset = new Vector3(0,0,0);
    private void FixedUpdate()
    {

        SmoothCameraFollow();

    }


    void SmoothCameraFollow()
    {
        Vector3 desiredPosition = _targetToFollow.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(this.transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        //transform.LookAt(_targetToFollow);
    }
}
