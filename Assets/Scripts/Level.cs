using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public const int VoidHeihgt = -4;
    public static Level Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Block[] sceneBlocks = FindObjectsOfType<Block>();
        List<Block> levelBlocks = new();
        List<BlockFinish> levelFinishBlocks = new();

        foreach (Block block in sceneBlocks)
        {
            if (block.GetType() != typeof(Cube))
                levelBlocks.Add(block);

            if (block.GetType() == typeof(BlockFinish))
                levelFinishBlocks.Add((BlockFinish)block);
        }

        blocks = levelBlocks.ToArray();
        finishBlocks = levelFinishBlocks.ToArray();
    }

    [HideInInspector]
    public Block[] blocks;
    [HideInInspector]
    public BlockFinish[] finishBlocks;

    [Header("Level numbers start from 0!")]
    public int levelNumber;
    public string NextLevel;
}
