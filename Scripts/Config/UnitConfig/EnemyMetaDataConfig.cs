using Client;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitMetaData", menuName = "Configs/UnitMeta")]
public class EnemyMetaDataConfig : ScriptableObject
{
    public AIProfile AIProfile;
    public GameObject EnemyPrefab;
    public SourceParticle ParticleSpawn;
    public WeaponConfig WeaponConfigBase;
    public float SlowSpeed;
    public float SpawnDelay;
    public DropConfig DropConfig;

    public LoadedEnemyExecutor DownloadedData;

    [Space(10)]
    public bool isRandomViewEnemy;
    public List<ViewMetaEnemy> viewMetaEnemy;
    public UnitMetaDetail GetUnit(int Meta = 0)
    {
        var unitConfig = new UnitMetaDetail();
        unitConfig.Health = DownloadedData.HealthBase + Meta * DownloadedData.HealthUp;
        unitConfig.Speed = DownloadedData.SpeedBase + Meta * DownloadedData.SpeedUp;
        unitConfig.MaxValueToughness = DownloadedData.MaxValueToughnessBase + Meta * DownloadedData.MaxValueToughnessUp;
        unitConfig.DelayRecoveryToughness = DownloadedData.DelayRecoveryToughnessBase + Meta * DownloadedData.MaxValueToughnessUp;
        unitConfig.SpeedRecovery = DownloadedData.SpeedRecoveryBase + Meta * DownloadedData.SpeedRecoveryUp;
        unitConfig.SlowSpeed = SlowSpeed;
        unitConfig.SpawnDelay = SpawnDelay;
        unitConfig.Percent = DownloadedData.Percent;
        unitConfig.AIProfile = AIProfile;
        unitConfig.ParticleSpawn = ParticleSpawn;
        unitConfig.Unit = EnemyPrefab;
        unitConfig.WeaponConfig = WeaponConfigBase;
        ViewMetaEnemy ViewMetaEnemy;
        if (isRandomViewEnemy)
            ViewMetaEnemy = viewMetaEnemy[UnityEngine.Random.Range(0, viewMetaEnemy.Count - 1)];
        else
            ViewMetaEnemy = viewMetaEnemy[Meta % viewMetaEnemy.Count];
        unitConfig.Material = ViewMetaEnemy.Material;
        unitConfig.MeshEnemy = ViewMetaEnemy.MetaMesh;
        unitConfig.DropConfig = DropConfig;
        return unitConfig;
    }
}
[Serializable]
public class ViewMetaEnemy
{
    public Material Material;
    public Mesh MetaMesh;
}