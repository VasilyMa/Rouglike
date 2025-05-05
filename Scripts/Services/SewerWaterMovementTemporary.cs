using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerWaterMovementTemporary : MonoBehaviour
{
    [SerializeField] private Vector2 _movementDirection;
    [SerializeField] private float _movementSpeed;
    private Material[] _materials;
    private Vector2 _nextOffset = new Vector2(); 
    void Start()
    {
        _movementDirection.Normalize();
        var renderers = transform.GetComponentsInChildren<MeshRenderer>();
        _materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            _materials[i] = renderers[i].material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _nextOffset += _movementDirection * Time.deltaTime * _movementSpeed;
        foreach (var material in _materials)
        {
            material.SetTextureOffset("_BaseMap", _nextOffset);
        }
    }
}
