using Client;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using DG.Tweening;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class AttackZone : MonoBehaviour, IPool
{
    
    private EcsWorld _world = null;
    public EcsPackedEntity _entity;
    private UnitMB Owner;
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;
    private List<EcsPackedEntity> _serviceEntityList = new List<EcsPackedEntity>();
    private EcsPool<UnitCollisionEvent> _unitCollisionPool = default;

    protected bool isAvaiable;
    public GameObject ThisGameObject => gameObject;

    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KEY_ID;
    public string KEY_ID;

    public void Init(EcsWorld world, int entity)
    {
        Owner = gameObject.transform.parent.GetComponent<UnitMB>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider.enabled = false;
        _unitCollisionPool = world.GetPool<UnitCollisionEvent>();
        _entity = world.PackEntity(entity);
        _world = world;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.gameObject.CompareTag("Enemy")) SendUnitCollision(other);
        }
        else
        {
            if (other.gameObject.CompareTag("Player")) SendUnitCollision(other);
        }
    }
    public void SetAttackZoneMesh(Mesh mesh)
    {
        meshCollider.sharedMesh = mesh;
    }
    public void AttackZoneEnable()
    {
        meshCollider.enabled = true;
    }
    public void AttackZoneDisable()
    {
        //todo 354 pool
        _serviceEntityList.Clear();
        meshCollider.enabled = false;
        ReturnToPool();
    }
    public void SendUnitCollision(Collider other)
    {
        if(other.TryGetComponent<UnitMB>(out UnitMB unitMB))
        {
            EcsPackedEntity unitEntity = _world.PackEntity(unitMB._entity);
            if(!_serviceEntityList.Contains(unitEntity)) 
            {
                if(_entity.Unpack(_world, out int entity))
                {
                    if (!_unitCollisionPool.Has(entity)) _unitCollisionPool.Add(entity);
                    ref var unitCollisionComp = ref _unitCollisionPool.Get(entity);
                    if(unitCollisionComp.CollisionEntity == null) unitCollisionComp.CollisionEntity = new List<EcsPackedEntity>();
                    unitCollisionComp.CollisionEntity.Add(unitEntity);
                    unitCollisionComp.SenderPackedEntity = _entity;
                }
                _serviceEntityList.Add(unitEntity);
            }
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
