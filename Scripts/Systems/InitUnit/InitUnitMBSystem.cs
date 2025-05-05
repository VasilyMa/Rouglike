using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    /// <summary>
    /// We take the Mono from the unit
    /// </summary>
    sealed class InitUnitMBSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<InitUnitEvent,ViewComponent>, Exc<AbilityUnitComponent>> _filter;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool;
        readonly EcsPoolInject<PhysicsUnitComponent> _physicsUnitPool;
        readonly EcsPoolInject<TelegraphingUnitComponent> _telegraphingUnitPool;
        readonly EcsPoolInject<SoundUnitComponent> _soundUnitPool;
        readonly EcsPoolInject<ViewComponent> _viewPool;

        public override MainEcsSystem Clone()
        {
            return new InitUnitMBSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                var GO = viewComp.GameObject;
                //groupUnit
                if (!GO.TryGetComponent<GroupUnitMB>(out GroupUnitMB GUMB))
                    GO.AddComponent<GroupUnitMB>();
                //ability
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Add(entity);
                abilityUnitComp.AbilityUnitMB = GO.GetComponent<AbilityUnitMB>();
                abilityUnitComp.AbilityUnitMB.Init(entity);

                
                //sound
                ref var soundUnitComp = ref _soundUnitPool.Value.Add(entity);
                soundUnitComp.SoundUnitMB = GO.GetComponent<SoundUnitMB>();
                soundUnitComp.SoundUnitMB.Init(entity);
                //phisics
                ref var physicsUnitComp = ref _physicsUnitPool.Value.Add(entity);
                physicsUnitComp.PhysicsUnitMB = GO.GetComponent<PhysicsUnitMB>();
                physicsUnitComp.PhysicsUnitMB.Init(entity);
            }
        }
    }
}