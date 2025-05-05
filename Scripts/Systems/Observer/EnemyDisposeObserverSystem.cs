using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class EnemyDisposeObserverSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DisposeObserverComponent, EnemyComponent, EnemyObserverComponent>> _filter = default;
        readonly EcsFilterInject<Inc<DisposeObserverComponent, BossComponent>> _bossFilter = default;
        readonly EcsPoolInject<EnemyObserverComponent> _observerPool = default;
        readonly EcsPoolInject<DisposeObserverComponent> _disposePool = default;

        public override MainEcsSystem Clone()
        {
            return new EnemyDisposeObserverSystem();
        } 
        
        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var observerComp = ref _observerPool.Value.Get(entity);
                ObserverEntity.Instance.RemoveUnit(observerComp.Observer);
                
                _observerPool.Value.Del(entity);
                _disposePool.Value.Del(entity);
            }

            foreach (var entity in _bossFilter.Value)
            {
                ObserverEntity.Instance.RemoveBoss();
                _observerPool.Value.Del(entity);
                _disposePool.Value.Del(entity);
            }
        }
    }
}