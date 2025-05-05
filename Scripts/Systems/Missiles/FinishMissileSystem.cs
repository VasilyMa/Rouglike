using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client
{
    sealed class FinishMissileSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<FinishMissileEvent>,Exc<DelEntityEvent>> _filter = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;
        readonly EcsPoolInject<DelEntityEvent> _delEntityPool = default;

        public override MainEcsSystem Clone()
        {
            return new FinishMissileSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                missileComp.missile.gameObject.SetActive(false);
                missileComp.missile.ReturnToPool();
                //missileComp.missile.gameObject.SetActive(false);
                // ochen strannaya huynya
                _delEntityPool.Value.Add(entity);

                //  _world.Value.DelEntity(entity);
            }
        }
    }
}