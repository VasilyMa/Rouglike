using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AddAbilityEffectByModifierSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AddAbilityEffect>> _filter = default;
        readonly EcsPoolInject<ResolveBlocksAbilityComponent> _resolveBlockPool = default;
        readonly EcsPoolInject<AddAbilityEffect> _modifierPool = default;
        public override MainEcsSystem Clone()
        {
            return new AddAbilityEffectByModifierSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //remove this if it not used
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var modifierComp = ref _modifierPool.Value.Get(entity);
                ref var resolveComp = ref _resolveBlockPool.Value.Get(modifierComp.AbilityEntity);
                resolveComp.Components.Add(modifierComp.Effect);
            }
        }
    }
}