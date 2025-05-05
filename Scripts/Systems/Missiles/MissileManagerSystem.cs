using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;

namespace Client {
    sealed class MissileManagerSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MissileComponent, MissileManagerComponent, NextMissileComponent>,Exc<FinishMissileEvent>> _filter;
        readonly EcsPoolInject<MissileManagerComponent> _missileManagerPool;
        readonly EcsPoolInject<NextMissileComponent> _nextMissilePool;
        readonly EcsPoolInject<FinishMissileEvent> _finishPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new MissileManagerSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach( var entity in _filter.Value)
            {
                ref var missileManager = ref _missileManagerPool.Value.Get(entity);
                if (missileManager.currentInedexComponent >= missileManager.Components.Count - 1)
                {
                    _finishPool.Value.Add(entity);
                    return;
                }
                missileManager.currentInedexComponent++;
                var nextIndex = missileManager.currentInedexComponent;
                missileManager.Components[nextIndex].Invoke(entity,_world.Value, missileManager.charge);
                _nextMissilePool.Value.Del(entity);
            }
        }
    }
}