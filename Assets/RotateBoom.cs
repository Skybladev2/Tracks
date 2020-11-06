using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection
{
    None = 0,
    Positive = 1,
    Negative = -1
};

public class RotateBoom : MonoBehaviour
{
    public RotationDirection rotationState = RotationDirection.None;
    public float RotateSpeed;
    public float RotateForce;
    public float RotateTargetAngleUp;
    public float RotateTargetAngleDown;
    private ArticulationBody _body;

    // Use this for initialization
    void Start()
    {
        _body = GetComponent<ArticulationBody>();
    }

    private void Update()
    {
        if (_body.IsSleeping())
            _body.WakeUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_body.IsSleeping())
            _body.WakeUp();

        var drive = _body.xDrive;

        rotationState = RotationDirection.None;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotationState = RotationDirection.Positive;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rotationState = RotationDirection.Negative;
        }

        
        float rotationChange = (float)rotationState * RotateSpeed * Time.fixedDeltaTime;
        float rotationGoal = CurrentPrimaryAxisRotation() + rotationChange;
        RotateTo(rotationGoal);
    }

    float CurrentPrimaryAxisRotation()
    {
        float currentRotationRads = _body.jointPosition[0];
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        return currentRotation;
    }

    void RotateTo(float primaryAxisRotation)
    {
        var drive = _body.xDrive;
        drive.target = primaryAxisRotation;
        _body.xDrive = drive;
    }
}
