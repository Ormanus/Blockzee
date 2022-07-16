using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockElevator : Block
{
    public Vector3Int ElevatorUpPosition;

    [HideInInspector]
    public Vector3Int ElevatorDownPosition;

    const float AnimationDuration = 0.25f;
    float animationStartTime;

    bool animating = false;
    bool movingUp = false;
    Cube passenger;

    public override void OnBlockEnter(Cube cube)
    {
        CubeController.BlockAnimating = true;
        animating = true;
        movingUp = true;
        passenger = cube;
        animationStartTime = Time.time;
    }

    public override void OnBlockExit(Cube cube)
    {
        CubeController.BlockAnimating = true;
        animating = true;
        movingUp = false;
        passenger = null;
        animationStartTime = Time.time;
    }

    protected override void OnStart()
    {
        ElevatorDownPosition = Position;
    }

    void MoveElevator()
    {
        float phase = (Time.time - animationStartTime) / AnimationDuration;

        Vector3 fromPosition = movingUp ? ElevatorDownPosition : ElevatorUpPosition;
        Vector3 toPosition = movingUp ? ElevatorUpPosition : ElevatorDownPosition;

        transform.position = Vector3.Lerp(fromPosition, toPosition, phase);

        if (movingUp && passenger)
            passenger.transform.position = transform.position + Vector3.up;

        if (phase >= 1)
        {
            transform.position = toPosition;

            CubeController.BlockAnimating = false;
            animating = false;
            UpdatePosition();

            if (passenger)
            {
                passenger.transform.position = toPosition + Vector3.up;
                passenger.UpdatePosition();
            }
        }
    }

    private void Update()
    {
        if (animating)
            MoveElevator();
    }
}
