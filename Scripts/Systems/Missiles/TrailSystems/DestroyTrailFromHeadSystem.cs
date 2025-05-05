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
    sealed class DestroyTrailFromHeadSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DestroyTrailFromHeadComponent, StopTrailComponent,TrailComponent>> _filter;
        readonly EcsPoolInject<DestroyTrailFromHeadComponent> _destroyTrailPool;
        readonly EcsPoolInject<TrailComponent> _trailPool;

        public override MainEcsSystem Clone()
        {
            return new DestroyTrailFromHeadSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var destroyTrailComp = ref _destroyTrailPool.Value.Get(entity);
                ref var trailComp = ref _trailPool.Value.Get(entity);
                //foreach (var bound in trailComp.listBounds) // NE YBIRAY BLAT
                //{
                //    DrawWireCube(bound.center, bound.size);
                //}
                if (destroyTrailComp.DelayDestroy >= 0)
                {
                    destroyTrailComp.DelayDestroy -= Time.deltaTime;
                    continue;
                }
                destroyTrailComp.TimerDestoyBounds -= Time.deltaTime;
                if (destroyTrailComp.TimerDestoyBounds >= 0) continue;
                destroyTrailComp.TimerDestoyBounds = destroyTrailComp.TimeOfDestroyBounds;
                if (trailComp.listBounds.Count > 0)
                    trailComp.listBounds.RemoveAt(trailComp.listBounds.Count - 1);

            }
        }
        // NE YBIRAY BLAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //private void DrawWireCube(Vector3 center, Vector3 size) 
        //{
        //    Vector3 halfSize = size * 0.5f;

        //    // Front face
        //    Debug.DrawLine(center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z), center + new Vector3(halfSize.x, -halfSize.y, halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(halfSize.x, -halfSize.y, halfSize.z), center + new Vector3(halfSize.x, halfSize.y, halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(halfSize.x, halfSize.y, halfSize.z), center + new Vector3(-halfSize.x, halfSize.y, halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(-halfSize.x, halfSize.y, halfSize.z), center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z), Color.green);

        //    // Back face
        //    Debug.DrawLine(center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z), center + new Vector3(halfSize.x, halfSize.y, -halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(halfSize.x, halfSize.y, -halfSize.z), center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z), center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), Color.green);

        //    // Connect front and back faces
        //    Debug.DrawLine(center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z), center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(halfSize.x, -halfSize.y, halfSize.z), center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(halfSize.x, halfSize.y, halfSize.z), center + new Vector3(halfSize.x, halfSize.y, -halfSize.z), Color.green);
        //    Debug.DrawLine(center + new Vector3(-halfSize.x, halfSize.y, halfSize.z), center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z), Color.green);
        //}
    }
}
