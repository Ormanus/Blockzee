using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : Undoable
{
    public Vector3Int UpPosition;

    [HideInInspector]
    public Vector3Int DownPosition;

    protected override void OnStart()
    {
        DownPosition = Position;
        UpPosition = Position + UpPosition;
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

        ResetPassengers();

        if (passengers.Count == 0)
            return;

        CubeController.BlockAnimating = true;
        animating = true;
        movingUp = true;


        animationStartTime = Time.time;
    }

    public void MoveDown()
    {
        if (Position == DownPosition)
            return;

        ResetPassengers();

        if (passengers.Count > 0)
            return;

        CubeController.BlockAnimating = true;
        animating = true;
        movingUp = false;


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

    public override void Do(int move)
    {
        _actions.Push(new UndoableAction()
        {
            data = new int[] { Position.x, Position.y, Position.z },
            move = move,
        });
    }

    public override void Undo(int move)
    {
        if (_actions.Count == 0)
            return;
        if (_actions.Peek().move == move)
        {
            animating = false;
            movingUp = false;
            CubeController.BlockAnimating = false;
            gameObject.SetActive(true);
            int[] d = _actions.Pop().data;
            Position = new Vector3Int(d[0], d[1], d[2]);
            transform.position = Position;
        }
    }
}
