using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWheelCollider : MonoBehaviour
{
    public float Torque;
    public float TurnStiffnessMultiplier;
    public WheelCollider[] LeftWheels;
    public WheelCollider[] RightWheels;
    private long counter = 0;
    private float q;
    private float a;
    private float w;
    private float s;
    private WheelFrictionCurve _originalSidewaysFriction;
    private WheelFrictionCurve _originalForwardFriction;

    // Use this for initialization
    void Start()
    {
        _originalSidewaysFriction = RightWheels[0].sidewaysFriction;
        _originalForwardFriction = RightWheels[0].forwardFriction;
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0);
        //LeftWheels[0].ConfigureVehicleSubsteps(5, 12, 15);
    }

    private void FixedUpdate()
    {
        UpdateCenterOfMass();

        var q = Input.GetKey(KeyCode.Q) ? Torque : 0;
        var a = Input.GetKey(KeyCode.A) ? -Torque : 0;
        var w = Input.GetKey(KeyCode.W) ? Torque : 0;
        var s = Input.GetKey(KeyCode.S) ? -Torque : 0;
        var leftTorque = q + a;
        var rightTorque = w + s;

        if (leftTorque != 0 && rightTorque != 0)
        {
            RestoreFriction(LeftWheels);
            RestoreFriction(RightWheels);

            ApplyTorque(LeftWheels, leftTorque);
            ApplyBreak(LeftWheels, 0);
            ApplyTorque(RightWheels, rightTorque);
            ApplyBreak(RightWheels, 0);
        }

        if (leftTorque != 0 && rightTorque == 0)
        {
            ApplyTorque(LeftWheels, leftTorque);
            ApplyBreak(LeftWheels, 0);
            ApplyTorque(RightWheels, 0);
            ApplyBreak(RightWheels, 0);

            RemoveSidewaysFriction(RightWheels);
            IncreaseForwardFriction(RightWheels);
        }

        if (leftTorque == 0 && rightTorque != 0)
        {
            ApplyTorque(LeftWheels, 0);
            ApplyBreak(LeftWheels, 0);
            ApplyTorque(RightWheels, rightTorque);
            ApplyBreak(RightWheels, 0);

            RemoveSidewaysFriction(LeftWheels);
            IncreaseForwardFriction(LeftWheels);
        }

        if (leftTorque == 0 && rightTorque == 0)
        {
            RestoreFriction(LeftWheels);
            RestoreFriction(RightWheels);

            ApplyTorque(LeftWheels, 0);
            ApplyBreak(LeftWheels, Torque);
            ApplyTorque(RightWheels, 0);
            ApplyBreak(RightWheels, Torque);
        }
    }

    private void UpdateCenterOfMass()
    {
        //var centerOfMass = Vector3.zero;
        //float c = 0f;

        //foreach (GameObject part in assembly)
        //{
        //    centerOfMass += part.rigidbody.worldCenterOfMass * part.rigidbody.mass;
        //    c += part.rigidbody.mass;
        //}

        //centerOfMass /= c;
    }

    private void RestoreFriction(WheelCollider[] colliders)
    {
        RestoreForwardFriction(colliders);
        RestoreSidewaysFriction(colliders);
    }

    private void IncreaseForwardFriction(WheelCollider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] && colliders[i].enabled)
            {
                var friction = colliders[i].forwardFriction;
                friction.stiffness = TurnStiffnessMultiplier;
                colliders[i].forwardFriction = friction;
            }
        }
    }

    private void RemoveSidewaysFriction(WheelCollider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] && colliders[i].enabled)
            {
                var friction = colliders[i].sidewaysFriction;
                friction.stiffness = 0;
                colliders[i].sidewaysFriction = friction;
            }
        }
    }

    private void RestoreSidewaysFriction(WheelCollider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] && colliders[i].enabled)
            {
                colliders[i].sidewaysFriction = _originalSidewaysFriction;
            }
        }
    }

    private void RestoreForwardFriction(WheelCollider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] && colliders[i].enabled)
            {
                colliders[i].forwardFriction= _originalForwardFriction;
            }
        }
    }

    private void ApplyBreak(WheelCollider[] colliders, float @break)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] && colliders[i].enabled)
            {
                colliders[i].brakeTorque = @break;
            }
        }
    }

    private void ApplyTorque(WheelCollider[] colliders, float torque)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] && colliders[i].enabled)
            {
                colliders[i].motorTorque = torque;
                //AutoBrake(colliders, torque, i);
            }
        };
    }

    //private void AutoBrake(WheelCollider collider, float @break)
    //{
    //    if (torque == 0)
    //        collider.brakeTorque = Torque;
    //    else
    //        collider.brakeTorque = 0;
    //}
}
