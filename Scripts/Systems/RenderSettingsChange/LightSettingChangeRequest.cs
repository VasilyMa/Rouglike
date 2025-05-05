using UnityEngine;
namespace Client {
    struct LightSettingChangeRequest {
        public RenderSettingsSO TargetSettings;
        public GameObject PostProcessingGO;
    }
}