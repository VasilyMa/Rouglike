using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
namespace Client {
    struct InputComponent {
        public PlayerInputAction InputAction;
        public InputActionPreset InputActionPreset;
        public void SetInputActionPreset(InputActionPreset inputActionPreset)
        {
            InputActionPreset = inputActionPreset;
        }
    }
    public enum InputActionPreset {FullControl, NonAttack, NonPlayerControl }
}