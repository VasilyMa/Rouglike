using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    [System.Serializable]
    public struct TrailComponent
    {
        public float SizeBounds;
        public NonCollisionMissileMB trailMB;
        public float MaxTimeTrail;
        [HideInInspector] public List<Bounds> listBounds;
        [HideInInspector] public EcsPackedEntity EntityCreatorTrail;
        [HideInInspector] public string LayerMaskTarget;
        public void Invoke(int entityTrail,int entityCreator, EcsWorld World, string LayerTarget)
        {
            ref var trailComp = ref World.GetPool<TrailComponent>().Add(entityTrail);
            ref var colliderCreator = ref World.GetPool<ColliderComponent>().Get(entityCreator);
            trailComp.listBounds = new() { new Bounds(colliderCreator.Collider.bounds.center,new Vector3(SizeBounds, colliderCreator.Collider.bounds.center.y,SizeBounds)) };
            trailComp.EntityCreatorTrail = World.PackEntity(entityCreator);
            trailComp.SizeBounds = SizeBounds;
            trailComp.LayerMaskTarget = LayerMaskTarget;
            trailComp.trailMB = PoolModule.Instance.GetFromPool<NonCollisionMissileMB>(trailMB);
            
            ref var transformTrail = ref World.GetPool<TransformComponent>().Add(entityTrail);
            transformTrail.Transform = trailComp.trailMB.transform;
            trailComp.trailMB.gameObject.SetActive(true);
            trailComp.LayerMaskTarget = LayerTarget;
            trailComp.MaxTimeTrail = MaxTimeTrail;
        }
    }
}
