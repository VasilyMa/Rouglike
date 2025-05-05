using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapPoint
{
    public bool IsLockedForBuild;
    public Vector2Int Position;
    public Vector2Int ForUIPosition;
    public bool IsEmpty;
    public List<GlobalMapPoint> ExitList;
    public List<GlobalMapPoint> ForUIExitList;
    public List<GlobalMapPoint> EnterList;
    public List<GlobalMapPoint> ForUIEnterList;
    public PointTypes PointType;
    public PointStates PointState;
    public AltarTypes AltarType;
    public int BiomeIndex;

    public void Init()
    {
        IsEmpty = true;
        ExitList = new List<GlobalMapPoint>();
        EnterList = new List<GlobalMapPoint>();
        PointType = PointTypes.Empty;
        PointState = PointStates.Closed;

    }
    public void RandomAltarType()
    {
        AltarType = (AltarTypes)Random.Range(0, 2); 
    }
}
public enum PointTypes 
{
    Empty, Reward, Battle, Altar, Undefined, Boss
}
//todo altar новая enum для алтарей
public enum AltarTypes
{
    Biba = 0, Boba = 1 //todo rewrite after test
}
public enum PointStates
{
    Open,
    Completed,
    ThisRoom,
    Closed
}
