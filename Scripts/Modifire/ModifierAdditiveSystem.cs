using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class ModifierAdditiveSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DamageModifier, AdditiveModifier>> _filter = default;
        readonly EcsPoolInject<DamageModifier> _modifierPool = default;
        readonly EcsPoolInject<ResolveBlocksAbilityComponent> _resolveBlockPool = default;
        public override MainEcsSystem Clone()
        {
            return new ModifierAdditiveSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var modifierComp = ref _modifierPool.Value.Get(entity);
                ref var resolveComp = ref _resolveBlockPool.Value.Get(modifierComp.AbilityEntity);
                for (int i = 0; i < resolveComp.Components.Count; i++)
                {
                    if(resolveComp.Components[i] is DamageEffect effect)
                    {
                        
                        effect.AdditiveModifiers += modifierComp.Modifier.Value;
                        

                        resolveComp.Components[i] = effect;
                    }
                }
            }
        }
    }
}