using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;


namespace Client {
    sealed class VFXFullChargeSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<VFXFullChargeComponent>> _filter = default;
        readonly EcsPoolInject<VFXFullChargeComponent> _chargePool = default;

        public override MainEcsSystem Clone()
        {
            return new VFXFullChargeSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var chargeComp = ref _chargePool.Value.Get(entity);
            }
        }
    }
}