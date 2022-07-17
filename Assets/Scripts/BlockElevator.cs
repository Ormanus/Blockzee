using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockElevator : BackAndForth
{
    public override void OnBlockEnter(Cube cube)
    {
        MoveUp();
    }

    public override void OnBlockExit(Cube cube)
    {
        MoveDown();
    }
}
