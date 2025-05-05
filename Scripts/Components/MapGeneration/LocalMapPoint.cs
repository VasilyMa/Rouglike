using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMapPoint
{
    public Vector2Int Position;
    public bool IsEmpty;
    public List<LocalMapPoint> ExitList;
    public List<LocalMapPoint> EnterList;
    public PointTypes PointType;
    public PointSizeTypes PointTypeSize;
    public Dictionary<LocalMapPoint, GameObject> Links;
    public GameObject RoomGameObject;
    public int[] ConnectArray = new int[4];
    public Dictionary<Vector2Int, Transform> ExitTransforms;
    public List<ExitPointMB> ExitPointMBs = new List<ExitPointMB>();
    public List<Transform> RoomExitList;
    public List<Transform> RoomSpawnList;
    public List<Transform> RoomGateList;
    public RoomTypes RoomType;
    public GameObject PivotGameObject;
    public RoomMB RoomMB;
    public void Init()
    {
        IsEmpty = true;
        ExitList = new List<LocalMapPoint>();
        EnterList = new List<LocalMapPoint>();
        Links = new Dictionary<LocalMapPoint, GameObject>();
        PointType = PointTypes.Empty;
        RoomType = RoomTypes.Normal;
    }
    public void LockPoint()
    {
        foreach(var exit in ExitPointMBs)
        {
            exit.LockPoint();
        }
    }
    public void UnlockPoint()
    {
        foreach(var exit in ExitPointMBs)
        {
            exit.UnLockPoint();
        }
    }
    public void LockAllLinks()
    {
        foreach(var link in Links.Values)
        {
            link.SetActive(false);
        }
    }
    public void DestroyRoom()
    {
        GameObject.Destroy(RoomGameObject);
    }
    public void LockTargetLink(LocalMapPoint point)
    {
        Links[point].SetActive(false);
    }
    public void InitPointSize(int startPoint = 0)
    {
        if(IsEmpty) return;
        int connectCount = ExitList.Count + EnterList.Count + startPoint;
        if(connectCount >= 4) PointTypeSize = PointSizeTypes.Big;
        else if(connectCount == 3) PointTypeSize = PointSizeTypes.Normal;
        else if(connectCount <= 2)
        {
            if(ExitList.Count == 2) PointTypeSize = PointSizeTypes.Normal;
            else if(ExitList.Count == 1)
            {
                if(EnterList.Count > 1)
                {
                    PointTypeSize = PointSizeTypes.Normal;
                }
                else if(EnterList.Count == 1)
                {
                    Vector2Int enterPosition = EnterList[0].Position;
                    Vector2Int exitPosition = ExitList[0].Position;

                    if(enterPosition.x == exitPosition.x || enterPosition.y == exitPosition.y)
                    {
                        PointTypeSize = PointSizeTypes.Small;
                    }
                    else
                    {
                        PointTypeSize = PointSizeTypes.Normal;
                    }
                }
                else
                {
                    Vector2Int exitPosition = ExitList[0].Position;
                    if(exitPosition.x != Position.x)
                    {
                        PointTypeSize = PointSizeTypes.Normal;
                    }
                    else
                    {
                        PointTypeSize = PointSizeTypes.Small;
                    }
                }
            }
        }
    }
    
    public Transform GetExitPosition(Vector2Int direction)
    {
        return ExitTransforms[direction];
    }
}
public enum PointSizeTypes{
    Small, Normal, Big
}
public enum RoomTypes
{
    Start, End, Normal
}
