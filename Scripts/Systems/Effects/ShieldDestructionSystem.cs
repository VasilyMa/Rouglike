using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.VisualScripting;

namespace Client {
    sealed class ShieldDestructionSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<ShieldDestructionEvent, ShieldsContainer>> _filter;
        readonly EcsPoolInject<ShieldDestructionEvent> _shieldDestructionPool;
        readonly EcsPoolInject<ShieldsContainer> _shieldsContainerPool;

        public override MainEcsSystem Clone()
        {
            return new ShieldDestructionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var shieldDestructionComp = ref _shieldDestructionPool.Value.Get(entity);
                ref var shieldContainer = ref _shieldsContainerPool.Value.Get(entity);

                foreach(var shield in shieldDestructionComp.shields)
                {
                    shield.InstantiatedSourceParticle.Dispose();
                    shieldContainer.shieldComponents.Remove(shield);
                }
                if (shieldContainer.shieldComponents.Count == 0) _shieldsContainerPool.Value.Del(entity);
            }
        }
    }
}