using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float speed = 10f;

    [HideInInspector]
    public float Angle = 0f;

    public void Rotate(float addition)
    {
        Angle += addition * Time.deltaTime * speed;
        if (Angle < 0)
            Angle += 360f;
        transform.localEulerAngles = new Vector3(45f, Angle, 0f);
    }
}
