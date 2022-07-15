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
}
