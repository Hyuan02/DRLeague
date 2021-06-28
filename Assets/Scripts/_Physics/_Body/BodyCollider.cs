using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollider : MonoBehaviour
{
    public bool IsOnGround { get; private set; } = false;

    [SerializeField]
    private Transform pointToCheck;

    private void FixedUpdate()
    {
        IsOnGround = BodyCheckRoutine();
    }


    bool BodyCheckRoutine()
    {
        var resultGround = Physics.Raycast(pointToCheck.position, transform.up, out var hit, 1f);
        if (resultGround && hit.collider.CompareTag("Ground")) {
            return true;
        }
        return false;
    }
}
