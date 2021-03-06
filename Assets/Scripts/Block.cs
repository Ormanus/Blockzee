using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3Int Position { get; protected set; }

    public virtual void OnBlockEnter(Cube cube) { }
    public virtual void OnBlockExit(Cube cube) { }
    protected virtual void OnStart() { }

    public void UpdatePosition()
    {
        //Position = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        Position = Vector3Int.RoundToInt(transform.position);
    }

    private void Start()
    {
        UpdatePosition();
        OnStart();
    }
}
