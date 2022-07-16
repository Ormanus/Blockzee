using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public static bool CubeAnimating = false;
    public static bool BlockAnimating = false;
    public static bool SceneTransition = false;

    public static CubeController Instance;
    public CubeController()
    {
        Instance = this;
    }

    public enum Direction
    {
        Right,
        Up,
        Left,
        Down
    }

    public Cube[] Cubes;
    public Level level;

    Cube Selected;

    private void Start()
    {
        if (Cubes.Length > 0)
            Selected = Cubes[0];
        else
        {
            Debug.LogError("No cubes in the list!");
        }
    }

    bool AllowInput
    {
        get { return !CubeAnimating && !SceneTransition && !BlockAnimating; }
    }

    public void Move(Direction direction)
    {
        if (AllowInput)
        {
            if (!IsObstructed(NextPosition(Selected.Position, direction)))
            {
                Selected.Move(direction);
            }
        }
    }

    public static Vector3Int NextPosition(Vector3Int position, Direction direction)
    {
        return direction switch
        {
            Direction.Right => position + Vector3Int.right,
            Direction.Up => position + Vector3Int.forward,
            Direction.Left => position + Vector3Int.left,
            Direction.Down => position + Vector3Int.back,
            _ => position + Vector3Int.right,
        };
    }

    bool IsObstructed(Vector3Int position)
    {
        foreach (Cube cube in Cubes)
        {
            if (cube.Position == position)
                return true;
        }

        foreach (Block block in level.blocks)
        {
            if (block.Position == position)
                return true;
        }

        return false;
    }
}
