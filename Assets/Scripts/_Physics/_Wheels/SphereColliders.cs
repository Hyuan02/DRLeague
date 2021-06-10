using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliders : MonoBehaviour
{

    public bool isTouchingSurface { private set; get; }

    float _wheelDiameter, _wheelOffset = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isTouchingSurface = isTouchingGround();
        Debug.Log(isTouchingSurface);
    }



    bool isTouchingGround()
    {
        bool isTouching = Physics.Raycast(transform.position, -transform.up, out var hit, transform.localScale.y);
        return false || isTouching;
    }

}
