
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
    sealed class UpdateTrailSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TrailComponent>,Exc<StopTrailComponent>> _filter;
        readonly EcsPoolInject<TrailComponent> _trailPool;
        readonly EcsPoolInject<ColliderComponent> _colliderPool;
        readonly EcsPoolInject<StopTrailComponent> _stopTrailPool;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new UpdateTrailSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var trailComponent = ref _trailPool.Value.Get(entity);
                trailComponent.MaxTimeTrail -= Time.deltaTime;
                if (trailComponent.EntityCreatorTrail.Unpack(_world.Value, out int entityCreator) && trailComponent.MaxTimeTrail >= 0)
                {
                    ref var colliderComp = ref _colliderPool.Value.Get(entityCreator);
                    if(trailComponent.listBounds.Count == 0)
                        trailComponent.listBounds.Add(new Bounds(colliderComp.Collider.bounds.center, new Vector3(trailComponent.SizeBounds, colliderComp.Collider.bounds.center.y, trailComponent.SizeBounds)));
                    var centerLastBounds = trailComponent.listBounds.Last().center;
                    if (Vector3.Distance(colliderComp.Collider.bounds.center, centerLastBounds) > trailComponent.SizeBounds)
                    {
                        while (!trailComponent.listBounds.Last().Intersects(colliderComp.Collider.bounds))
                        {
                            var centerPoint = centerLastBounds + (colliderComp.Collider.bounds.center - centerLastBounds).normalized * trailComponent.SizeBounds * 0.75f;
                            trailComponent.listBounds.Add(new Bounds(centerPoint, new Vector3(trailComponent.SizeBounds, centerPoint.y, trailComponent.SizeBounds)));
                            centerLastBounds = trailComponent.listBounds.Last().center;
                        }
                    }
                    ref var transformTrail = ref _transformPool.Value.Get(entity);
                    ref var transformCreator = ref _transformPool.Value.Get(entityCreator);
                    transformTrail.Transform.forward = transformCreator.Transform.forward;
                    var newPosotionTrail = transformCreator.Transform.position;
                    newPosotionTrail.y = 0;//????????????????????????????????? XZ nado li
                    transformTrail.Transform.position = newPosotionTrail;
                }
                else
                    _stopTrailPool.Value.Add(entity);

            }

        }
    }
}
