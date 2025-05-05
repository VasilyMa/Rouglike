using AbilitySystem;
using DG.Tweening;
using Leopotam.EcsLite;

namespace Client {
    struct RamComponent : IAbilityComponent
    {
        public Ease Boost;
        public float Distance;
        public float SpeedMove;
        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
            EcsPool<RamComponent> _ramPool = world.GetPool<RamComponent>();
            EcsPool<TimeStopMoveComponent> _timeStopPool = world.GetPool<TimeStopMoveComponent>();
            EcsPool<MoveAbilityComponent> _movePool = world.GetPool<MoveAbilityComponent>();
            if (_ramPool.Has(entityCaster)) _ramPool.Del(entityCaster);
            if (_timeStopPool.Has(entityCaster)) _timeStopPool.Del(entityCaster);
            if (_movePool.Has(entityCaster))
            {
                ref var moveComp = ref _movePool.Get(entityCaster);
                moveComp.Tween.Pause();
                moveComp.Tween.onComplete.Invoke();
            }
        }

        public void Init()
        {
        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            EcsPool<RamComponent> _pool = world.GetPool<RamComponent>();
            if(!_pool.Has(entityCaster)) _pool.Add(entityCaster);
            ref var RamComp= ref _pool.Get(entityCaster);
            RamComp.Boost = Boost;
            RamComp.Distance=Distance;
            RamComp.SpeedMove=SpeedMove;
            
        }
    }
}