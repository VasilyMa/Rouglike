using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;


namespace Client {
    sealed class PermanentMissileSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<MissilePermanentComponent>, Exc<NextMissileComponent, UnitCollisionEvent>> _filter = default;
        readonly EcsPoolInject<MissilePermanentComponent> _permamnentPool = default;
        readonly EcsPoolInject<NextMissileComponent> _nextPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;
        readonly EcsPoolInject<UnitCollisionEvent> _unitCollisionPool = default;
        int _entity;

        public override MainEcsSystem Clone()
        {
            return new PermanentMissileSystem();
        }

        public override void Run (IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _entity = entity;

                ref var permanentComp = ref _permamnentPool.Value.Get(_entity);

                permanentComp.Duration -= Time.deltaTime;

                if (permanentComp.Duration <= 0)
                {
                    _permamnentPool.Value.Del(entity);
                    _nextPool.Value.Add(_entity);
                    continue;
                }

                permanentComp._timeToReoslve -= Time.deltaTime; 

                if (permanentComp._timeToReoslve <= 0)
                {
                    permanentComp._timeToReoslve = permanentComp.TimeToResolve;
                    ResolveEffect();
                }
            }
        }

        void ResolveEffect()
        {
            ref var transformComp = ref _transformPool.Value.Get(_entity);
            ref var permanentComp = ref _permamnentPool.Value.Get(_entity);
            ref var missileComp = ref _missilePool.Value.Get(_entity);
           
            Collider[] hits = Physics.OverlapSphere(transformComp.Transform.position, permanentComp.Radius);

            List<EcsPackedEntity> entities = new List<EcsPackedEntity>();
            entities.Clear();
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (hit is null) continue;

                    if (hit.TryGetComponent<UnitMB>(out var unit))
                    {
                        if (hit.gameObject.layer != missileComp.missile.layerMaskTarget) continue;
                        EcsPackedEntity unitEntity = _world.Value.PackEntity(unit._entity);
                        if(!entities.Contains(unitEntity)) entities.Add(unitEntity);
                    }
                }
            }

            ref var unitCollision = ref _unitCollisionPool.Value.Add(_entity);
            unitCollision.CollisionEntity = entities;
            unitCollision.SenderPackedEntity = _world.Value.PackEntity(_entity);
        }
    }
}