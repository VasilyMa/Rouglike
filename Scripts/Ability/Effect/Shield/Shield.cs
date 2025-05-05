using AbilitySystem;
using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Client
{
    public class Shield 
    {
        [HideInInspector] public SourceParticle InstantiatedSourceParticle;
        public SourceParticle SourceParticle;
        [Range(0, 3000)] public float DamageProtection;
        [Range(0, 300)] public float Duration;
        public Shield (SourceParticle SourceParticle, float DamageProtection, float Duration)
        {
            this.SourceParticle = SourceParticle;
            this.DamageProtection = DamageProtection;
            this.Duration = Duration;
        }
        public void Invoke(Transform owner)
        {
                InstantiatedSourceParticle = PoolModule.Instance.GetFromPool<SourceParticle>(SourceParticle,true);

                InstantiatedSourceParticle.gameObject.SetActive(true);

                var transfromGO = InstantiatedSourceParticle.gameObject.transform;
                transfromGO.position = owner.position;
                transfromGO.SetParent(owner);

                InstantiatedSourceParticle.GetEffect().SetFloat("Lifetime", Duration);
                InstantiatedSourceParticle.Timer = Duration;
                InstantiatedSourceParticle.transform.localPosition = new Vector3(0, 1, 0);
        }
    }
}