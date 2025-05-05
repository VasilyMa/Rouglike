using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client {
    sealed class StartFightSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<StartFightEvent>> _filter;
        readonly EcsPoolInject<StartFightEvent> _startFightPool;
        readonly EcsPoolInject<FightRoomComponent> _fightRoomPool;

        public override MainEcsSystem Clone()
        {
            return new StartFightSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var fightRoom—omp = ref _fightRoomPool.Value.Add(entity);
                fightRoom—omp.TimerNextWave = 0;
                _startFightPool.Value.Del(entity);
            }
        
        }
    }
}