using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class AIDesynchronizationSystem : MainEcsSystem {
        readonly private EcsFilterInject<Inc<UnitBrain,DesynchronizationComponent>> _filter = default;
        readonly private EcsPoolInject<DesynchronizationComponent> _desynchronizationPool = default;

        public override MainEcsSystem Clone()
        {
            return new AIDesynchronizationSystem();
        }

        public override void Run (IEcsSystems systems) {
            // add your run code here.
            foreach(var entity in _filter.Value)
            {
               ref var desyncComp = ref _desynchronizationPool.Value.Get(entity);
                if (desyncComp.DesynchronizationValue > 0)
                    desyncComp.DesynchronizationValue -= Time.deltaTime;
                else
                    _desynchronizationPool.Value.Del(entity);
            }
        }
    }
}