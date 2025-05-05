using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements;
using JetBrains.Annotations;
using static UnityEngine.EventSystems.EventTrigger;
using System.Linq;

namespace Client {
    sealed class RunMissileTrailSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MissileTrailComponent>> _filter;
        readonly EcsPoolInject<MissileTrailComponent> _missileTrailPool;
        readonly EcsPoolInject<NextMissileComponent> _nextMissilePool;

        public override MainEcsSystem Clone()
        {
            return new RunMissileTrailSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                _nextMissilePool.Value.Add(entity);
                _missileTrailPool.Value.Del(entity);
            }
        }
    }
}