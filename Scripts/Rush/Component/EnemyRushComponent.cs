using AbilitySystem;
using Leopotam.EcsLite;
using DG.Tweening;

namespace Client {
    struct EnemyRushComponent : IAbilityComponent
    {
        public float SpeedAnimation;

        public void Init()
        {

        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            EcsPool<EnemyRushComponent> _pool = world.GetPool<EnemyRushComponent>();
            if (!_pool.Has(entityCaster)) _pool.Add(entityCaster);
            ref var _enemyRushComp = ref _pool.Get(entityCaster);
            _enemyRushComp.SpeedAnimation = SpeedAnimation;
        }

        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {
            EcsPool<EnemyRushComponent> _enemyRushPool=world.GetPool<EnemyRushComponent>();
            EcsPool<TimeStopMoveComponent> _timeStopPool=world.GetPool<TimeStopMoveComponent>();
            EcsPool<MoveAbilityComponent> _movePool=world.GetPool<MoveAbilityComponent>();
            if(_enemyRushPool.Has(entityCaster)) _enemyRushPool.Del(entityCaster);
            if (_timeStopPool.Has(entityCaster)) _timeStopPool.Del(entityCaster);
            if(_movePool.Has(entityCaster))
            {
                ref var moveComp=ref _movePool.Get(entityCaster);
                moveComp.Tween.Pause();
                moveComp.Tween.onComplete.Invoke();
            }
        }

    }
}