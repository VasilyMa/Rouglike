using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class TiredSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<TiredComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<TiredComponent> _tiredPool = default;
        readonly EcsPoolInject<IrrevocabilityComponent> _irrevocPool = default;
        readonly EcsPoolInject<RequestAddHardControlEvent> _requestHardControlPool = default;
        readonly EcsPoolInject<ToughnessAnimationState> _tiredAnimationPool = default;
        readonly EcsPoolInject<RecoveryAnimationState> _recoveryPool = default;

        public override MainEcsSystem Clone()
        {
            return new TiredSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var tiredComp = ref _tiredPool.Value.Get(entity);
                if (tiredComp.Duration > 0)
                {
                    tiredComp.Duration -= Time.deltaTime;
                    //probably will break taking damage while tired, though fixing rotation lock after tired condition
                    if (!tiredComp.IsTired)
                    {
                        _tiredAnimationPool.Value.Add(entity);
                        ref var requestComp = ref _requestHardControlPool.Value.Add(_world.Value.NewEntity());
                        requestComp.TargetEntity = _world.Value.PackEntity(entity);
                        requestComp.ControlTime = tiredComp.Duration;
                        tiredComp.IsTired = true;   
                    }
                    if (_irrevocPool.Value.Has(entity)) _irrevocPool.Value.Del(entity); //peredelat govno fuuuu   buuueee
                }
                else
                {
                    _recoveryPool.Value.Add(entity).IsRootMotion = false;
                    _tiredPool.Value.Del(entity);
                }
            }
        }
    }
}