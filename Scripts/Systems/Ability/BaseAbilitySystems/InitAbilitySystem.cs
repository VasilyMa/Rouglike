using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using AbilitySystem;

namespace Client {
    sealed class InitAbilitySystem : MainEcsSystem {
        /// <summary>
        /// Init new Ability
        /// </summary>
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InitAbilityEvent>> _filter = default;
        readonly EcsPoolInject<InitAbilityEvent> _initAbilityPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<VisualEffectsComponent> _visualEffectPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitAbilitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                //проверка на существование владельца
                ref var initAbilityComp = ref _initAbilityPool.Value.Get(entity);
                if(initAbilityComp.PackedEntity.Unpack(_world.Value, out int unpackedEntityOwner))
                {
                    ref var abilityComp = ref _abilityPool.Value.Add(entity);
                    abilityComp.Ability = initAbilityComp.AbilityBase;
                    
                    ref var ownerComp = ref _ownerPool.Value.Add(entity);
                    ownerComp.OwnerEntity = initAbilityComp.PackedEntity;

                    ref var visualEffectComp = ref _visualEffectPool.Value.Add(entity);
                    visualEffectComp.Init();
                }
                else
                {
                    _initAbilityPool.Value.Del(entity);
                }
            }
        }
        
    }
}