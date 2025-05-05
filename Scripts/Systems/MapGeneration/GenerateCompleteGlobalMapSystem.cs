using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Linq;
using UnityEngine;
using Statement;

namespace Client 
{
    public class GenerateCompleteGlobalMapSystem : MainEcsSystem
    {
        private readonly EcsFilterInject<Inc<GlobalMapGenerateCompleteEvent>> _filter = default;
        private readonly EcsPoolInject<GlobalMapGenerateCompleteEvent> _generateCompleteEvent = default;
        private readonly EcsPoolInject<GlobalMapComponent> _globalMapPool = default;
        private readonly EcsFilterInject<Inc<TestGameplayComponent>> _testGameplayFilter = default;
        public override MainEcsSystem Clone()
        {
            return new GenerateCompleteGlobalMapSystem();
        }
        public override void Run(IEcsSystems systems)
        {
            if (_testGameplayFilter.Value.GetEntitiesCount() > 0) return;
            foreach (var entity in _filter.Value)
            {
                ref var globalMapComp = ref _globalMapPool.Value.Get(entity);
                if(globalMapComp.PointsArray[globalMapComp.CurrentGlobalMapPointPosition.x,globalMapComp.CurrentGlobalMapPointPosition.y].PointType == PointTypes.Boss)
                {
                    
                    globalMapComp.CurrentBiomIndex++;
                }
                UIMapData uiMapData = new UIMapData();
                uiMapData.MapData = ConvertGlobalMapToUIMapData(BattleState.Instance.UpdateGlobalMapForUI(globalMapComp.CurrentBiomIndex));
                UIManagerRitualist.GetUIManager.UIMapManagerGlobal.UIMapDataVisualise(uiMapData.MapData);
            }
        }
        //MAde it static cause need it in another place
        public static UIMapData.MapPoint[,] ConvertGlobalMapToUIMapData(GlobalMapPoint[,] globalMapPoints)
        {
            int width = globalMapPoints.GetLength(0);
            int height = globalMapPoints.GetLength(1);

            UIMapData.MapPoint[,] uiMapPoints = new UIMapData.MapPoint[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GlobalMapPoint globalPoint = globalMapPoints[x, y];

                    UIMapData.MapPoint.PointType uiMapPointType;
                    UIMapData.MapPoint.PointState uiMapPointState;


                    uiMapPointState = UIMapData.MapPoint.PointState.Open;
                    uiMapPointType = UIMapData.MapPoint.PointType.Unknown;

                    //todo SEREGA globalPoint.Availability CHECK


                    // if (globalPoint.PointType == PointTypes.Target)
                    // {
                    //     uiMapPointType = UIMapData.MapPoint.PointType.Target;
                    // }
                    if (globalPoint.PointType == PointTypes.Battle)
                    {
                        uiMapPointType = UIMapData.MapPoint.PointType.Battle;
                    }
                    if (globalPoint.PointType == PointTypes.Boss)
                    {
                        uiMapPointType = UIMapData.MapPoint.PointType.Boss;
                    }
                    if (globalPoint.PointType == PointTypes.Altar)
                    {
                        //todo altar sprite
                        switch(globalPoint.AltarType)
                        {
                            case AltarTypes.Biba:
                                uiMapPointType = UIMapData.MapPoint.PointType.Heart; 
                                break;  
                            case AltarTypes.Boba:
                                uiMapPointType = UIMapData.MapPoint.PointType.Heart; 
                                break;
                        }
                        
                    }
                    if (globalPoint.PointType == PointTypes.Reward)
                    {
                        uiMapPointType = UIMapData.MapPoint.PointType.Chest;
                    }
                    //todo GENERATE


                    switch (globalPoint.PointState)
                    {
                        case PointStates.Open:
                            uiMapPointState = UIMapData.MapPoint.PointState.Open;
                            break;
                        case PointStates.Completed:
                            uiMapPointState = UIMapData.MapPoint.PointState.Completed;
                            break;
                        case PointStates.Closed:
                            uiMapPointState = UIMapData.MapPoint.PointState.Closed;
                            break;
                        case PointStates.ThisRoom:
                            uiMapPointState = UIMapData.MapPoint.PointState.ThisRoom;
                            break;
                    }

                    if (globalPoint.IsEmpty == true)
                    {
                        uiMapPointState = UIMapData.MapPoint.PointState.Empty;
                    }



                    var uiMapPoint = new UIMapData.MapPoint(globalPoint.Position, new Vector2Int(x,y), uiMapPointType, uiMapPointState, globalPoint.IsEmpty, globalPoint.IsLockedForBuild, false);
                    uiMapPoints[x, y] = uiMapPoint;
                    
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GlobalMapPoint globalPoint = globalMapPoints[x, y];
                    UIMapData.MapPoint uiMapPoint = uiMapPoints[x, y];

                    foreach (var exit in globalPoint.ForUIExitList)
                    {
                        uiMapPoint.ExitList.Add(uiMapPoints[exit.ForUIPosition.x, exit.ForUIPosition.y]);
                        // Vector2Int exitPosition = uiMapPoints[exit.ForUIPosition.x, exit.ForUIPosition.y];
                        // if (exitPosition.x >= 0 && exitPosition.x < width && exitPosition.y >= 0 && exitPosition.y < height)
                        // {
                            
                        // }
                    }
                    foreach (var enter in globalPoint.ForUIEnterList)
                    {
                        uiMapPoint.ExitList.Add(uiMapPoints[enter.ForUIPosition.x, enter.ForUIPosition.y]);
                        // Vector2Int enterPosition = enter.ForUIPosition;
                        // if (enterPosition.x >= 0 && enterPosition.x < width && enterPosition.y >= 0 && enterPosition.y < height)
                        // {
                        //     uiMapPoint.ExitList.Add(uiMapPoints[enterPosition.x, enterPosition.y]);
                        // }
                    }
                }
            }

            return uiMapPoints;
        }
    }

}