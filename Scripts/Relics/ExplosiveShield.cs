using Client;

using Statement;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite;
using UnityEngine;
public class ExplosiveShield : SourceRelic
{
    [SerializeField] SourceParticle ExpolosiveEffect;
    [Range(0, 1f)] public float LevelDamageAmplifier = 0f;
    public float DamageValue;
    public float Radius;
    public LayerMask LayerMask;
    public override void InvokeRelic()
    {
        if (State.Instance.EcsRunHandler == null || State.Instance.EcsRunHandler.World == null) return;

        var state = State.Instance;

        var world = state.EcsRunHandler.World;

        ref var inputComp = ref world.GetPool<InputComponent>().Get(state.GetEntity("InputEntity"));
        ref var unitComp = ref world.GetPool<AbilityUnitComponent>().Get(state.GetEntity("PlayerEntity"));

        var shield = unitComp.AbilityUnitMB.GetAbilitiesListByActionName(inputComp.InputAction.ActionMap.UtilityAbility.name);

        if (shield.Unpack(world, out int entityAbility))
        {
            if (world.GetPool<ExplosiveAbilityComponent>().Has(state.GetEntity("PlayerEntity")))
            {
                ref var explosiveShieldComponent = ref world.GetPool<ExplosiveAbilityComponent>().Get(state.GetEntity("PlayerEntity"));
                explosiveShieldComponent.DamageValue += (explosiveShieldComponent.DamageValue * LevelDamageAmplifier);
                explosiveShieldComponent.Level++;
            }
            else
            {
                ref var explosiveShieldComponent = ref world.GetPool<ExplosiveAbilityComponent>().Add(state.GetEntity("PlayerEntity"));
                explosiveShieldComponent.DamageValue = DamageValue;
                explosiveShieldComponent.Radius = Radius;
                explosiveShieldComponent.LayerMask = LayerMask;
                explosiveShieldComponent.ExpolosiveEffect = ExpolosiveEffect;
            }
        }
    }
}
