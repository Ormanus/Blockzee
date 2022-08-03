using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFalling : Undoable
{
    Vector3 originalPosition;
    const float Gravity = 20;
    float fallTime;
    bool falling = false;
    public override void OnBlockExit(Cube cube)
    {
        originalPosition = Position;
        Position = new Vector3Int(Position.x, Level.VoidHeihgt - 1, Position.z);
        fallTime = Time.time;
        falling = true;
    }

    private void Update()
    {
        if (falling)
        {
            float t = Time.time - fallTime;
            transform.position = originalPosition + Vector3.down * (Gravity * t * t);

            if (transform.position.y < -100)
            {
                falling = false;
                gameObject.SetActive(false);
            }
        }
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
            falling = false;
            gameObject.SetActive(true);
            int[] d = _actions.Pop().data;
            Position = new Vector3Int(d[0], d[1], d[2]);
            transform.position = Position;
        }
    }
}
