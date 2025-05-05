using UnityEngine;

namespace Client {
    struct LightSettingsAreChanging {
        public RenderSettingsSO TargetSettings;
        public Light LightReference;
    }
}