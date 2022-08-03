using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    public float fadeTime = 30f;
    public bool destroy = true;

    Material _mat;
    Color _color;
    Color _targetColor;
    float _startTime;

    private void Awake()
    {
        var mr = GetComponent<MeshRenderer>();
        _mat = new Material(mr.sharedMaterial);
        _color = _mat.color;
        _targetColor = new Color(_color.r, _color.g, _color.b, 0f);
        _startTime = Time.time;
    }

    private void Update()
    {
        float t = (Time.time - _startTime) / fadeTime;
        Color c = Color.Lerp(_color, _targetColor, t);
        _mat.color = c;

        if (destroy && t > 1f)
        {
            Destroy(_mat);
            Destroy(gameObject);
        }
    }
}
