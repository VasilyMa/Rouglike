using AbilitySystem;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.Collections.LowLevel.Unsafe;

namespace Client {
    sealed class ChangeResolveMissileSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<ChangeResolveMissileComponent, ResolveBlockComponent>> _filter;
        readonly EcsPoolInject<ChangeResolveMissileComponent> _changeResolve;
        readonly EcsPoolInject<ResolveBlockComponent> _resolveBlockPool;
        readonly EcsPoolInject<NextMissileComponent> _nextMissilePool;

        public override MainEcsSystem Clone()
        {
            return new ChangeResolveMissileSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var changeResolveBlock =  ref _changeResolve.Value.Get(entity);
                ref var resolveBlockComp = ref _resolveBlockPool.Value.Get(entity);
                resolveBlockComp.Components = new System.Collections.Generic.List<AbilitySystem.IAbilityEffect>(changeResolveBlock.ResolveBlocks);
                foreach (var comp in resolveBlockComp.Components)
                {
                    comp.Recalculate(changeResolveBlock.charge);
                }
                _changeResolve.Value.Del(entity);
                _nextMissilePool.Value.Add(entity);  
            }
        }
    }
}