using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level Instance { get; private set; }
    public Level()
    {
        Instance = this;
    }

    private void Start()
    {
        Block[] sceneBlocks = FindObjectsOfType<Block>();
        List<Block> levelBlocks = new();

        foreach (Block block in sceneBlocks)
        {
            if (block.GetType() != typeof(Cube))
                levelBlocks.Add(block);
        }

        blocks = levelBlocks.ToArray();
        Debug.Log("Blocks in level: " + blocks.Length);
    }

    public Block[] blocks;
}
