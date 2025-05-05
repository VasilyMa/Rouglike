using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class TelegraphyOfMissileTargetSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TelegraphyOfMissileTargetComponent>> _filter;
        readonly EcsPoolInject<TelegraphyOfMissileTargetComponent> _telegraphingOfMissileTargetPool;
        public override MainEcsSystem Clone()
        {
            return new TelegraphyOfMissileTargetSystem();
        }

        public override void Init(IEcsSystems systems)
        {
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var telegraphingOfMissileComp = ref _telegraphingOfMissileTargetPool.Value.Get(entity);
                telegraphingOfMissileComp.TimeBeforeInvoke -= Time.deltaTime;
                if (telegraphingOfMissileComp.TimeBeforeInvoke > 0) 
                    continue;
                if(telegraphingOfMissileComp.CreatedObject is null)
                {
                    telegraphingOfMissileComp.CreatedObject = PoolModule.Instance.GetFromPool<NonCollisionMissileMB>(telegraphingOfMissileComp.TelegraphingMissleMB, true);
                    telegraphingOfMissileComp.CreatedObject.transform.position = telegraphingOfMissileComp.Position;
                }

                telegraphingOfMissileComp.LifeTime -= Time.deltaTime;
                if (telegraphingOfMissileComp.LifeTime > 0) 
                    continue;
                telegraphingOfMissileComp.CreatedObject.gameObject.SetActive(false);
                telegraphingOfMissileComp.CreatedObject.ReturnToPool();
                _telegraphingOfMissileTargetPool.Value.Del(entity);
            }
        }
    }
}