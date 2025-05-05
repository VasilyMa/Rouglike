
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    sealed class FinishTrailSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TrailComponent, StopTrailComponent>> _filter;
        readonly EcsPoolInject<TrailComponent> _trailPool;
        readonly EcsPoolInject<DelEntityEvent> _delEntityEvent;

        public override MainEcsSystem Clone()
        {
            return new FinishTrailSystem();
        }

        public override void Run(IEcsSystems systems)
        {
           foreach ( var entity in _filter.Value)
            {
                ref var trailComp = ref _trailPool.Value.Get(entity);
                if (trailComp.listBounds.Count == 0)
                    _delEntityEvent.Value.Add(entity);
            }
        }
    }
}
