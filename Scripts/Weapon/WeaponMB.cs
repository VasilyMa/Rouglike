using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
public class WeaponMB : MonoBehaviour
{
    Material originalMaterial;
    [HideInInspector] public Renderer renderer;
    Vector3 startPosition;
    Quaternion startRotation;
    private void Start()
    {
        FindPhysicsUnit(transform);
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        originalMaterial = renderer.material;
    }
    public void WeaponReset()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        renderer.material = originalMaterial;
    }
    public void FindPhysicsUnit(Transform transform)
    {
        if (transform is null) return;
        if(!transform.TryGetComponent<PhysicsUnitMB>(out var physicsUnitMB))
        {
            FindPhysicsUnit(transform.parent);
            return;
        }
        physicsUnitMB.weaponMBs.Add(this);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        renderer = GetComponent<Renderer>();
        GetComponent<MeshCollider>().convex = true;
        //originalMaterial = renderer.material;
    }
#endif
}
