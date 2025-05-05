using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;
using AbilitySystem;
using UnityEngine.VFX;

namespace Client
{
    sealed class AbilityHitSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<HitEffect,TransformComponent>> _filter = default;
        readonly EcsPoolInject<HitEffect> _pool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public override MainEcsSystem Clone()
        {
            return new AbilityHitSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var hitComp = ref _pool.Value.Get(entity);
                ref var transformOwnerComp = ref _transformPool.Value.Get(entity);
                ref var transformSenderComp = ref _transformPool.Value.Get(hitComp.EntitySender);
                if (transformOwnerComp.Transform is not null)
                {
                    Vector3 directionToSender = (transformSenderComp.Transform.position - transformOwnerComp.Transform.position).normalized;
                    Quaternion rotation = Quaternion.LookRotation(directionToSender, Vector3.up);
                    Vector3 eulerRotation = new Vector3(0f, rotation.eulerAngles.y, 0f);
                    foreach (var particle in hitComp.ParticlesToPlay)
                    {

                        SourceParticle sourceParticle = PoolModule.Instance.GetFromPool<SourceParticle>(particle,true);
                        sourceParticle.gameObject.SetActive(true);

                        Vector3 position = directionToSender * hitComp.OffsetZ + transformOwnerComp.Transform.position + Vector3.up;
                        Quaternion targetRotation = Quaternion.Euler(eulerRotation);


                        sourceParticle.AttachVisualEffectToEntity(position, targetRotation, _world.Value.PackEntity(entity));
                        //go.GetComponent<VisualEffect>().Play();
                    }
                }
            }
        }
    }
}