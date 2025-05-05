using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AfterTimerDisposeSystem : MainEcsSystem
    {   
        readonly EcsFilterInject<Inc<AfterTimerDispose>, Exc<UnusedHelper>> _filter = default;
        readonly EcsPoolInject<AfterTimerDispose> _timerPool = default;
        readonly EcsPoolInject<UnusedHelper> _unusedPool = default;

        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new AfterTimerDisposeSystem();
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
                else _unusedPool.Value.Add(entity);
            }
        }
    }
}