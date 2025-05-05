using Cinemachine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Statement;
using UnityEngine;

namespace Client {
    sealed class PlayerDeadSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<PlayerComponent, MomentDeadEvent>> _filter;

        public override MainEcsSystem Clone()
        {
            return new PlayerDeadSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                State.Instance.RemoveEntity("PlayerEntity");
                PlayerEntity.Instance.Reset();
                Statement.State.Instance.SendRequest(new GameRequest(Status.Lose));
                Camera.main.GetComponent<CinemachineVirtualCamera>().Follow = null;
                Camera.main.GetComponent<CinemachineVirtualCamera>().LookAt = null;
            }
        }
    }
}