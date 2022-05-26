using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliders : MonoBehaviour
{

    public bool isTouchingSurface { private set; get; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isTouchingSurface = isTouchingGround();
    }



    bool isTouchingGround()
    {
        bool isTouching = Physics.Raycast(transform.position, -transform.up, out var hit, transform.localScale.y);
        return false || isTouching;
    }

}
