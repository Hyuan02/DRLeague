using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    [SerializeField]
    private float _maxAngularVelocity = 5.5f;

    public float MaxAngularVelocity => _maxAngularVelocity;

    [SerializeField]
    private float _normalLength = 0.95f;

    public float NormalLength => _normalLength;


    public readonly float MaxSpeed = 14.1f;
    public readonly float IntermediateSpeed = 14.0f;

    public const float BREAK_STRENGTH = 35;

    public const float BrakeAcceleration = 5.25f;

    public const float wheelsRevolutionSpeed = 13f;



    public static Constants Instance;

    private void Awake()
    {
        Instance = this;
    }

}
