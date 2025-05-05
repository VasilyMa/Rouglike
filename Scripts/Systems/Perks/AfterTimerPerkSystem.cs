using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AfterTimerPerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AfterTimePerk>, Exc<UnusedPerk>> _filter = default;
        readonly EcsPoolInject<AfterTimePerk> _timerPool = default;
        readonly EcsPoolInject<UnusedPerk> _unusedPerkPool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new AfterTimerPerkSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //remove this if it not used
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var timerComp = ref _timerPool.Value.Get(entity);

                timerComp.CurrentTime += Time.deltaTime;
                
                if(timerComp.CurrentTime >= timerComp.TargetTime) timerComp.CurrentTime = 0;
                else _unusedPerkPool.Value.Add(entity);
            }
        }
    }
}