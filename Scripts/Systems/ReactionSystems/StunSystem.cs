using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class StunSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        private readonly EcsFilterInject<Inc<StunEffect>> _filter = default;
        private readonly EcsPoolInject<StunEffect> _pool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<DeadComponent> _deadPool = default;
        private readonly EcsPoolInject<IrrevocabilityComponent> _irrevocPool = default;
        readonly EcsPoolInject<RequestAddHardControlEvent> _requestHardControlPool = default;

        readonly EcsPoolInject<PlayerComponent> _playerPool = default;

        public override MainEcsSystem Clone()
        {
            return new StunSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var stunComp = ref _pool.Value.Get(entity);
                ref var trasnformComp = ref _transformPool.Value.Get(entity);
                if (stunComp.Duration > 0)
                {
                    stunComp.Duration -= Time.deltaTime;
                    if (_irrevocPool.Value.Has(entity)) _irrevocPool.Value.Del(entity);

                    
                    if (!stunComp.IsStuned)
                    {
                        //Do Stun
                        if (!stunComp.InstantiatedObject)
                        {
                            
                            SourceParticle sourceParticle = PoolModule.Instance.GetFromPool<SourceParticle>(stunComp.SourceParticle,true);

                            stunComp.InstantiatedObject = sourceParticle.gameObject;
                            stunComp.InstantiatedObject.SetActive(true);    
                            // stunComp.InstantiatedObject.transform.position = trasnformComp.Transform.position;
                            // stunComp.InstantiatedObject.transform.rotation = Quaternion.identity;
                            // stunComp.InstantiatedObject.transform.parent = trasnformComp.Transform;
                            // stunComp.InstantiatedObject.transform.position += Vector3.up;
                            sourceParticle.AttachVisualEffectToEntity(trasnformComp.Transform.position + Vector3.up, Quaternion.identity, 
                            _world.Value.PackEntity(entity), trasnformComp.Transform,stunComp.Duration);
                            

                        }
                        stunComp.IsStuned = true;

                        ref var requestComp = ref _requestHardControlPool.Value.Add(_world.Value.NewEntity());
                        requestComp.TargetEntity = _world.Value.PackEntity(entity);
                        requestComp.ControlTime = stunComp.Duration;

                    }
                }                                                          
                else                                                       
                {

                    // if (_2pool.Value.Has(entity)) _2pool.Value.Del(entity);
                    // if (_3pool.Value.Has(entity)) _3pool.Value.Del(entity);
                    // 
                    _pool.Value.Del(entity);
                }
            }
        }
    }
}