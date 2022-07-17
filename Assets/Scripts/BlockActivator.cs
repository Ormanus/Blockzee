using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockActivator : Block
{
    public BackAndForth door;
    int number;

    public override void OnBlockEnter(Cube cube)
    {
        if (cube.GetEyes() == number)
            door.MoveUp();
    }

    public override void OnBlockExit(Cube cube)
    {
        door.MoveDown();
    }

    private void OnEnable()
    {
        number = GetComponentInChildren<Face>().eyes;
    }
}
