using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class CheckBusyPerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<BusyPerk>, Exc<UnusedPerk>> _filter = default;
        readonly EcsPoolInject<UnusedPerk> _unusedPerkPool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new CheckBusyPerkSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //remove this if it not used
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                _unusedPerkPool.Value.Add(entity);
            }
        }
    }
}