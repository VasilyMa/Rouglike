using Leopotam.EcsLite;
using UnityEngine.InputSystem;
namespace Client {
    struct InitAbilityEvent {
        public AbilityBase AbilityBase;
        public EcsPackedEntity PackedEntity;






        public bool initFromCollection;
        public InputActionReference NewAbilityInputReference;
        public bool IsReplace;
    }
}