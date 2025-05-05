using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client 
{
    sealed class InitInterface : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<InterfaceComponent> _intefacePool = default;

        public override MainEcsSystem Clone()
        {
            return new InitInterface();
        }

        public override void Init (IEcsSystems systems) 
        {
            var state = BattleState.Instance;

            var entity = _world.Value.NewEntity();

            state.RegisterNewEntity("InterfaceEntity", entity);

            ref var interfaceComp = ref _intefacePool.Value.Add(entity);

            var uiManager = GameObject.FindObjectOfType<UIManager>();

            if(uiManager != null) interfaceComp.Init(uiManager);
        }
    }
}