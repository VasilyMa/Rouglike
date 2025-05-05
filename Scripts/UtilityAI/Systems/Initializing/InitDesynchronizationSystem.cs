using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class InitDesynchronizationSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<UnitBrain, InitContextEvent>,Exc<DesynchronizationComponent, DelDesynchronizationComponentcs>> _filter = default;
        readonly EcsPoolInject<DesynchronizationComponent> _desyncPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitDesynchronizationSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var desyncComp = ref _desyncPool.Value.Add(unitEntity);
                desyncComp.DesynchronizationValue = Random.Range(0f, 3f);
            }
        }
    }
}