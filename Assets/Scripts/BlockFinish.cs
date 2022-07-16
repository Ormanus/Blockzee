using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFinish : Block
{
    public override void OnBlockEnter(Cube cube)
    {
        AudioManager.PlayClip(AudioManager.Instance.SingleFinishSound);
    }
}
