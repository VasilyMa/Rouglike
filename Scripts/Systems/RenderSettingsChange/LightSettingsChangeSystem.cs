using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class LightSettingsChangeSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<LightSettingsAreChanging>> _filter = default;
        readonly EcsPoolInject<LightSettingsAreChanging> _settingsPool = default;

        public override MainEcsSystem Clone()
        {
            return new LightSettingsChangeSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _filter.Value)
            {
                ref var settings = ref _settingsPool.Value.Get(entity);

                RenderSettings.ambientSkyColor = settings.TargetSettings.SkyColor;
                RenderSettings.ambientEquatorColor = settings.TargetSettings.EquatorColor;
                RenderSettings.ambientGroundColor = settings.TargetSettings.GroundColor;
                RenderSettings.fogColor = settings.TargetSettings.FogColor;
                RenderSettings.fogStartDistance = settings.TargetSettings.Start;
                RenderSettings.fogEndDistance = settings.TargetSettings.End;
                settings.LightReference.color = settings.TargetSettings.Color;
                settings.LightReference.cookieSize = settings.TargetSettings.CookieSize;
                settings.LightReference.intensity = settings.TargetSettings.Intensity;
                settings.LightReference.bounceIntensity = settings.TargetSettings.IndirectMultiplier;
               
                _settingsPool.Value.Del(entity);
                
            }
        }
    }
}