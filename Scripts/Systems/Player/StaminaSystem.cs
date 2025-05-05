using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UI;

using UnityEngine;

namespace Client {
    sealed class StaminaSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<StaminaComponent>> _filter = default;
        readonly EcsPoolInject<StaminaComponent> _staminaPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        GameConfig gameConfig;

        public override MainEcsSystem Clone()
        {
            return new StaminaSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            gameConfig = ConfigModule.GetConfig<GameConfig>();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var staminaComp = ref _staminaPool.Value.Get(entity);

                float value = Time.deltaTime * gameConfig.StaminaRecoveryRate;

                float valueUpdate = staminaComp.GetValue();

                staminaComp.Add(value);
            }
        }
    }
}