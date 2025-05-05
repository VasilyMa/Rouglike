using System.Collections;
using System.Collections.Generic;

using Client;

using UnityEngine;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite;
using UnityEngine.VFX;
using Statement;

[RequireComponent(typeof(LineRenderer))]
public class LightningMissile : MonoBehaviour, IPool
{
    [HideInInspector] public float DamageValue; 
    [HideInInspector] public int MaxTargets;
    [HideInInspector] public float Radius;
    [SerializeField] SourceParticle HitEffect;
    private List<UnitMB> _targets = new List<UnitMB>();
    private LineRenderer _lineRenderer;

    protected bool isAvaiable;
    public GameObject ThisGameObject => gameObject;

    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KEY_ID;
    public string KEY_ID;

    public void Invoke()
    {
        if(_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();

        gameObject.SetActive(true);

        var world = State.Instance.EcsRunHandler.World;

        FillTargetList(transform.position);

        _lineRenderer.positionCount = _targets.Count + 1;

        ref var transformComp = ref world.GetPool<TransformComponent>().Get(State.Instance.GetEntity("PlayerEntity"));

        Vector3 startPos = transformComp.Transform.position;
        startPos.y = startPos.y + 0.5f;

        

        _lineRenderer.SetPosition(0, startPos);

        for (int i = 0; i < _targets.Count; i++)
        {
            var target = _targets[i];

            ref var telegraphComp = ref world.GetPool<TelegraphingUnitComponent>().Get(target._entity);

            var targetTransform = telegraphComp.TelegraphingUnitMB.GetMemberOfBodyByType(MemberTypes.Body);

            if (targetTransform == null) targetTransform = world.GetPool<TransformComponent>().Get(target._entity).Transform;     

            Vector3 pos = targetTransform.position;

            var hit = PoolModule.Instance.GetFromPool<SourceParticle>(HitEffect, true);
            hit.AttachVisualEffectToEntity(pos, Quaternion.identity, world.PackEntity(target._entity));

            _lineRenderer.SetPosition(i + 1, pos);

            
            //а почему всегда PlayerEntity????
            ref var takeDamageComp = ref world.GetPool<TakeDamageComponent>().Add(world.NewEntity());
            takeDamageComp.Damage = DamageValue;
            takeDamageComp.KillerEntity = world.PackEntity(State.Instance.GetEntity("PlayerEntity"));
            takeDamageComp.TargetEntity = world.PackEntity(target._entity);
            // if (!world.GetPool<TakeDamageComponent>().Has(target._entity))
            // {
            //     world.GetPool<TakeDamageComponent>().Add(target._entity).AddDamage(DamageValue, world.PackEntity(GameState.Instance.PlayerEntity));
            // }
            // else
            // {
            //     world.GetPool<TakeDamageComponent>().Get(target._entity).AddDamage(DamageValue, world.PackEntity(GameState.Instance.PlayerEntity));
            // }
        }
    }

    public void FinishMissile()
    {
        gameObject.SetActive(false);
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
    private void FillTargetList(Vector3 targetPosition)
    {
        _targets.Clear();

        var world = State.Instance.EcsRunHandler.World;

        Collider[] targetsInRange = new Collider[MaxTargets * 2];

        int count = Physics.OverlapSphereNonAlloc(targetPosition, Radius, targetsInRange, LayerMask.GetMask("Enemy"));

        

        foreach (var target in targetsInRange)
        {
            if (target == null) continue;

            if (target.transform.TryGetComponent<UnitMB>(out var unit))
            {
                if (world.GetPool<DeadComponent>().Has(unit._entity)) continue;

                _targets.Add(unit);
            }
        }
    }

    public void InitPool()
    {
    }

    public void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
    }
}
