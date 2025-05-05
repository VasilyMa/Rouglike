using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Client {
    sealed class TimerShieldSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<ShieldsContainer>, Exc<DeadComponent>> _filterShied;
        readonly EcsPoolInject<ShieldsContainer> _shieldContainerPool;
        readonly EcsPoolInject<ShieldDestructionEvent> _shieldDestructionPool;

        public override MainEcsSystem Clone()
        {
            return new TimerShieldSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach ( var entity in _filterShied.Value)
            {
                ref var shieldContainer = ref _shieldContainerPool.Value.Get(entity);
                for(int i =0; i< shieldContainer.shieldComponents.Count;i++)
                {
                    shieldContainer.shieldComponents[i].Duration -= Time.deltaTime;
                    if (shieldContainer.shieldComponents[i].Duration > 0) continue;

                    if (!_shieldDestructionPool.Value.Has(entity)) _shieldDestructionPool.Value.Add(entity).shields = new();
                    ref var shieldDestruction = ref _shieldDestructionPool.Value.Get(entity);
                    shieldDestruction.shields.Add(shieldContainer.shieldComponents[i]);
                }
            }
        }
    }
}