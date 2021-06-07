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


    public static Constants Instance;

    private void Awake()
    {
        Instance = this;
    }

}
