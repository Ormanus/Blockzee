using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockElevator : Block
{
    public Vector3Int ElevatorUpPosition;

    [HideInInspector]
    public Vector3Int ElevatorDownPosition;

    const float AnimationDuration = 1f;
    float animationStartTime;

    bool movingUp = false;
    Cube passenger;

    public override void OnBlockEnter(Cube cube)
    {
        CubeController.BlockAnimating = true;
        movingUp = true;
        passenger = cube;
        animationStartTime = Time.time;
    }

    public override void OnBlockExit(Cube cube)
    {
        CubeController.BlockAnimating = true;
        movingUp = false;
        passenger = cube;
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
            CubeController.BlockAnimating = false;
            UpdatePosition();

            if (passenger)
                passenger.UpdatePosition();
        }
    }

    private void Update()
    {
        if (CubeController.BlockAnimating)
            MoveElevator();
    }
}
