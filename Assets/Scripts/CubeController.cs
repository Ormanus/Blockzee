using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

public class CubeController : MonoBehaviour
{
    public static bool CubeAnimating = false;
    public static bool BlockAnimating = false;
    public static bool SceneTransition = false;
    public static bool Winning = false;
    public static bool Dying = false;

    public static CubeController Instance;

    public enum Direction
    {
        Right,
        Up,
        Left,
        Down
    }
    [HideInInspector]
    public Cube Selected;
    [HideInInspector]
    public Cube[] Cubes;
    public GameObject DeathPrefab;
    public GameObject EndRoot;
    public UndoManager undoManager;
    public CameraRotation camRot;
    public PlayerInput input;

    private void Awake()
    {
        Instance = this;
        Dying = false;
        CubeAnimating = false;
        BlockAnimating = false;
        SceneTransition = true;
        Winning = false;
        Cubes = FindObjectsOfType<Cube>();
        Cubes = Cubes.OrderBy(x => x.diceId).ToArray();

        EndRoot.SetActive(false);

        if (Cubes.Length > 0)
            Selected = Cubes[0];
        else
        {
            Debug.LogError("No cubes in the list!");
        }
    }

    bool AllowInput
    {
        get { return !CubeAnimating && !SceneTransition && !BlockAnimating && !Winning && !Dying; }
    }

    public void Move(Direction direction)
    {
        if (AllowInput)
        {
            // Apply camera rotation
            int dir = (int)direction;
            dir = (dir + Mathf.RoundToInt(-camRot.Angle / 90 + 4)) % 4;
            direction = (Direction)dir;

            // Check for collisions
            if (!GetBlockAtPosition(NextPosition(Selected.Position, direction)) && 
                !GetBlockAtPosition(Selected.Position + Vector3Int.up) && 
                !GetBlockAtPosition(NextPosition(Selected.Position, direction) + Vector3Int.up))
            {
                undoManager.Do();
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

    public void Restart()
    {
        StartCoroutine(PrepareSceneTransition(SceneManager.GetActiveScene().name, 1));
    }

    public void PromptRestart()
    {
        EndRoot.SetActive(true);
        input.currentActionMap = input.actions.FindActionMap("Menu");
    }

    public void Undo()
    {
        EndRoot.SetActive(false);
        input.currentActionMap = input.actions.FindActionMap("Menu");
        undoManager.Undo();
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

    IEnumerator PrepareSceneTransition(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        FadeController.Instance.EndFade(sceneName);
    }

    public void Win(bool yahtzee)
    {
        Winning = true;
        Cube[] victoryCubes = Cubes.OrderBy(x => x.Position.x).ToArray();
        int id = 0;
        foreach (Cube cube in victoryCubes)
        {
            cube.Win(id++);
        }

        StartCoroutine(PrepareSceneTransition(Level.Instance.NextLevel, 2));

        SaveSystem.SetLevelState(Level.Instance.levelNumber, yahtzee ? 2 : 1);
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
