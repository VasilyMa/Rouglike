using Client;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class AttackRoomMB : MonoBehaviour
{
    public EcsPackedEntity entityAttackZone;
    public int IndexAttackZone;
    private List<EcsPackedEntity> listEntityAtZone = new();
    private EcsWorld _world;
    private EcsPool<UnitCollisionEvent> _unitCollisionPool = default;
    private void Start()
    {
       gameObject.SetActive(false);
    }
    public void Invoke(EcsWorld world, int entityZone)
    {
        listEntityAtZone.Clear();
        _world = world;
        ref var attackRoomComp = ref _world.GetPool<AttackRoomComponent>().Add(entityZone);
        attackRoomComp.attackZone = this;
        ref var transformComponent = ref _world.GetPool<TransformComponent>().Add(entityZone);
        transformComponent.Transform = transform;
        entityAttackZone = _world.PackEntity(entityZone);
        gameObject.SetActive(true);
        _unitCollisionPool = world.GetPool<UnitCollisionEvent>();
    }
    public void Dispose()
    {
        gameObject.SetActive(false);
        if (!entityAttackZone.Unpack(_world, out int unpuckEntityAttack)) return;
        _world.DelEntity(unpuckEntityAttack);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                AddEntityToServiceList(other);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                AddEntityToServiceList(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                RemoveEntityToServiceList(other);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                RemoveEntityToServiceList(other);
            }
        }
    }
    public void AllCollisionUnit()
    {
        if (!entityAttackZone.Unpack(_world, out int entityAttack)) return;
        if (!_unitCollisionPool.Has(entityAttack)) _unitCollisionPool.Add(entityAttack);
        ref var unitCollisionComp = ref _unitCollisionPool.Get(entityAttack);
        unitCollisionComp.CollisionEntity = new(listEntityAtZone);
        unitCollisionComp.SenderPackedEntity = entityAttackZone;
    }
    public void AddEntityToServiceList(Collider other)
    {
        if (other.TryGetComponent<UnitMB>(out UnitMB unitMB))
        {
            EcsPackedEntity entity = _world.PackEntity(unitMB._entity);
            if (!listEntityAtZone.Contains(entity)) listEntityAtZone.Add(entity);
        }
    }
    public void RemoveEntityToServiceList(Collider other)
    {
        if (other.TryGetComponent<UnitMB>(out UnitMB unitMB))
        {
            EcsPackedEntity entity = _world.PackEntity(unitMB._entity);
            if (listEntityAtZone.Contains(entity)) listEntityAtZone.Remove(entity);
        }
    }
}
