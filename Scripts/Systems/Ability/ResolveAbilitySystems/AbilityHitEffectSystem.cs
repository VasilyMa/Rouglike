using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client {
    sealed class AbilityHitEffectSystem : IEcsRunSystem {
        readonly private EcsFilterInject<Inc<HitEffect>> _filter = default;
        readonly private EcsPoolInject<HitEffect> _pool = default;
        readonly private EcsPoolInject<TransformComponent> _playerTransformPool = default; //for test, need enemyEntity and his transform, however, we need merge for use pool
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var hitEffect = ref _pool.Value.Get(entity); // ����� ������ (�� ����)
                ref var transormplayerComp = ref _playerTransformPool.Value.Get(State.Instance.GetEntity("PlayerEntity"));
                var pos = transormplayerComp.Transform.position;
                pos.z += hitEffect.OffsetZ;

                    GameObject go = PoolModule.Instance.GetFromPool<SourceParticle>(hitEffect.SourceParticle,false).gameObject;
                    go.transform.position = pos;
                    go.SetActive(true);
            }
        }
    }
}