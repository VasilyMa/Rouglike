using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class RotateWhileMovingSystem : IEcsRunSystem {        
        readonly EcsFilterInject<Inc<EnemyComponent, TargetComponent, RotationComponent, MoveComponent>> _filter = default;
        public void Run (IEcsSystems systems) {
            // add your run code here.
        }
    }
}