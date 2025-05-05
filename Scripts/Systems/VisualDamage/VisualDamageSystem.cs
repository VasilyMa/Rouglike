using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class VisualDamageSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc< ViewDamageComponent, MeshComponent>> _filter;
        readonly EcsPoolInject<ViewDamageComponent> _timerPool;
        readonly EcsPoolInject<MeshComponent> _meshPool;
        private VisualDamageConfig _visualDamageConfig;

        public override MainEcsSystem Clone()
        {
            return new VisualDamageSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            _visualDamageConfig = ConfigModule.GetConfig<ViewConfig>().VisualDamageConfig;
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var timerComp = ref _timerPool.Value.Get(entity);
                ref var meshComp = ref _meshPool.Value.Get(entity);
                if (timerComp.Duration < _visualDamageConfig.TotalDuration)
                {
                    timerComp.Duration += Time.deltaTime;
                    if (timerComp.Duration < _visualDamageConfig.TimeMaxIntensity)
                    {
                        timerComp.CurrentIntensity = Mathf.Lerp(0f, _visualDamageConfig.MaxIntensity, timerComp.Duration / _visualDamageConfig.TotalDuration);
                    }
                    else
                    {
                        float deltaTime = timerComp.Duration - _visualDamageConfig.TimeMaxIntensity;
                        float DurationReduce = _visualDamageConfig.TotalDuration - _visualDamageConfig.TimeMaxIntensity;
                        timerComp.CurrentIntensity = Mathf.Lerp(_visualDamageConfig.MaxIntensity, 0f , deltaTime / DurationReduce);
                    }
                    foreach (SkinnedMeshRenderer renderer in meshComp.SkinnedMeshRenderers)
                    {
                        renderer.material.SetColor("_Emission", _visualDamageConfig.DamageColor * timerComp.CurrentIntensity);
                    }
                    
                }
                else
                {
                    var oldCollor = new Color(0, 0, 0);
                    foreach (SkinnedMeshRenderer renderer in meshComp.SkinnedMeshRenderers)
                    {
                        renderer.material.SetColor("_Emission", oldCollor);
                    }
                    _timerPool.Value.Del(entity);
                }
            }
        }
    }
}