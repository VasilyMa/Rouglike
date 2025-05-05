using Client;

using Statement;
using Leopotam.EcsLite;

public class DodgeAmplifier : SourceRelic
{
    public int DodgeChargeAmplifier;

    public override void InvokeRelic()
    {
        if (State.Instance == null || State.Instance.EcsRunHandler.World == null) return;

        var state = State.Instance;

        var world = state.EcsRunHandler.World;

        ref var inputComp = ref world.GetPool<InputComponent>().Get(state.GetEntity("InputEntity"));
        ref var unitComp = ref world.GetPool<AbilityUnitComponent>().Get(state.GetEntity("PlayerEntity"));

        var dodge = unitComp.AbilityUnitMB.GetAbilitiesListByActionName(inputComp.InputAction.ActionMap.Dash.name);

        if (dodge.Unpack(world, out int entity))
        {
            ref var chargeComponent = ref world.GetPool<ChargePointComponent>().Get(entity);
            chargeComponent.MaxChargeCount++;
            chargeComponent.CurrentChargeCount = chargeComponent.MaxChargeCount;
            chargeComponent.OnChargePointChange?.Invoke(chargeComponent.CurrentChargeCount);
        }
    }
}
