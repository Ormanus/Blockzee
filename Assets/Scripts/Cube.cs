using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public enum CubeState
    {
        Idle,
        Moving
    }

    [HideInInspector]
    public CubeState State { get; private set; }

    public Color color;
    public int diceId;
    public const float animationDuration = 0.2f;

    Vector3 originalPosition;
    Quaternion originalRotation;

    Vector3 targetPosition;
    Quaternion targetRotation;

    float animationStartTime;

    public Cube()
    {
        State = CubeState.Idle;
    }

    // Start is called before the first frame update
    void Start()
    {
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

    void MoveCube()
    {
        if (State == CubeState.Moving)
        {
            float phase = (Time.time - animationStartTime) / animationDuration;
            if (phase >= 1)
                State = CubeState.Idle;

            phase = Math.Min(phase, 1);

            transform.position = Vector3.Lerp(originalPosition, targetPosition, phase) + Vector3.up * 0.33f * Mathf.Sin(phase * Mathf.PI);
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, phase);
        }

    }

    private void Update()
    {
        MoveCube();
    }

    internal void Move(CubeController.Direction direction)
    {
        if (State != CubeState.Idle)
            return;

        State = CubeState.Moving;

        animationStartTime = Time.time;

        float amount = 90;
        Vector3 axis = Vector3.right;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        switch (direction)
        {
            case CubeController.Direction.Up:
                targetPosition = originalPosition + Vector3.forward;
                break;
            case CubeController.Direction.Down:
                targetPosition = originalPosition + Vector3.back;
                amount = -amount;
                break;
            case CubeController.Direction.Left:
                targetPosition = originalPosition + Vector3.left;
                axis = Vector3.forward;
                break;
            case CubeController.Direction.Right:
                targetPosition = originalPosition + Vector3.right;
                axis = Vector3.forward;
                amount = -amount;
                break;
        }

        targetRotation =  Quaternion.Euler(axis * amount) * transform.rotation;
    }
}
