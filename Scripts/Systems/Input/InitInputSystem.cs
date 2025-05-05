using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Statement;

namespace Client {
    sealed class InitInputSystem : MainEcsSystem 
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<InputComponent> _inputPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitInputSystem();
        }

        public override void Init (IEcsSystems systems) {

            var state = State.Instance;

            int entity = _world.Value.NewEntity();

            state.RegisterNewEntity("InputEntity", entity);

            ref var inputComp = ref _inputPool.Value.Add(entity);
            inputComp.InputAction = new PlayerInputAction();
            inputComp.InputAction.Enable();
            state.PlayerInputAction = inputComp.InputAction;

            // inputComp.InputReferences = new List<InputActionReference>();
            foreach (var action in inputComp.InputAction.asset)
            {
                // var reference = new InputActionReference();
                // reference.Set(action);
                //inputComp.InputReferences.Add(action);
            }
        }
    }
}