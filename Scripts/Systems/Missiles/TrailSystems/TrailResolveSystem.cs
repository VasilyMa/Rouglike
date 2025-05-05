using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    sealed class TrailResolveSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TrailComponent, ResolveTrailComponent,ResolveBlockComponent>> _filter;
        readonly EcsPoolInject<UnitCollisionEvent> _unitCollisionEvent;
        readonly EcsPoolInject<ResolveTrailComponent> _resolveTrailPool;
        readonly EcsFilterInject<Inc<PlayerComponent>, Exc<DeadComponent>> _filterPlayer;
        readonly EcsFilterInject<Inc<EnemyComponent>, Exc<DeadComponent>> _filterEnemy;
        readonly EcsPoolInject<ColliderComponent> _colliderPool;
        readonly EcsPoolInject<TrailComponent> _trailPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new TrailResolveSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var resolveTrailComponent = ref _resolveTrailPool.Value.Get(entity);
                resolveTrailComponent.TimerToResolve -= Time.deltaTime;
                if (resolveTrailComponent.TimerToResolve >= 0) continue;
                resolveTrailComponent.TimerToResolve = resolveTrailComponent.TimeToResolve;
                ref var trailComp = ref _trailPool.Value.Get(entity);
                if (trailComp.LayerMaskTarget == "Player")
                    CheckCollision(_filterPlayer.Value, entity, trailComp.listBounds);
                else
                    CheckCollision(_filterEnemy.Value, entity, trailComp.listBounds);

            }
        }
        public void CheckCollision(EcsFilter filter, int entity,List<Bounds> bounds)
        {
            foreach (var entityTarget in _filterPlayer.Value)
            {
                foreach (var bound in bounds)
                {
                    var boundsPlayer = _colliderPool.Value.Get(entityTarget).Collider.bounds;
                    if (bound.Intersects(boundsPlayer))
                    {
                        TakeResolve(entityTarget, entity);
                        break;
                    }
                }

            }
        }
        public void TakeResolve(int entityTarget, int entitySender)
        {
            if(!_unitCollisionEvent.Value.Has(entitySender))
            {
                ref var unitCollisionComp = ref _unitCollisionEvent.Value.Add(entitySender);
                unitCollisionComp.CollisionEntity = new();
                unitCollisionComp.SenderPackedEntity = _world.Value.PackEntity(entitySender);
            }
            ref var unitCollisionEvent = ref _unitCollisionEvent.Value.Get(entitySender);
            unitCollisionEvent.CollisionEntity.Add(_world.Value.PackEntity(entityTarget));
        }
    }
}
