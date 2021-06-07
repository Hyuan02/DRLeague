using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class CarManager : MonoBehaviour
{
    internal bool isAllWheelsSurface = false;
    internal bool isCanDrive = false;
    internal bool isBodySurface = false;



    Rigidbody _rBody;

    [SerializeField]
    internal Transform cogLow;

    internal CarStates carState;


    // Start is called before the first frame update
    void Start()
    {
        _rBody = this.GetComponent<Rigidbody>();
        _rBody.centerOfMass = cogLow.localPosition;
        _rBody.maxAngularVelocity = Constants.Instance.MaxAngularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void DetermineCarState()
    {
        if (isAllWheelsSurface)
        {
            carState = CarStates.AllWheelsSurface;
        }

        if(!isAllWheelsSurface && !isBodySurface)
        {
            carState = CarStates.SomeWheelsSurface;
        }

        if (isBodySurface && !isAllWheelsSurface)
        {
            carState = CarStates.BodySideGround;
        }
            
        if(isAllWheelsSurface && Vector3.Dot(Vector3.up, transform.up) > Constants.Instance.NormalLength)
        {
            carState = CarStates.AllWheelsGround;
        }

        if (isAllWheelsSurface && Vector3.Dot(Vector3.up, transform.up) < -Constants.Instance.NormalLength)
        {
            carState = CarStates.AllWheelsGround;
        }

        //if (!isBodySurface && numWheelsSurface == 0)
        //{
        //    carState = CarStates.Air;
        //}

        isCanDrive = carState == CarStates.AllWheelsSurface || carState == CarStates.AllWheelsGround;
    }
}
