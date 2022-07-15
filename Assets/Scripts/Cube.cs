using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Color color;
    public int diceId;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material material = new Material(mr.sharedMaterial);
        material.color = color;
        mr.material = material;
    }

    internal void Move(CubeController.Direction right)
    {
        throw new NotImplementedException();
    }
}
