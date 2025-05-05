using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class ExampleEcsSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;

        readonly EcsFilterInject<Inc<PlayerComponent>> _filter = default;

        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new ExampleEcsSystem();
        }

        //remove this if it not used
        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);

            int newEntity = _world.Value.NewEntity();

            _playerPool.Value.Add(newEntity);

            
        }

        //remove this if it not used
        public override void Run(IEcsSystems systems)
        {
            base.Run(systems);

            foreach (var entity in _filter.Value)
            {
                ref var playerComp = ref _playerPool.Value.Get(entity);

                
            }
        }
    }
}