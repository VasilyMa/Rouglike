using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Leopotam.EcsLite;
using Statement;

public class ModifierRelic : SourceRelic
{
    [SerializeField] private Modifier _modifier;
    public override void InvokeRelic()
    {
        
        if (BattleState.Instance == null || BattleState.Instance.EcsRunHandler.World == null) return;

        var state = BattleState.Instance;

        var world = state.EcsRunHandler.World;

        ref var requestAddModifierComp = ref world.GetPool<RequestAddModifier>().Add(world.NewEntity());
        requestAddModifierComp.Modifier = _modifier;
        requestAddModifierComp.UnitPackedEntity = world.PackEntity(state.GetEntity("PlayerEntity"));
    }
}
