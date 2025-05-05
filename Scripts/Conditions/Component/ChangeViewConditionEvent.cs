using Leopotam.EcsLite;

namespace Client {
    struct ChangeViewConditionEvent : IConditionResolve
    {
        public SourceParticle SourseParticle;
        public void InvokeResolve(int entityCondition, int entityOwner, EcsWorld world)
        {
            var viewPool = world.GetPool<ChangeViewConditionEvent>();
            if (!viewPool.Has(entityCondition)) viewPool.Add(entityCondition);
            ref var viewComp = ref viewPool.Get(entityCondition);
            viewComp.SourseParticle = SourseParticle;
        }
        public void Recalculate(float charge)
        {

        }
    }
    struct ViewConditionComponent 
    {
        public SourceParticle SourseParticle;
    }
}