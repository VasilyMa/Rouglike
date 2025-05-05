using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class ChangeViewMissileSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<MissileComponent, ChangeViewMissileComponent>> _filter;
        readonly EcsPoolInject<ChangeViewMissileComponent> _changeViewPool;
        readonly EcsPoolInject<MissileComponent> _missilePool;
        readonly EcsPoolInject<NextMissileComponent> _NextPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new ChangeViewMissileSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach ( var entity in _filter.Value)
            {
                ref var changeViewComp = ref _changeViewPool.Value.Get(entity);
                changeViewComp.Delay -= Time.deltaTime;
                if (changeViewComp.Delay >= 0) continue;
                ref var missileComponent = ref _missilePool.Value.Get(entity);
                missileComponent.ChangeViewMissile(changeViewComp.newMissile,entity,_world.Value);
                missileComponent.missile.Invoke(_world.Value, entity, LayerMask.NameToLayer(missileComponent.LayerNameTarget));
                _changeViewPool.Value.Del(entity);
                _NextPool.Value.Add(entity);
            }
        }
    }
}