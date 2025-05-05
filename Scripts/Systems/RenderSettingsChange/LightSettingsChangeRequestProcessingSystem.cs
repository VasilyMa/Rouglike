using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class LightSettingsChangeRequestProcessingSystem : MainEcsSystem 
    {
        private readonly EcsFilterInject<Inc<LightSettingChangeRequest>> _requestFilter = default;
        private readonly EcsFilterInject<Inc<InterfaceComponent>> _persistentEntityFilter = default;
        private readonly EcsPoolInject<LightSettingChangeRequest> _requestPool = default;
        private readonly EcsPoolInject<LightSettingsAreChanging> _settingsPool = default;

        public override MainEcsSystem Clone()
        {
            return new LightSettingsChangeRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int requestEntity in _requestFilter.Value)
            {
                foreach (int persistentEntity in _persistentEntityFilter.Value)
                {
                    ref var request = ref _requestPool.Value.Get(requestEntity);
                    if (!_settingsPool.Value.Has(persistentEntity)) _settingsPool.Value.Add(persistentEntity);
                    ref var settings = ref _settingsPool.Value.Get(persistentEntity);
                    GameObject.Instantiate(request.PostProcessingGO);
                    settings.LightReference = Object.FindObjectOfType<MainSceneLight>().GetComponent<Light>();
                    settings.TargetSettings = request.TargetSettings;
                }
            }
        }
    }
}