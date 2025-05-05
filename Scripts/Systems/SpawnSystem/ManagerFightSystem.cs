using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.SceneManagement;
using Statement;

namespace Client {
    /// <summary>
    ///  The system reacts to the death of enemies, checks if all waves were forced,
    ///  if not, throws a New Wave Event
    ///  if so, throws a Stop Fight Event and deletes the FightRoomComponent
    /// </summary>
    sealed class ManagerFightSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<FightRoomComponent>,Exc<StopFightEvent>> _filterDead;
        readonly EcsSharedInject<GameState> _state;
        readonly EcsWorldInject _world;
        readonly EcsPoolInject<FightRoomComponent> _fightPool;
        readonly EcsPoolInject<StopFightEvent> _stopFightPool;
        readonly EcsPoolInject <SpawnWaveEvent> _spawnWavePool;

        public override MainEcsSystem Clone()
        {
            return new ManagerFightSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filterDead.Value) // NE SMOTRITE ETO XYETA
            {
                ref var fightComp = ref _fightPool.Value.Get(entity);
                fightComp.TimerNextWave -= Time.deltaTime;
                var state = BattleState.Instance;
                if (fightComp.TimerNextWave > 0 && state.CurrentRoom.CurrentNumberOfEnemies > 0) continue;
                while (state.IndexWave < state.CurrentRoom.RoomConfig.enemyWaves.Count)
                {
                    EnemyWave currentWave = state.CurrentRoom.RoomConfig.enemyWaves[state.IndexWave];
                    if (currentWave.GetSumCount() == 0)
                    {
                        state.IndexWave++;
                        currentWave = state.CurrentRoom.RoomConfig.enemyWaves[state.IndexWave];
                        continue;
                    }
                    ref var spawnWaveEvent = ref _spawnWavePool.Value.Add(_world.Value.NewEntity());
                    spawnWaveEvent.EnemyWave = currentWave;
                    spawnWaveEvent.SpawnPoints = state.CurrentRoom.SpawnPoints;
                    fightComp.TimerNextWave = currentWave.TimerUntilNextWave;
                    state.CurrentRoom.CurrentNumberOfEnemies += currentWave.GetSumCount();
                    state.IndexWave++;
                    return;
                }
                if (state.CurrentRoom.CurrentNumberOfEnemies == 0)
                {
                    _stopFightPool.Value.Add(entity);
                    _fightPool.Value.Del(entity);
                }
            }
        }
    }
}