using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Undoable
{
    public Color color;
    public int diceId;
    public const float animationDuration = 0.125f;
    public const float victoryDuration = 0.25f;

    Vector3 originalPosition;
    Quaternion originalRotation;

    Vector3 targetPosition;
    Quaternion targetRotation;

    float animationStartTime;

    bool animating = false;
    bool falling = false;

    const float Gravity = 20;
    readonly Color selectionColor = new Color(1.0f, 1.0f, 0.5f);
    readonly Color winColor1 = new Color(1.0f, 0.25f, 0.25f);
    readonly Color winColor2 = new Color(1.0f, 0.5f, 0.5f);

    public Block blockBelow;
    int victoryId = 0;

    Material material;

    protected override void OnStart()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        material = new Material(mr.sharedMaterial);
        material.color = color;
        mr.material = material;
    }

    public int GetEyes()
    {
        Face[] faces = GetComponentsInChildren<Face>();

        float maxHeight = faces[0].transform.position.y;
        int eyes = faces[0].eyes;

        foreach (Face face in faces)
        {
            if (face.transform.position.y > maxHeight)
            {
                eyes = face.eyes;
                maxHeight = face.transform.position.y;
            }
        }
        return eyes; 
    }

    void ChangeBlock()
    {
        if (blockBelow)
            blockBelow.OnBlockExit(this);

        blockBelow = CubeController.Instance.GetBlockAtPosition(Position + Vector3Int.down);

        if (blockBelow)
            blockBelow.OnBlockEnter(this);
    }

    void EndFalling()
    {
        UpdatePosition();
        animating = false;
        falling = false;
        CubeController.CubeAnimating = false;
        AudioManager.PlayClip(AudioManager.Instance.FallSound);
        ChangeBlock();
    }

    void EndAnimation()
    {
        UpdatePosition();
        int fallHeight = 0;
        Vector3Int checkedBlockPosition = Position + Vector3Int.down;
        while (!CubeController.Instance.GetBlockAtPosition(checkedBlockPosition) && checkedBlockPosition.y > Level.VoidHeihgt)
        {
            fallHeight++;
            checkedBlockPosition += Vector3Int.down;
        }

        if (fallHeight == 0)
        {
            animating = false;
            CubeController.CubeAnimating = false;
            AudioManager.PlayClip(AudioManager.Instance.MoveSound);
            ChangeBlock();
        }
        else
        {
            falling = true;
            originalPosition = Position;
            targetPosition = Position + Vector3Int.down * fallHeight;
            animationStartTime = Time.time;
        }
    }

    public void Die()
    {
        CubeController.Instance.PromptRestart();
        Instantiate(CubeController.Instance.DeathPrefab, transform.position, Quaternion.identity);
        AudioManager.PlayClip(AudioManager.Instance.DeathSound);

        foreach (Face face in GetComponentsInChildren<Face>())
        {
            var tr = face.transform;
            var f = Instantiate(face.gameObject, tr.position, tr.rotation, null);
            var fade = f.AddComponent<ObjectFade>();
            fade.fadeTime = 3f;
            fade.destroy = true;
            Rigidbody rb = f.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = f.transform.up * 2;
        }

        gameObject.SetActive(false);
    }

    void MoveCube()
    {
        if (falling)
        {
            float t = Time.time - animationStartTime;
            float h = Mathf.Max(targetPosition.y, originalPosition.y - t * t * Gravity);
            transform.position = new Vector3(transform.position.x, h, transform.position.z);

            if (h <= Level.VoidHeihgt + 1)
            {
                Die();
            }
            else if (h == targetPosition.y)
            {
                EndFalling();
            }
        }
        else if (animating)
        {
            float phase = (Time.time - animationStartTime) / animationDuration;

            transform.position = Vector3.Lerp(originalPosition, targetPosition, phase) + Vector3.up * 0.33f * Mathf.Sin(Math.Clamp(phase, 0, 1) * Mathf.PI);
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, phase);

            if (phase >= 1)
                EndAnimation();
        }

    }

    void ColorCube()
    {
        if (CubeController.Instance.Selected == this)
        {
            float phase = 0.5f + Mathf.Sin(Time.time * Mathf.PI) / 2;
            material.color = Color.Lerp(color, selectionColor, 0.25f + phase * 0.5f);
        }
        else
        {
            material.color = color;
        }
    }

    public void Win(int id)
    {
        victoryId = id;
        animationStartTime = Time.time;
    }
    void WinAnimation()
    {

        float phase = (Time.time - animationStartTime - victoryDuration * victoryId / 4f);
        if (phase < 1)
        {
            if (Time.time > animationStartTime + victoryDuration * victoryId / 4f)
            {
                material.color = Color.Lerp(winColor1, winColor2, 0.5f + Mathf.Sin((phase + 0.5f) * Mathf.PI) / 2);
            }

            float y = Mathf.Max(0, Mathf.Sin(phase * Mathf.PI * 2), 0);
            transform.position = Vector3.up * y + Position;
        }
        else
        {
            transform.position = Vector3.up * Gravity * (phase - 1) * (phase - 1) + Position;
            material.color = winColor1; 
        }
    }

    void TransitionAnimation()
    {
        float h = (1f - Time.timeSinceLevelLoad) * 20f;
        if (h < 0f)
        {
            transform.position = Position;
            CubeController.SceneTransition = false;

            foreach (Cube c in CubeController.Instance.Cubes)
            {
                c.UpdatePosition();
                c.transform.position = c.Position;
            }

            AudioManager.PlayClip(AudioManager.Instance.FallSound);
            ChangeBlock();
        }
        else
        {
            transform.position = Position + Vector3.up * h;
        }
    }

    private void Update()
    {
        if (CubeController.SceneTransition)
        {
            TransitionAnimation();
        }
        else if (CubeController.Winning)
        {
            WinAnimation();
        }
        else
        {
            MoveCube();
            ColorCube();
        }
    }

    internal void Move(CubeController.Direction direction)
    {
        animating = true;
        CubeController.CubeAnimating = true;

        animationStartTime = Time.time;

        float amount = 90;
        Vector3 axis = Vector3.right;

        Vector3Int nextPosition = CubeController.NextPosition(Position, direction);

        targetPosition = nextPosition;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        switch (direction)
        {
            case CubeController.Direction.Up:
                break;
            case CubeController.Direction.Down:
                amount = -amount;
                break;
            case CubeController.Direction.Left:
                axis = Vector3.forward;
                break;
            case CubeController.Direction.Right:
                axis = Vector3.forward;
                amount = -amount;
                break;
        }

        targetRotation =  Quaternion.Euler(axis * amount) * transform.rotation;
    }

    public override void Do(int move)
    {
        _actions.Push(new UndoableAction()
        {
            data = new int[] {
                Position.x, Position.y, Position.z, 
                Mathf.RoundToInt(transform.eulerAngles.x), Mathf.RoundToInt(transform.eulerAngles.y), Mathf.RoundToInt(transform.eulerAngles.z)
            },
            move = move,
        });
    }

    public override void Undo(int move)
    {
        //Debug.Log($"Undoing! ac: {_actions.Count} move: {move} peek: {_actions.Peek().move}");
        if (_actions.Count == 0)
            return;
        if (_actions.Peek().move == move)
        {
            falling = false;
            animating = false;
            CubeController.CubeAnimating = false;
            gameObject.SetActive(true);
            int[] d = _actions.Pop().data;
            Vector3Int pos = new Vector3Int(d[0], d[1], d[2]);
            Vector3Int rot = new Vector3Int(d[3], d[4], d[5]);
            //Debug.Log("Pos 1: " + Position);
            Position = pos;
            transform.position = Position;
            transform.eulerAngles = rot;
            ChangeBlock();
            //Debug.Log("Pos 2: " + Position);
        }
    }
}
