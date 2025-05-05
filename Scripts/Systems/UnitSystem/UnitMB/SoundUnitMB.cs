using Client;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class SoundUnitMB : UnitMB 
{
    public EventReference Foots;
    public EventReference GetDamage;
    public float SoundMinDamage;
    public EventReference Death;
    [Range(0, 1)] public float HeartRateThresholdPercentage;
    public EventReference HeartRate;
    
    public void FootSound()
    {
        if (Foots.IsNull) return;
        PlayAnySound(Foots);
    }
    public void PlayDamageSound(float minDamageForSound = 0)
    {
        if (GetDamage.IsNull) return;
        PlayAnySound(GetDamage);
    }
    public void HeartRateSound()
    {
        ref var healthComp = ref _world.GetPool<HealthComponent>().Get(_entity);
        if ((healthComp.CurrentValue / healthComp.MaxValue) <= HeartRateThresholdPercentage)
        {
            PlayAnySound(HeartRate, 1 - (healthComp.CurrentValue / healthComp.MaxValue));
        }
    }
    public void DeathSound()
    {
        if (Death.IsNull) return;
        PlayAnySound(Death);
    }
    public void PlayAnySound(EventReference eventReference, float volume = 1)
    {
        SoundEntity.Instance.PlayAudioAttached(eventReference, this.transform);
    }
}
