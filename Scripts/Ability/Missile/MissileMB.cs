using System.Collections.Generic;
using Client;
using FMODUnity;
using Leopotam.EcsLite;

using UnityEngine;

public abstract class MissileMB : MonoBehaviour, IPool
{

    protected EcsWorld _world;
    protected int _entityMissile;
    [HideInInspector] public int layerMaskTarget;
    public EventReference soundEventReference;
    public bool attachTransform;

    protected bool isAvaiable;
    public GameObject ThisGameObject => gameObject;

    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KEY_ID;
    public string KEY_ID;

    public virtual void Invoke(EcsWorld world, int entity, int layerMask)
    {
        _entityMissile = entity;
        _world = world;
        layerMaskTarget = layerMask;
        if (attachTransform)
        {
            SoundEntity.Instance.PlayAudioAttached(soundEventReference, this.transform);
        }
        else
        {
            SoundEntity.Instance.PlayAudioAtPosition(soundEventReference, this.transform.position);
        }
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
        }
    }
#endif
    public void InitPool()
    {
    }

    public void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
    }
}
