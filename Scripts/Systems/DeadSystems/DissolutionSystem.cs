using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class DissolutionSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DeadComponent, MeshComponent,PhysicsUnitComponent>,Exc<PlayerComponent>> _filter;
        readonly EcsPoolInject<MeshComponent> _meshPool;
        readonly EcsPoolInject<DeadComponent> _deadPool;
        readonly EcsPoolInject<PhysicsUnitComponent> _physicsUnitPool;


        public override MainEcsSystem Clone()
        {
            return new DissolutionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var meshComp = ref _meshPool.Value.Get(entity);
                ref var deadComp = ref _deadPool.Value.Get(entity);
                ref var physicsUnitComp = ref _physicsUnitPool.Value.Get(entity);
                float ratioDissolve = Mathf.Clamp01(deadComp.TimerToDestroy / deadComp.TimeOfDeath);
                foreach (SkinnedMeshRenderer renderer in meshComp.SkinnedMeshRenderers)
                {
                    renderer.material.SetFloat("_DissolveAmount", ratioDissolve);
                }
                physicsUnitComp.PhysicsUnitMB.DissolutionWeapon(ratioDissolve); 
            }
        }
    }
}