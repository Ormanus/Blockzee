using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : Block
{
    public Vector3Int UpPosition;

    [HideInInspector]
    public Vector3Int DownPosition;

    protected override void OnStart()
    {
        DownPosition = Position;
    }

    const float AnimationDuration = 0.25f;
    float animationStartTime;

    bool animating = false;
    bool movingUp = false;
    List<Cube> passengers = new();

    void ResetPassengers()
    {
        passengers.Clear();

        int h = 1;
        while (true)
        {
            Block blockAbove = CubeController.Instance.GetBlockAtPosition(Position + Vector3Int.up * h);

            if (blockAbove && blockAbove.GetType() == typeof(Cube))
                passengers.Add((Cube)blockAbove);
            else
                break;

            h++;
        }
    }

    public void MoveUp()
    {
        if (Position == UpPosition)
            return;

        CubeController.BlockAnimating = true;
        animating = true;
        movingUp = true;

        ResetPassengers();

        animationStartTime = Time.time;
    }

    public void MoveDown()
    {
        if (Position == DownPosition)
            return;

        CubeController.BlockAnimating = true;
        animating = true;
        movingUp = false;

        ResetPassengers();

        animationStartTime = Time.time;
    }

    void MoveElevator()
    {
        float phase = (Time.time - animationStartTime) / AnimationDuration;

        Vector3 fromPosition = movingUp ? DownPosition : UpPosition;
        Vector3 toPosition = movingUp ? UpPosition : DownPosition;

        transform.position = Vector3.Lerp(fromPosition, toPosition, phase);

        int passengerCount = 1;
        foreach (Cube passenger in passengers)
        {
            passenger.transform.position = transform.position + Vector3.up * passengerCount++;
        }

        if (phase >= 1)
        {
            transform.position = toPosition;

            CubeController.BlockAnimating = false;
            animating = false;
            UpdatePosition();

            int passengerCount2 = 1;
            foreach (Cube passenger in passengers)
            {
                passenger.transform.position = toPosition + Vector3.up * passengerCount2++;
                passenger.UpdatePosition();
            }

            if (CubeController.Instance.GetBlockAtPosition(Position) is Cube cube)
            {
                cube.Die();
            }
        }
    }

    private void Update()
    {
        if (animating)
            MoveElevator();
    }
}
