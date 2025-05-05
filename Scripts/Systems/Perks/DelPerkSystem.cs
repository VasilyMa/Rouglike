using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client
{
    public class DelPerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DelPerkRequest>> _filter = default;
        readonly EcsPoolInject<DelPerkRequest> _requestPool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new DelPerkSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //remove this if it not used
        }

        
        public override void Run(IEcsSystems systems)
        {
            //remove this if it not used
        }
    }
}