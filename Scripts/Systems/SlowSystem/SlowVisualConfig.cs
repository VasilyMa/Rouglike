using Client;
using Leopotam.EcsLite;
using System;
using System.Collections;

using Statement;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "SlowVisualConfig", menuName = "Configs/SlowVisualConfig")]
public class SlowVisualConfig : ScriptableObject
{
    public SlowSetting LastEnemyWave;
    public SlowSetting LastEnemyRoom;
    public SlowSetting ResetCombo;
    public SlowSetting DeadEnemy;
    public float ShakeStrongDanage;


    public void AddSlow(SlowSetting slowSetting)
    {
        if (slowSetting.Active)
        {
            float value = UnityEngine.Random.Range(0f, 100f);
            if (value > slowSetting.Probability) return;
            EcsPool<SlowComponent> _slowPool = BattleState.Instance.EcsRunHandler.World.GetPool<SlowComponent>();
            var entity = BattleState.Instance.GetEntity("PlayerEntity");
            if (!_slowPool.Has(entity)) _slowPool.Add(entity);
            ref var slowComp = ref _slowPool.Get(entity);
            slowComp.Active = true;
            slowComp.Value = slowSetting.Value;
            slowComp.Duration = slowSetting.Duration * slowSetting.Value;
        }
    }
    public enum SlowTrigger
    {
        LastEnemyWave,
        LastEnemyRoom,
        ResetCombo,
        DeadEnemy,
    }
    [Serializable]
    public struct SlowSetting
    {
        public bool Active;
        public float Duration;
        public float Value;
        [Range(0f, 100f)] public float Probability;
    }
}
