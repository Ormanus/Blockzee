using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Block
{
    public Color color;
    public int diceId;
    public const float animationDuration = 0.2f;

    Vector3 originalPosition;
    Quaternion originalRotation;

    Vector3 targetPosition;
    Quaternion targetRotation;

    float animationStartTime;

    bool animating = false;
    bool falling = false;

    const float Gravity = 20;

    Block blockBelow;

    protected override void OnStart()
    {
        ChangeBlock();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material material = new Material(mr.sharedMaterial);
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

    void MoveCube()
    {
        if (falling)
        {
            float t = Time.time - animationStartTime;
            float h = Mathf.Max(targetPosition.y, originalPosition.y - t * t * Gravity);
            transform.position = new Vector3(transform.position.x, h, transform.position.z);

            if (h == targetPosition.y)
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

    private void Update()
    {
        MoveCube();
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
}
