using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;


namespace Client 
{
    sealed class DelEntitySystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DelEntityEvent>> _filter = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;

        public override MainEcsSystem Clone()
        {
            return new DelEntitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                if (_transformPool.Value.Has(entity))
                {
                    ref var transformComp = ref _transformPool.Value.Get(entity);
                    transformComp.Transform.gameObject.SetActive(false);
                }
                if (_abilityUnitPool.Value.Has(entity))
                {
                    ref var abilityComp = ref _abilityUnitPool.Value.Get(entity);
                    abilityComp.AbilityUnitMB.ReturnToPool(); 
                }
                _world.Value.DelEntity(entity);
            }
        }
    }
}