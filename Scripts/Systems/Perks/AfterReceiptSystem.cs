using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AfterReceiptSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AfterReceivingPerk, PerkComponent>,Exc<UnusedPerk>> _filter;
        readonly EcsPoolInject<ReceivingRelic> _receivingPerkPool;
        readonly EcsPoolInject<UnusedPerk> _unusedPool = default;
        public override MainEcsSystem Clone()
        {
            return new AfterReceiptSystem();
        }  
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                if (!_receivingPerkPool.Value.Has(entity))
                    _receivingPerkPool.Value.Add(entity);
                else
                    _unusedPool.Value.Add(entity);
            }
        }
    }
}