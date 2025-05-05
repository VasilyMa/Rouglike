using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Client {
    sealed class UpdateAlliesContextSystem : MainEcsSystem
    {
        private readonly EcsFilterInject<Inc<UnitBrain, AlliesContext>> _filter = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<AlliesContext> _supportContextPool = default;
        private readonly EcsPoolInject<SelfContext> _selfContextPool = default;
        private readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateAlliesContextSystem();
        }

        public override void Run (IEcsSystems systems) {
            //TODOihor think about good entity to entity distance algorythm without dictionaries
            foreach (int supportEntity in _filter.Value)
            {
                ref var supportTransform = ref _transformPool.Value.Get(supportEntity);
                ref var alliesContext = ref _supportContextPool.Value.Get(supportEntity);
                foreach (var pair in alliesContext.alliedEntitiesWithDistance.ToList())
                {
                    if (pair.Key.Unpack(_world.Value, out int entity))
                    {
                        if (_transformPool.Value.Has(entity))
                        {
                            ref var allyTransform = ref _transformPool.Value.Get(entity);
                            float distance = Vector3.Distance(allyTransform.Transform.position, supportTransform.Transform.position);
                            alliesContext.alliedEntitiesWithDistance[pair.Key] = distance;
                        }
                    }
                }
            }
        }
    }
}