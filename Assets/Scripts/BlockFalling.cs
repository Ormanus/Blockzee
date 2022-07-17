using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFalling : Block
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
}
