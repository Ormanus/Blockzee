using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCubes : MonoBehaviour
{
    Vector3 targetPosition;
    const float CameraSpeed = 10;
    void UpdateTargetPosition()
    {


        float minX = 0, maxX = 0, minY = 0, maxY = 0, minZ = 0, maxZ = 0;
        bool firstCubeChecked = false;

        foreach (Cube cube in CubeController.Instance.Cubes)
        {
            Vector3 pos = cube.transform.position;
            if (CubeController.Winning || CubeController.SceneTransition)
            {
                pos = cube.Position;
            }

            if (!firstCubeChecked)
            {
                firstCubeChecked = true;
                minX = pos.x;
                maxX = pos.x;
                minY = pos.y;
                maxY = pos.y;
                minZ = pos.z;
                maxZ = pos.z;
            }
            else
            {
                minX = Mathf.Min(minX, pos.x);
                maxX = Mathf.Max(maxX, pos.x);
                minY = Mathf.Min(minY, pos.y);
                maxY = Mathf.Max(maxY, pos.y);
                minZ = Mathf.Min(minZ, pos.z);
                maxZ = Mathf.Max(maxZ, pos.z);
            }
        }
        float centerX = (minX + maxX) / 2;
        float centerY = (minY + maxY) / 2;
        float centerZ = (minZ + maxZ) / 2;

        float dist = Mathf.Max(maxX - minX, maxZ - minZ);

        targetPosition = new Vector3(centerX, centerY, centerZ) - Camera.main.transform.forward * (dist / 1.4f + 10);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateTargetPosition();
        transform.position = targetPosition;
    }

    bool started = false;
    // Update is called once per frame
    void Update()
    {
        if (!started)
            transform.position = targetPosition;

        started = true;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1.0f - Mathf.Pow(1 / CameraSpeed, Time.deltaTime));
    }

    private void FixedUpdate()
    {

        UpdateTargetPosition();
    }
}
