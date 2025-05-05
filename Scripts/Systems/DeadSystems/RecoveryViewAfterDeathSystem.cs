using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RecoveryViewAfterDeathSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<DelEntityEvent, PhysicsUnitComponent, MeshComponent>> _filter;
        readonly EcsPoolInject<MeshComponent> _meshPool = default;
        readonly EcsPoolInject<PhysicsUnitComponent> _physicUnitPool = default;

        public override MainEcsSystem Clone()
        {
            return new RecoveryViewAfterDeathSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var meshComp = ref _meshPool.Value.Get(entity);
                ref var phComp = ref _physicUnitPool.Value.Get(entity);
                phComp.PhysicsUnitMB.UnitReset();
                var mat = new Material[1];
                mat[0] = phComp.PhysicsUnitMB.Material;
                foreach (SkinnedMeshRenderer renderer in meshComp.SkinnedMeshRenderers)
                {
                    renderer.materials = mat;
                }
            }
        }
    }
}