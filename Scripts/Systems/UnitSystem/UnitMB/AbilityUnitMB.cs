using Client;
using DG.Tweening;
using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Statement;

public class AbilityUnitMB: UnitMB // ���
{
    [HideInInspector] public int ComboCount = 0; // �����
    public WeaponConfig WeaponConfig; // �����
    [HideInInspector] public Animator Animator; // ����� �����
    [HideInInspector] public AnimatorOverrideController TemporaryAnimatorOverrideController; // ����� �����
    [HideInInspector] public AnimationClipOverrides ClipOverrides; // ����� �����
    public MeshFilter WeaponMeshFilter; // �����
    public List<AbilityBase> NonAttackAbilities;
    private EcsPool<LockAfterCast> _lockAfterCastPool;
    private EcsPool<TelegraphingUnitComponent> _telegraphingPool;
    [HideInInspector] public Dictionary<string, List<EcsPackedEntity>> AllAbilities = new Dictionary<string, List<EcsPackedEntity>>();
    public string CurrentAbility;
    public List<Modifier> TEST_MODIFIERS_DELETE_AFTER_TEST = new List<Modifier>();
    public List<Perk> TEST_PERKS_DELETE_AFTER_TEST = new List<Perk>();
    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    public override void Init(int entity)
    {
        base.Init(entity);
        _lockAfterCastPool = _world.GetPool<LockAfterCast>();
        _telegraphingPool = _world.GetPool<TelegraphingUnitComponent>();
        ResetCombo();
    }
    public List<int> GetAbilitiesListByActionNameTemp(string actionName)
    {
        List<int> list = new List<int>();
        if(AllAbilities.ContainsKey(actionName))
        {
            foreach (EcsPackedEntity packedEntity in AllAbilities[actionName])
            {
                if (packedEntity.Unpack(BattleState.Instance.EcsRunHandler.World, out int entity))
                {
                    list.Add(entity);
                }
            }
        }
        
        return list;
    }
    public List<int> GetAllAbilitiesEntities()
    {
        List<int> list = new List<int>();
        foreach (var value in AllAbilities.Values)
        {
            foreach (EcsPackedEntity packedEntity in value)
            {
                if (packedEntity.Unpack(BattleState.Instance.EcsRunHandler.World, out int entity))
                {
                    list.Add(entity);
                }
            }
        }
        return list;
    }    
    public EcsPackedEntity GetAbilitiesListByActionName(string actionName)
    {
        return AllAbilities[actionName][0];
    }
    public void ClearAllData()
    {
        var world = BattleState.Instance.EcsRunHandler.World;
        foreach (var item in AllAbilities)
        {
            foreach (var packedEntity in item.Value)
            {
                if(packedEntity.Unpack(world, out var entity))
                BattleState.Instance.EcsRunHandler.World.DelEntity(entity);
            }
        }
        AllAbilities.Clear();
    }

    public void ResetCombo() => ComboCount = 0;
    public void AddCombo() => ComboCount++;

    List<Type> ObjectComponents = new() {
            typeof(AbilityUnitMB), typeof(SoundUnitMB), typeof(TelegraphingUnitMB), typeof(PhysicsUnitMB), typeof(Animator),
            typeof(NavMeshAgent),typeof(CapsuleCollider),typeof(Rigidbody)
        };
    List<Type> ObjectChild = new(){
            typeof(SkinnedMeshRenderer), typeof(AttackZone)
        };
    [ExecuteInEditMode]
#if UNITY_EDITOR
    protected override void OnValidate()
    {

        /*CheckComponentAtUnit(ObjectComponents.ToArray());
        CheckChildrenComponent(ObjectChild.ToArray());*/
        base.OnValidate();

    }
#endif
    public void CheckComponentAtUnit(params Type[] types)
    {
        var components = gameObject.GetComponents<Component>();
        foreach (var type in types)
        {
            bool flag = true;
            foreach (var component in components)
            {
                if (component.GetType() == type)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                
            }
        }
    }
    public void CheckChildrenComponent(params Type[] types)
    {
        foreach (var type in types)
        {
            bool flag = true;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                var components = child.GetComponents<Component>();

                foreach (var component in components)
                {
                    if (component.GetType() == type)
                    {
                        flag = false;
                    }
                }
            }
            if (flag)
            {
                
            }
        }
    }
}
