using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCabin : MonoBehaviour
{
    public float RotateSpeed;
    public HingeJoint Joint;
    private Rigidbody _rigidbody;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_rigidbody.IsSleeping())
            _rigidbody.WakeUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rigidbody.IsSleeping())
            _rigidbody.WakeUp();

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            var motor = Joint.motor;
            motor.targetVelocity = 100;
            Joint.motor = motor;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            var motor = Joint.motor;
            motor.targetVelocity = -100;
            Joint.motor = motor;
        }
        else
        {
            var motor = Joint.motor;
            motor.targetVelocity = 0;
            Joint.motor = motor;
        }
    }
}
