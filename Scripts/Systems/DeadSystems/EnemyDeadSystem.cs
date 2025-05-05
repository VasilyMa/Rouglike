using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client {
    sealed class EnemyDeadSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<MomentDeadEvent, EnemyComponent, MeshComponent,PhysicsUnitComponent>, Exc<StaticUnitComponent>> _filter;
        readonly EcsPoolInject<FightRoomComponent> _fightPool;
        readonly EcsPoolInject<MeshComponent> _meshPool = default;
        readonly EcsPoolInject<DisposeObserverComponent> _disposeObserver = default;
        readonly EcsPoolInject<PhysicsUnitComponent> _physicsUnitComponent = default;
        private Material[] _disolveMat = new Material[1];

        public override MainEcsSystem Clone()
        {
            return new EnemyDeadSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            _disolveMat[0] = ConfigModule.GetConfig<ViewConfig>().DisolveMaterial;
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var meshComp = ref _meshPool.Value.Get(entity);
                ref var physicsUnitComponent = ref _physicsUnitComponent.Value.Get(entity);
                foreach (SkinnedMeshRenderer renderer in meshComp.SkinnedMeshRenderers)
                {
                    renderer.materials = _disolveMat;
                }
                physicsUnitComponent.PhysicsUnitMB.SetMaterialWeapon(_disolveMat[0]);
                _disposeObserver.Value.Add(entity);
                BattleState.Instance.CurrentRoom.CurrentNumberOfEnemies--;
            }

        }
    }
}