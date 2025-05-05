using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

[CreateAssetMenu(fileName = "RoomGOConfig", menuName = "Configs/RoomGOConfig")]
public class RoomGOConfig : ScriptableObject
{
    public int DistanceBetweenRooms = 50;
    public GameObject[] Backdrops;
    public GameObject EnterDungeon;
    public GameObject ExitDungeon;

    public GameObject[] BigRooms;
    public GameObject[] NormalRooms;
    public GameObject[] SmallRooms;
    public SplineDrawer[] Corridors;
    public GameObject[] Docks;
    public GameObject[] BossRooms;
    public GameObject[] RewardRooms;
    [SerializeReference] List<AltarContainer> AltarRooms;
    
    public GameObject LockExitGameObject;
    public GameObject LockEnterGameObject;
    [Header("EnemyTiers")]
    public EnemyBiom EnemyBiom;
    [Header("PostProcessing")]
    public GameObject PostProcessingGO;
    public RenderSettingsSO RenderSettingsSO;
    [Header("BridgesSetting")]
    public Vector2[] MainMeshHalfPositions;
    public Vector2[] RailingHalfPositions;

    public EventReference BiomAmbient;

    public GameObject GetRoomBySizeType(PointSizeTypes size)
    {
        GameObject value = null;
        switch(size)
        {
            case PointSizeTypes.Small:
                value = SmallRooms[UnityEngine.Random.Range(0, SmallRooms.Length)];
                break;
            case PointSizeTypes.Normal:
                value = NormalRooms[UnityEngine.Random.Range(0, NormalRooms.Length)];
                break;
            case PointSizeTypes.Big:
                value = BigRooms[UnityEngine.Random.Range(0, BigRooms.Length)];
                break;
        }
        return value;
    }
    public GameObject GetDock()
    {
        return Docks[UnityEngine.Random.Range(0, Docks.Length)];
    }
    public SplineDrawer GetCorridor()
    {
        return Corridors[UnityEngine.Random.Range(0, Corridors.Length)]; 
    }
    public GameObject GetBossRoom()
    {
        return BossRooms[UnityEngine.Random.Range(0, BossRooms.Length)];
    }
    public GameObject GetAltarRoom(AltarTypes altarType)
    {
        GameObject value = null;
        foreach(var container in AltarRooms)
        {
            if(container.AltarKey == altarType)
            {
                value = container.GetRandomAltar();
            }
        }
        if(value == null) value = AltarRooms[0].GetRandomAltar();

        return value;
    }
    public GameObject GetRewardRoom()
    {
        return RewardRooms[UnityEngine.Random.Range(0, RewardRooms.Length)];
    }
    public GameObject GetLastRoomByPointType(GlobalMapPoint point)
    {
        GameObject value = null;
        if(point.PointType == PointTypes.Undefined)
        {
            point.PointType = (PointTypes)UnityEngine.Random.Range(1, 4);
            if(point.PointType == PointTypes.Altar)
            {
                point.RandomAltarType();
            } 
        }
        switch(point.PointType)
        {
            case PointTypes.Reward:
                value = GetRewardRoom();
                break;
            case PointTypes.Battle:
                value = GetRoomBySizeType(PointSizeTypes.Small);
                break;
            case PointTypes.Altar: 
                //todo altar подсосать тип алтаря
                value = GetAltarRoom(point.AltarType);
                break;
            case PointTypes.Boss:
                value = GetBossRoom();
                break;
        }
        return value;
    }
    public GameObject GetBackDrop()
    {
        return Backdrops[UnityEngine.Random.Range(0, Backdrops.Length)];
    }

    private Vector3[] Directions = new Vector3[4]{new Vector3(0, 0, 1),new Vector3(1, 0, 0),new Vector3(0, 0, -1),new Vector3(-1, 0, 0)};
    private Vector2Int[] Vector2IntDirections = new Vector2Int[4] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
    public int GetIndexDirectionByVector3(Vector3 vector)
    {
        int value = 0;
        for (int i = 0; i < Directions.Length; i++)
        {
            if(Directions[i] == vector)
            {
                value = i;
            }
        }
        return value;
    }
    public int GetIndexDirectionByVector2Int(Vector2Int vector)
    {
        int value = 0;
        for (int i = 0; i < Vector2IntDirections.Length; i++)
        {
            if(Vector2IntDirections[i] == vector)
            {
                value = i;
            }
        }
        return value;
    }
}
[Serializable]
public class AltarContainer
{
    public AltarTypes AltarKey;
    [SerializeReference] public List<GameObject> Altars = new List<GameObject>();
    public GameObject GetRandomAltar()
    {
        return Altars[UnityEngine.Random.Range(0, Altars.Count)];
    }
}
