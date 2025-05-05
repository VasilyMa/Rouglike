using System.Collections.Generic;
using Client;
using UnityEngine;
using Leopotam.EcsLite;
using AbilitySystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using Statement;

public class UnitMB : MonoBehaviour, IPool
{
    protected BattleState _state = null;
    [HideInInspector] public int _entity; 
    protected EcsWorld _world = null;
    private GroupUnitMB groupUnitMB;

    protected bool isAvaiable;
    public GameObject ThisGameObject => gameObject;

    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KEY_ID;
    [HideInInspector]public string KEY_ID;

    public virtual void Init(int entity)
    {
        _entity = entity;
        _state = BattleState.Instance;
        _world = _state.EcsRunHandler.World;
    }
    public Template GetUnitMB<Template>() where Template : UnitMB
    {
        foreach (var unitMB in groupUnitMB._unitMBs)
        {
            if (unitMB is Template) return unitMB as Template;
        }
        return null;
    }
    public bool CheckUnitMB<Template>() where Template : UnitMB
    {
        foreach (var unitMB in groupUnitMB._unitMBs)
        {
            if (unitMB is Template) return true;
        }
        return false;
    }
    public GroupUnitMB SetGroupUnitMB { set { groupUnitMB = value; } }

    public void InitPool()
    {

    }

    public virtual void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
        }
    }
#endif
}
