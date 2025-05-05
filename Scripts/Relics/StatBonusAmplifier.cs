using Client;

using UnityEngine;
using Statement;

public class StatBonusAmplifier : SourceRelic
{
    public StatAmplifier StatAmplifier;
    [Range(0, 1f)] public float StatBonusValue;

    public override void InvokeRelic()
    {
        var state = State.Instance.EcsRunHandler;
        var world = state.World;

        switch (StatAmplifier)
        {
            case StatAmplifier.health:
                if (world.GetPool<HealthComponent>().Has(State.Instance.GetEntity("PlayerEntity")))
                {
                    ref var healthComp = ref world.GetPool<HealthComponent>().Get(State.Instance.GetEntity("PlayerEntity"));

                    float maxValue = healthComp.MaxValue;

                    float currentValue = healthComp.CurrentValue;

                    float maxValueBonus = maxValue * StatBonusValue; // should be calculated from BASE value

                    healthComp.MaxValue += maxValueBonus;
                    healthComp.CurrentValue += maxValueBonus; 
                }
                break;
            case StatAmplifier.attack:
                //TODO Add attack bonus
                break;
        }

        
    }
}

public enum StatAmplifier { health, attack } 
