using UnityEngine;
using AbilitySystem;
using Leopotam.EcsLite;

namespace Client {
    struct WaitClick
    {
        public float CurrentTime;
        public float TargetTime;
        public void InitComponent(float targetTime = 0.5f)
        {
            TargetTime = targetTime;
            CurrentTime = 0f;
        }
    }
    struct DelWaitClick
    {

    }

}