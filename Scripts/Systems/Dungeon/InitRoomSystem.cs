using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Client {
    sealed class InitRoomSystem : IEcsInitSystem {
        readonly EcsWorldInject _world;
        readonly EcsPoolInject<RoomComponent> _roomPool;
        public void Init (IEcsSystems systems) {

            RoomMB[] roomMBs = GameObject.FindObjectsOfType<RoomMB>();
            foreach (RoomMB roomMB in roomMBs)
            {
                _roomPool.Value.Add(_world.Value.NewEntity()).RoomMB = roomMB;
            }
        }
    }
}