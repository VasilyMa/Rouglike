using System.Collections.Generic;

using Client;

using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CollisionMissileMB : MissileMB
{
    public bool IsOnceCollision;
    [Range(0, 10)] public float radius;
    protected SphereCollider _sphereCollider;
    [HideInInspector] public bool Collision;
    private void Update()
    {
        Collision = false;
    }
    public override void Invoke(EcsWorld world, int entity, int layerMask)
    {
        base.Invoke(world, entity, layerMask);
        layerMaskTarget = layerMask;
        if (!world.GetPool<ColliderComponent>().Has(entity)) world.GetPool<ColliderComponent>().Add(entity);
        ref var colliderComp = ref world.GetPool<ColliderComponent>().Get(entity);
        colliderComp.Collider = GetComponent<SphereCollider>();
        Collision = false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerMaskTarget) return;
        if (other.TryGetComponent<UnitMB>(out var unit))
        {
            EcsPackedEntity unitPackedEntity = _world.PackEntity(unit._entity);
                if (!_world.GetPool<UnitCollisionEvent>().Has(_entityMissile)) _world.GetPool<UnitCollisionEvent>().Add(_entityMissile);
                ref var unitCollisionComp = ref _world.GetPool<UnitCollisionEvent>().Get(_entityMissile);
                if(unitCollisionComp.CollisionEntity == null) unitCollisionComp.CollisionEntity = new List<EcsPackedEntity>();
                unitCollisionComp.CollisionEntity.Add(unitPackedEntity);
                unitCollisionComp.SenderPackedEntity = _world.PackEntity(_entityMissile);
            Collision = true;

                if (IsOnceCollision && !_world.GetPool<Dashing>().Has(unit._entity))
                {
                    if (!_world.GetPool<FinishMissileEvent>().Has(_entityMissile))
                        _world.GetPool<FinishMissileEvent>().Add(_entityMissile);
                }
        }
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        _sphereCollider.radius = radius;
        KEY_ID = name;
    }
#endif
}
