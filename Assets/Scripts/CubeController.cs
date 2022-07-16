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
        Cubes = FindObjectsOfType<Cube>();
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
            if (!GetBlockAtPosition(NextPosition(Selected.Position, direction)))
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



    public Block GetBlockAtPosition(Vector3Int position)
    {
        foreach (Cube cube in Cubes)
        {
            if (cube.Position == position)
                return cube;
        }

        foreach (Block block in Level.Instance.blocks)
        {
            if (block.Position == position)
                return block;
        }

        return null;
    }

    private void Update()
    {
        if (AllowInput && Input.GetMouseButtonDown(0))
        {
            Camera cam = Camera.main;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Transform hitObject = hit.transform;
                if (hitObject.TryGetComponent<Cube>(out var cube))
                {
                    Selected = cube;
                }
            }
        }
    }
}
