using AbilitySystem;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitResolveBlocksAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<InitAbilityEvent, AbilityComponent>> _filter = default;
        readonly EcsPoolInject<ResolveBlocksAbilityComponent> _resolveBlocks = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitResolveBlocksAbilitySystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                if(abilityComp.Ability.SourceAbility.ResolveBlocks.Count > 0)
                {
                    ref var resolveBlockComp = ref _resolveBlocks.Value.Add(entity);
                    resolveBlockComp.Components = new System.Collections.Generic.List<IAbilityEffect>();
                    resolveBlockComp.OriginalComponents = new System.Collections.Generic.List<IAbilityEffect>();
                    foreach (var item in abilityComp.Ability.SourceAbility.ResolveBlocks)
                    {
                        foreach(var comp in item.Components)
                        {
                            resolveBlockComp.Components.Add(comp);
                            resolveBlockComp.OriginalComponents.Add(comp);
                        }
                    }
                }
                
            }
        }
    }
}