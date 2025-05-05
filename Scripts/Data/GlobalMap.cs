using System.Drawing;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Statement;

[System.Serializable]
public class GlobalMap
{
    public bool IsDispose;
    public List<MapPoint> Points;
    public int CurrentGlobalMapPointPosition_x;
    public int CurrentGlobalMapPointPosition_y;
    public int MaxLength;
    public int MaxWidth;
    public int BiomCount;
    public int[] BiomsIndexes;
    public int CurrentBiomIndex;
    public int BiomeLength;

    //data for run
    public float currentMatchTick;
    public float currentDamage;
    public int kills;

    public GlobalMap()
    {
	    Points = new List<MapPoint>();
    } 

    public void ProcessUpdataData()
    {
        if (State.Instance.EcsRunHandler == null) return;

        ref var globalMapComp = ref State.Instance.EcsRunHandler.World.GetPool<GlobalMapComponent>().Get(State.Instance.GetEntity("GlobalMapEntity"));
        CurrentGlobalMapPointPosition_x = globalMapComp.CurrentGlobalMapPointPosition.x;
        CurrentGlobalMapPointPosition_y = globalMapComp.CurrentGlobalMapPointPosition.y;

        MaxWidth = globalMapComp.MaxWidth;
        MaxLength = globalMapComp.MaxLength;
        BiomCount = globalMapComp.BiomCount;
        BiomsIndexes = globalMapComp.BiomsIndexes;
        CurrentBiomIndex = globalMapComp.CurrentBiomIndex;
        BiomeLength = globalMapComp.BiomeLenLength;

        currentDamage = BattleState.Instance.currentDamage;
        kills = BattleState.Instance.kills;
        currentMatchTick = BattleState.Instance.currentMatchTick;

        Points = new List<MapPoint>();
        for (int x = 0; x < globalMapComp.MaxWidth; x++)
        {
            for (int y = 0; y < globalMapComp.MaxLength; y++)
            {
                var globalMapPoint = globalMapComp.PointsArray[x, y];
                var mapPoint = new MapPoint();

                mapPoint.x = globalMapPoint.Position.x;
                mapPoint.y = globalMapPoint.Position.y;
                mapPoint.IsEmpty = globalMapPoint.IsEmpty;
                mapPoint.PointState = globalMapPoint.PointState;
                mapPoint.PointType = globalMapPoint.PointType;
                mapPoint.AltarType = globalMapPoint.AltarType;
                mapPoint.BiomeIndex = globalMapPoint.BiomeIndex;


                mapPoint.ExitPositions = new List<LinkGlobalMap>();
                foreach(var exitLink in globalMapPoint.ExitList)
                {
                    LinkGlobalMap link = new LinkGlobalMap();
                    link.x = exitLink.Position.x;
                    link.y = exitLink.Position.y;
                    mapPoint.ExitPositions.Add(link);
                }

                mapPoint.EnterPositions = new List<LinkGlobalMap>();
                foreach(var enterLink in globalMapPoint.EnterList)
                {
                    LinkGlobalMap link = new LinkGlobalMap();
                    link.x = enterLink.Position.x;
                    link.y = enterLink.Position.y;
                    mapPoint.EnterPositions.Add(link);
                }

                Points.Add(mapPoint);
            }
        }

    }

    public void Dispose()
    {
        // TODO: Remove data to default values, invokes where Clear Data
        if(Points != null) Points = null;
        CurrentGlobalMapPointPosition_x = 0;
        CurrentGlobalMapPointPosition_y = 0;
        MaxLength = 0;
        MaxWidth = 0;
        BiomCount = 0;
        BiomsIndexes = null;
        CurrentBiomIndex = 0;
        BiomeLength = 0;
        currentMatchTick = 0;
        currentDamage = 0;
        kills = 0;
    }
}

[System.Serializable]
public class MapPoint
{
    public int x; 
    public int y;
    public bool IsEmpty;
    public List<LinkGlobalMap> ExitPositions;
    public List<LinkGlobalMap> EnterPositions;
    public PointTypes PointType;
    public PointStates PointState;
    public AltarTypes AltarType;
    public int BiomeIndex;

}
[System.Serializable]
public class LinkGlobalMap
{
    public int x; 
    public int y;
}