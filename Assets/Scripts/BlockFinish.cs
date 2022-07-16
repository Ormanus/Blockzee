using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFinish : Block
{
    bool IsYachtzee()
    {
        int eyes = -1;
        foreach (Cube cube in CubeController.Instance.Cubes)
        {
            if (eyes == -1)
                eyes = cube.GetEyes();

            if (cube.GetEyes() != eyes)
                return false;

            if (!cube.blockBelow || cube.blockBelow.GetType() != typeof(BlockFinish))
                return false;
        }

        return true;
    }

    public override void OnBlockEnter(Cube cube)
    {
        if (IsYachtzee())
        {
            Debug.Log("Level Finished");
            AudioManager.PlayClip(AudioManager.Instance.FullFinishSound);
            CubeController.Instance.Win();
        }
        else
        {
            AudioManager.PlayClip(AudioManager.Instance.SingleFinishSound);
        }
    }
}
