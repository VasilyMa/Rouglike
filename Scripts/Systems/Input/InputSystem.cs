using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace Client {
    sealed class InputSystem : MainEcsSystem {
        //inputactionreference
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<PlayerComponent>, Exc<DeadComponent>> _playerFilter = default;
        readonly EcsPoolInject<InputComponent> _inputPool = default;
        readonly EcsPoolInject<InputHolder> _inputHolderPool = default;
        readonly EcsFilterInject<Inc<InputComponent>> _filter = default;
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;
        readonly EcsPoolInject<WASDInputEvent> _wasdPool = default;
        readonly EcsPoolInject<MousePositionInputEvent> _mousePool = default;
        readonly EcsPoolInject<VIEWDIRECTIONInputEvent> _viewDirectionPool = default;
        readonly EcsPoolInject<AbilityInputEvent> _abilityInputPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        InputActionMap currentActionMap;

        public override MainEcsSystem Clone()
        {
            return new InputSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var inputComp = ref _inputPool.Value.Get(entity);
                foreach (int playerEntity in _playerFilter.Value)
                {
                    ref var transfromComp = ref _transfromPool.Value.Get(playerEntity);
                    ref var inputHolderComp = ref _inputHolderPool.Value.Get(playerEntity);
                    switch (inputComp.InputActionPreset)
                    {
                        case InputActionPreset.FullControl:
                            currentActionMap = inputComp.InputAction.ActionMap;
                            break;
                        case InputActionPreset.NonAttack:
                            currentActionMap = inputComp.InputAction.NonAttackActionMap;
                            break;
                        case InputActionPreset.NonPlayerControl:
                            currentActionMap = inputComp.InputAction.NonPlayerControlMap;
                            break;
                    }
                    var mousePosition = currentActionMap.FindAction("Mouse").ReadValue<Vector2>();
                    Vector3 point = Vector3.zero;
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(mousePosition);

                    if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Ground")))
                    {
                        point = hit.point;
                        point = new Vector3(point.x, 0, point.z);
                    }
                    if (inputComp.InputActionPreset == InputActionPreset.NonPlayerControl)
                    {
                        inputHolderComp.MoveDirection = Vector3.zero;
                        continue;
                    }
                    Vector3 playerPosition = new Vector3(transfromComp.Transform.position.x, 0, transfromComp.Transform.position.z);
                    Vector3 directionOfView = point - playerPosition;

                    ref var viewDirectionComp = ref _viewDirectionPool.Value.Add(entity);
                    viewDirectionComp.ViewDirection = directionOfView.normalized;

                    ref var mousePositionComp = ref _mousePool.Value.Add(entity);
                    mousePositionComp.MousePosition = point;
                    inputHolderComp.MoveDirection = currentActionMap.FindAction("Move").ReadValue<Vector3>().normalized;
                    Vector3 wasd = currentActionMap.FindAction("Move").ReadValue<Vector3>().normalized;
                    if (wasd != Vector3.zero)
                    {
                        ref var wasdDirectionComp = ref _wasdPool.Value.Add(entity);
                        wasdDirectionComp.WasdDirection = wasd;

                        ref var animatorComp = ref _animatorPool.Value.Get(playerEntity);
                        animatorComp.Animator.SetBool("IsInput", true);
                    }

                    foreach (var inputAction in currentActionMap)
                    {
                        if (inputAction.WasPressedThisFrame())
                        {
                            ref var inputAbilityComp = ref _abilityInputPool.Value.Add(_world.Value.NewEntity());
                            inputAbilityComp.InputAction = inputAction;
                            inputAbilityComp.Pressing = true;
                        }
                        if (inputAction.WasReleasedThisFrame())
                        {
                            ref var inputAbilityComp = ref _abilityInputPool.Value.Add(_world.Value.NewEntity());
                            inputAbilityComp.InputAction = inputAction;
                            inputAbilityComp.Pressing = false;
                        }
                    }
                }
            }
        }
    }
}