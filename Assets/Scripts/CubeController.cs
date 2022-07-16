using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class CubeController : MonoBehaviour
{
    public static bool CubeAnimating = false;
    public static bool BlockAnimating = false;
    public static bool SceneTransition = false;
    public static bool Winning = false;

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
    public Cube Selected;

    public Cube[] Cubes;

    private void Awake()
    {
        CubeAnimating = false;
        BlockAnimating = false;
        SceneTransition = true;
        Winning = false;
        Cubes = FindObjectsOfType<Cube>();
        if (Cubes.Length > 0)
            Selected = Cubes[0];
        else
        {
            Debug.LogError("No cubes in the list!");
        }

        Cubes = Cubes.OrderBy(x => x.diceId).ToArray();
    }

    bool AllowInput
    {
        get { return !CubeAnimating && !SceneTransition && !BlockAnimating && !Winning; }
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

    public void SelectCube(int index)
    {
        Selected = Cubes[index];
    }

    public void NextCube()
    {
        int index = (Selected.diceId) % Cubes.Length;
        Selected = Cubes[index];
    }

    public void PreviousCube()
    {
        int index = (Selected.diceId - 2 + Cubes.Length) % Cubes.Length;
        Selected = Cubes[index];
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

    IEnumerator PrepareSceneTransition()
    {
        yield return new WaitForSeconds(2);
        FadeController.Instance.EndFade(Level.Instance.NextLevel);
    }

    public void Win()
    {
        Winning = true;
        Cube[] victoryCubes = Cubes.OrderBy(x => x.Position.x).ToArray();
        int id = 0;
        foreach (Cube cube in victoryCubes)
        {
            cube.Win(id++);
        }

        StartCoroutine(PrepareSceneTransition());
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
        if (AllowInput && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Camera cam = Camera.main;

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

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
