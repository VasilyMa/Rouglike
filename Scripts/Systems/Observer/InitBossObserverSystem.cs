using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class InitBossObserverSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<BossComponent, InitUnitEvent>> _fitler = default;
        readonly EcsPoolInject<BossObserverComponent> _bossObserverPool = default;
        readonly EcsPoolInject<InitUnitEvent> _initPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitBossObserverSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
#if UNITY_EDITOR
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TestScene") return;
#endif
            foreach (var entity in _fitler.Value)
            {
                ref var bossObserver = ref _bossObserverPool.Value.Add(entity);
                bossObserver.BossHealthValue = new UniRx.ReactiveProperty<HealthValue>();
                bossObserver.BossToughnessbarValue = new UniRx.ReactiveProperty<ToughnessValue>();
                bossObserver.BossStageValue = new UniRx.ReactiveProperty<BossStageValue> ();
                bossObserver.BossPositionBarValue = new UniRx.ReactiveProperty<Vector2> ();

                ref var initComp = ref _initPool.Value.Get(entity);

                var observer = new BossObserver();
                observer.SetHealthValue(bossObserver.BossHealthValue);
                observer.SetStageValue(bossObserver.BossStageValue);
                observer.SetToughnessValue(bossObserver.BossToughnessbarValue);
                observer.SetPositionValue(bossObserver.BossPositionBarValue);
                observer.SetBossValue(initComp.EnemyMetaConfig);

                ObserverEntity.Instance.AddBoss(observer);

                Statement.State.Instance.SendRequest(new GameRequest(Status.BossFight));
            }
        }
    }
}