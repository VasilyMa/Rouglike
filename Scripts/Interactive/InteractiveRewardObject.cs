using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Client;
using Statement;
using System;

public class InteractiveRewardObject : InteractiveObject
{
    public int FavourDropAmount;
    public int EffigiesDropAmount;

    private Animator _animator;
    public Action ActionDrop;
    private ReactiveProperty<bool> _useValue = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> OnUseValueChange => _useValue;

    public DropConfig _viewConfig;

    protected override void OnEnable()
    {
        base.OnEnable();
        ActionDrop += DropItem;
    }
    protected override void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        _useValue.Dispose();
        ActionDrop -= DropItem;
        
    }

    protected override void InputButton()
    {
        base.InputButton();

        ActionDrop?.Invoke();
    }

    protected override void Init()
    { 

    }

    protected override void Dispose()
    {

    }

    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
    }

    public override void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);
    }

    void DropItem()
    {
        _animator?.Play("ChestAnimation");
        var drops = _viewConfig.GetDropLoot();
        foreach (var drop in drops)
        {
            DropLoot(drop);
        }

        ActionDrop = null;

        _useValue.Value = true;
    }

    void DropLoot(IDrop dropLoot, int modifyAmount = 1)
    {
        ref var drop = ref State.Instance.EcsRunHandler.World.GetPool<DropEvent>().Add(State.Instance.EcsRunHandler.World.NewEntity());
        drop.DropPosition = this.transform.position;
        drop.DropPosition.y = this.transform.position.y + 2;
        drop.EndPosition = this.transform.position + UnityEngine.Random.insideUnitSphere + Vector3.right * 2;
        drop.dropItem = dropLoot;
    }

}
