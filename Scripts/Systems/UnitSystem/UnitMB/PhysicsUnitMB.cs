using Client;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Statement;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine.UIElements;


public class PhysicsUnitMB : UnitMB // ��� ����� ���
{
    public Collider SoulsCollector; // �����
    private EcsPool<TransformComponent> _transformPool = null;
    private EcsPool<WeaponComponent> _weaponPool = null;
    [HideInInspector] public Rigidbody[] RagDollRigidBody;
    [HideInInspector] public Collider[] RagDollCollider;
    [HideInInspector] public Transform Transform;
    [HideInInspector] public Rigidbody Rigidbody;
    [HideInInspector] public Collider Collider;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public Material Material;
    private ParticleSystem _particleSystem;
    [HideInInspector] public List<VFXOnBodyMB> _VFXOnBody = new();
    private ViewConfig _viewConfig;
    [HideInInspector] public List<WeaponMB> weaponMBs;
    private void Awake()
    {
        Transform = GetComponent<Transform>();
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        RagDollCollider = GetComponentInChildren<RootMB>().GetComponentsInChildren<Collider>();
        RagDollRigidBody = GetComponentInChildren<RootMB>().GetComponentsInChildren<Rigidbody>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Material = GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial; // TODO get enemy material somewhere else
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        weaponMBs = new List<WeaponMB>();
    }
    public override void Init(int entity)
    {
        base.Init(entity);
        _transformPool = _world.GetPool<TransformComponent>();
        _weaponPool = _world.GetPool<WeaponComponent>();
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = false;
        ActivateRagDoll(false);
    }
    public void SetMaterialWeapon(Material material)
    {
        foreach (var weapon in weaponMBs)
            weapon.renderer.material = material;
    }
    public void DissolutionWeapon(float value)
    {
        foreach (var weapon in weaponMBs)
            weapon.renderer.material.SetFloat("_DissolveAmount", value);
    }
    public void ActivateRagDoll(bool flag)
    {
        foreach (var collider in RagDollCollider)
        {
            collider.enabled = flag;
        }
        foreach (var rigidBody in RagDollRigidBody)
        {
            rigidBody.isKinematic = !flag;
            rigidBody.useGravity = flag;
        }
        Rigidbody.useGravity = flag;
        Collider.enabled = !flag;
    }
    public void ActiveVFXOnBody(int indexVFX)
    {
        _VFXOnBody.FindAll(x => x.IndexVFX == indexVFX).ForEach(x => x.Play());
    }
    public void KillUnit()
    {
        ActivateRagDoll(true);
        GetUnitMB<AbilityUnitMB>().Animator.enabled = false;
        //NavMeshAgent.enabled = false;
        _particleSystem?.Play();
        _VFXOnBody.ForEach(x => x.Dispose());
    }
    public void UnitReset()
    {
        foreach (var weapon in weaponMBs)
            weapon.WeaponReset();
        //NavMeshAgent.enabled = true;
        GetUnitMB<AbilityUnitMB>().Animator.enabled = true;
        ActivateRagDoll(false);
        _particleSystem?.Stop();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
    }
#endif

}
