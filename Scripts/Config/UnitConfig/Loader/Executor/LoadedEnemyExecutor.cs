using AbilitySystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
[Serializable]
public class LoadedEnemyExecutor : IExecutor
{
    private static EnemyMetaDataConfig[] allConfigs;
    private string NameEnemy = "Unknown";
    [Space(10)]
    public float HealthBase = 0;
    public float SpeedBase = 0;
    public float MaxValueToughnessBase = 0;
    public float DelayRecoveryToughnessBase = 0;
    public bool Percent = false;
    public float SpeedRecoveryBase = 0 ;
    [Space(10)]
    public float HealthUp;
    public float SpeedUp;
    public float MaxValueToughnessUp;
    public float DelayRecoveryToughnessUp;
    public float SpeedRecoveryUp;

    private int _dataWriteCount = 12;
    private int _sizeDataCurrent;
    public bool CheckFullData()
    {
        if(_sizeDataCurrent == _dataWriteCount) return true;
        return false;
    }
    private EnemyMetaDataConfig FindEnemyByName(string name)
    {
        foreach (var config in allConfigs)
        {
            if (config.name == name)
            {
                return config;
            }
        }
        return null;
    }

    public void Invoke()
    {
        if (!CheckFullData())
        {
            
            return;
        }
        if (allConfigs is null) allConfigs = Resources.LoadAll<EnemyMetaDataConfig>("");
        var enemyConfig = FindEnemyByName(NameEnemy);
        enemyConfig.DownloadedData = (LoadedEnemyExecutor)this.MemberwiseClone();
    }

    public void SetData(params string[] data)
    {
        if (data.Length < _dataWriteCount) return;
        _sizeDataCurrent = 1;
        NameEnemy = data[0];
        for (int i = 1; i < 5;i++)
        {
            GetAttributeByIndex(i) = TryParseValue<float>(data[i]);
        }
        Percent = TryParseValue<bool>(data[5]);
        for(int i = 6; i < _dataWriteCount; i++)
        {
            GetAttributeByIndex(i) = TryParseValue<float>(data[i]);
        }
    }
    private ref float GetAttributeByIndex(int index)
    {
        switch (index)
        {
            case 1:
                return ref HealthBase;
            case 2:
                return ref SpeedBase;
            case 3:
                return ref MaxValueToughnessBase;
            case 4:
                return ref DelayRecoveryToughnessBase;
            case 6:
                return ref SpeedRecoveryBase;
            case 7:
                return ref HealthUp;
            case 8:
                return ref SpeedUp;
            case 9:
                return ref MaxValueToughnessUp;
            case 10:
                return ref DelayRecoveryToughnessUp;
            case 11:
                return ref SpeedRecoveryUp;
            default:
                throw new ArgumentException("�������� ������"); // ��� ����� ��������� ����������
        }
    }
    private T TryParseValue<T>(string input)
    {
        if (typeof(T) == typeof(float))
        {
            if(float.TryParse(input, out float result))
            _sizeDataCurrent++;
            return (T)(object)result;
        }
        else if (typeof(T) == typeof(bool))
        {
            if(bool.TryParse(input, out bool result))
            _sizeDataCurrent++;
            return (T)(object)result;
        }
        return default(T);
    }
}


