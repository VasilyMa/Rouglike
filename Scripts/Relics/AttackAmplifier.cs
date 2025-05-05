using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite;
using Client;
using Statement;

public class AttackAmplifier : SourceRelic
{
    public int AttackTargetCount;
    [SerializeReference] public EffectAmplifier effect;

    public override void InvokeRelic()
    {
        if (State.Instance == null || State.Instance.EcsRunHandler.World == null) return;

        var state = State.Instance;

        var world = state.EcsRunHandler.World; ;

        ref var inputComp = ref world.GetPool<InputComponent>().Get(state.GetEntity("InputEntity"));
        ref var unitComp = ref world.GetPool<AbilityUnitComponent>().Get(state.GetEntity("PlayerEntity"));

        var shield = unitComp.AbilityUnitMB.GetAbilitiesListByActionName(inputComp.InputAction.ActionMap.Attack.name);

        if (shield.Unpack(world, out int entityAbility))
        {
            if (world.GetPool<AttackEffectComponent>().Has(entityAbility))
            {
                ref var attackEffectComponent = ref world.GetPool<AttackEffectComponent>().Get(entityAbility);
                attackEffectComponent.AddEffect(effect);
            }
            else
            {
                ref var attackEffectComponent = ref world.GetPool<AttackEffectComponent>().Add(entityAbility);
                attackEffectComponent.TargetAttackCount = AttackTargetCount;
                attackEffectComponent.Effects = new List<EffectAmplifier>();
                attackEffectComponent.AddEffect(effect);
            }
        }
    }
}

[System.Serializable]
public abstract class EffectAmplifier
{
    public string EffectName;
    public abstract EffectAmplifier Clone();
    public virtual void InvokeEffect()
    {
        var world = State.Instance.EcsRunHandler.World;
        world.GetPool<InvokeRelicEvent>().Add(world.NewEntity()).SetEffect(this);
    }
    public abstract void ResolveEffect();
    public abstract void IncreaseEffect();
}

public class ChainLightningEffect : EffectAmplifier
{
    public float DamageValue;
    public int MaxTarget;
    public float Radius;
    public float TimeLifeLightning;
    public LightningMissile LightningMissile;

    public ChainLightningEffect(ChainLightningEffect data)
    {
        MaxTarget = data.MaxTarget;
        Radius = data.Radius;
        DamageValue = data.DamageValue;
        LightningMissile = data.LightningMissile;
        TimeLifeLightning = data.TimeLifeLightning;
    }

    public override EffectAmplifier Clone()
    {
        return new ChainLightningEffect(this);
    }

    public override void IncreaseEffect()
    {
        MaxTarget++;

        DamageValue += (DamageValue * 0.1f);
    }

    public override void ResolveEffect()
    {
        var world = State.Instance.EcsRunHandler.World;

        var entityMissile = world.NewEntity();

        ref var missileEvent = ref world.GetPool<LightningMissileEvent>().Add(entityMissile);

        missileEvent.Missile = PoolModule.Instance.GetFromPool<LightningMissile>(LightningMissile, true);
    
        missileEvent.Missile.transform.position = world.GetPool<TransformComponent>().Get(State.Instance.GetEntity("PlayerEntity")).Transform.position;
        missileEvent.TimeLife = TimeLifeLightning;
        missileEvent.Missile.Radius = Radius;
        missileEvent.Missile.DamageValue = DamageValue;
        missileEvent.Missile.MaxTargets = MaxTarget;
        missileEvent.Missile.Invoke();
    }
}

public class ExplosionEffect : EffectAmplifier
{
    public float DamageValue;

    public ExplosionEffect(ExplosionEffect data)
    {
        DamageValue = data.DamageValue;
    }

    public override EffectAmplifier Clone()
    {
        return new ExplosionEffect(this);
    }

    public override void IncreaseEffect()
    {

    }

    public override void ResolveEffect()
    {

    }
}