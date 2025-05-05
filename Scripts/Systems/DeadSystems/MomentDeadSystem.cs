using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client {
    sealed class MomentDeadSystem : MainEcsSystem
    {
        /// <summary>
        /// A system that adjusts the death of the enemy
        /// </summary>
        readonly EcsFilterInject<Inc<MomentDeadEvent>, Exc<DeadComponent>> _filter = default;
        readonly EcsFilterInject<Inc<MomentDeadEvent>, Exc<PlayerComponent>> _enemyDeadFilter = default;
        readonly EcsPoolInject<HighToughnessComponent> _highToughnessPool = default;
        readonly EcsPoolInject<IrrevocabilityComponent> _irrevocabilityPool = default;
        readonly EcsPoolInject<WaveIndex> _waveIndexPool;
        readonly EcsPoolInject<DeadComponent> _deadComp;
        readonly EcsPoolInject<MomentDeadEvent> _momentDeadPool;
        readonly EcsWorldInject _world;
        private GameConfig gameConfig;

        public override MainEcsSystem Clone()
        {
            return new MomentDeadSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            gameConfig = ConfigModule.GetConfig<GameConfig>();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value) 
            {
                ref var momentDeadComp = ref _momentDeadPool.Value.Get(entity);
                ref var deadComponent = ref _deadComp.Value.Add(entity);

                deadComponent.TimerToDestroy = 0;
                deadComponent.KillerEntity = momentDeadComp.killerEntity;
                deadComponent.TimeOfDeath = gameConfig.TimeOfDeath;

                if (_highToughnessPool.Value.Has(entity)) _highToughnessPool.Value.Del(entity);
                if (_irrevocabilityPool.Value.Has(entity)) _irrevocabilityPool.Value.Del(entity);
                if(_waveIndexPool.Value.Has(entity)) _waveIndexPool.Value.Del(entity);
            }
            foreach(var enemy in _enemyDeadFilter.Value)
            {
                BattleState.Instance.AddKill();
            }
        }
    }
}