using Leopotam.EcsLite;

namespace Client {
    struct DestroyConditionEvent : IConditionResolve
    {
        public void InvokeResolve(int entityCondition, int entityOwner, EcsWorld world)
        {
            var _destroyPool =  world.GetPool<DestroyConditionEvent>();
            if (!_destroyPool.Has(entityOwner)) _destroyPool.Add(entityCondition);
        }

        public void Recalculate(float charge)
        {

        }
    }
}