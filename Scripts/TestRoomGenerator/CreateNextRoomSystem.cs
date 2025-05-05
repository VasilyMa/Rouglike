using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateNextRoomSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<CreateNextRoomEvent>> _filter = default;
        readonly EcsPoolInject<CreateNextRoomEvent> _nextRoomPool = default;
        readonly EcsSharedInject<GameState> _state = default;
        public void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var nextRoomComp = ref _nextRoomPool.Value.Get(entity);
                
            }
        }
    }
}