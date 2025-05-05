using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client {
    sealed class DeadSlowSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MomentDeadEvent>,Exc<PlayerComponent>> _filter;
        readonly EcsFilterInject<Inc<WaveIndex>> _waveIndexFilter = default;
        readonly EcsSharedInject<GameState> _state;
        readonly EcsFilterInject<Inc<TestGameplayComponent>> _filterTest;

        public override MainEcsSystem Clone()
        {
            return new DeadSlowSystem();
        }
        public override void Run (IEcsSystems systems) 
        {
            if (_filterTest.Value.GetEntitiesCount() > 0) return;
            foreach (var entity in _filter.Value)
            {
                var SlowConfig = ConfigModule.GetConfig<ViewConfig>().SlowVisualConfig;
                SlowConfig.AddSlow(SlowConfig.DeadEnemy);
                if (_waveIndexFilter.Value.GetEntitiesCount() == 0)
                {
                    SlowConfig.AddSlow(SlowConfig.LastEnemyWave);

                    var state = BattleState.Instance;

                    if (state.IndexWave == state.CurrentRoom.RoomConfig.enemyWaves.Count)
                    {
                        SlowConfig.AddSlow(SlowConfig.LastEnemyRoom);
                    }
                }
            }
        }
    }
}