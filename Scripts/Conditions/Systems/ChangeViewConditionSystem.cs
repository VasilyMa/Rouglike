using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Client {
    sealed class ChangeViewConditionSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<ChangeViewConditionEvent, ViewConditionComponent,ConditionCompnent>> _filter;
        readonly EcsPoolInject<ChangeViewConditionEvent> _changeViewConditionPool;
        readonly EcsPoolInject<ViewConditionComponent> _viewConditionPool;
        readonly EcsPoolInject<ConditionCompnent> _conditionPool;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new ChangeViewConditionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var changeViewComp = ref _changeViewConditionPool.Value.Get(entity);
                ref var viewConditionComp = ref _viewConditionPool.Value.Get(entity);
                ref var conditionComp = ref _conditionPool.Value.Get(entity);
                if (!conditionComp.PackedEntityOwner.Unpack(_world.Value, out int entityOwner)) continue;
                if (!_transformPool.Value.Has(entityOwner)) continue;
                ref var transformComp = ref _transformPool.Value.Get(entityOwner);
                if (viewConditionComp.SourseParticle is not null)
                {
                    viewConditionComp.SourseParticle.Dispose();
                    viewConditionComp.SourseParticle.transform.SetParent(null);
                }

                SourceParticle sourceParticle = PoolModule.Instance.GetFromPool<SourceParticle>(changeViewComp.SourseParticle, true);
                sourceParticle.AttachVisualEffectToEntity(transformComp.Transform.position + Vector3.up * 0.5f, Quaternion.identity, conditionComp.PackedEntityOwner, transformComp.Transform, float.MaxValue);
                sourceParticle.gameObject.SetActive(true);
                viewConditionComp.SourseParticle = sourceParticle;
            }
        }
    }
}