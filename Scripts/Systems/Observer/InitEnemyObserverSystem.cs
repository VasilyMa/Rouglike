using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class InitEnemyObserverSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<EnemyComponent, InitUnitEvent, HealthComponent>,Exc<BossComponent>> _fitler = default;
        readonly EcsPoolInject<EnemyObserverComponent> _enemyObserverPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsFilterInject<Inc<TestGameplayComponent>> _filterTest;

        public override MainEcsSystem Clone()
        {
            return new InitEnemyObserverSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            if (_filterTest.Value.GetEntitiesCount() > 0) return;
            foreach (var entity in _fitler.Value)
            {
                ref var enemyObserverComp = ref _enemyObserverPool.Value.Add(entity);

                enemyObserverComp.EnemyHealthbarValue = new UniRx.ReactiveProperty<HealthValue>();
                enemyObserverComp.EnemyPositionBarValue = new UniRx.ReactiveProperty<Vector2>();
                enemyObserverComp.EnemyToughnessbarValue = new UniRx.ReactiveProperty<ToughnessValue>();
                enemyObserverComp.Observer = new EnemyObserver(enemyObserverComp.EnemyHealthbarValue, enemyObserverComp.EnemyToughnessbarValue, enemyObserverComp.EnemyPositionBarValue);
                ObserverEntity.Instance.AddUnit(enemyObserverComp.Observer);
            }
        }
    }
}