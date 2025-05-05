using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualDamageConfigs", menuName = "Configs/VisualDamage")]
public class VisualDamageConfig : ScriptableObject
{
    [SerializeField] public Color DamageColor = Color.white;
    public float TotalDuration = 0.5f;
    public float TimeMaxIntensity = 0.25f;
    public float MaxIntensity = 3f;
    public float LifeTime = 2f;
    [SerializeField] public DamageNumber PhysicalDamageEnemy;
    [SerializeField] public DamageNumber PhysicalDamagePlayer;
    [SerializeField] public DamageNumber PositiveHealthChange;
    [SerializeField] public DamageNumber ConditionDamageEnemy;
    [SerializeField] public DamageNumber ConditionDamagePlayer;
    public DamageNumber CurrencyPickup;
    private void OnValidate() {
        PhysicalDamageEnemy.lifetime = LifeTime;
        PhysicalDamagePlayer.lifetime = LifeTime;
        PositiveHealthChange.lifetime = LifeTime;
        ConditionDamageEnemy.lifetime = LifeTime;
        ConditionDamagePlayer.lifetime = LifeTime;
    }
}
