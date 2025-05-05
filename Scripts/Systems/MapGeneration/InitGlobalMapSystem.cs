using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client {

    sealed class InitGlobalMapSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<GlobalMapComponent> _globalMapPool = default;
        readonly EcsPoolInject<CreateGlobalMapSelfRequest> _createMapEventPool = default;
        readonly EcsPoolInject<GlobalMapGenerateCompleteEvent> _globalMapGenerateComplete = default;
        private bool V_NALICHII = false;

        public override MainEcsSystem Clone()
        {
            return new InitGlobalMapSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            int entity = _world.Value.NewEntity();

            State.Instance.RegisterNewEntity("GlobalMapEntity", entity);

            ref var globalMapComp = ref _globalMapPool.Value.Add(entity);
            //GENERATION вытащить карту из сохранений, если нет карты создать карту
            var data = PlayerEntity.Instance.Map;

            if (data == null || data.Points == null/* || !BattleState.Instance.IsMainScene*/)
            {
                _createMapEventPool.Value.Add(entity);
            } 
            else if (data.Points.Count != 0)
            {
                globalMapComp.PointsArray = new GlobalMapPoint[data.MaxWidth, data.MaxLength];
                for (int x = 0; x < data.MaxWidth; x++)
                {
                    for (int y = 0; y < data.MaxLength; y++)
                    {
                        globalMapComp.PointsArray[x, y] = new GlobalMapPoint();
                    }
                }
                foreach (var point in data.Points)
                {
                    globalMapComp.PointsArray[point.x, point.y].IsEmpty = point.IsEmpty;
                    globalMapComp.PointsArray[point.x, point.y].PointState = point.PointState;
                    globalMapComp.PointsArray[point.x, point.y].AltarType = point.AltarType;
                    globalMapComp.PointsArray[point.x, point.y].PointType = point.PointType;
                    globalMapComp.PointsArray[point.x, point.y].Position = new Vector2Int(point.x, point.y);
                    globalMapComp.PointsArray[point.x, point.y].ExitList = new System.Collections.Generic.List<GlobalMapPoint>();
                    globalMapComp.PointsArray[point.x, point.y].EnterList = new System.Collections.Generic.List<GlobalMapPoint>();
                    globalMapComp.PointsArray[point.x, point.y].BiomeIndex = point.BiomeIndex;
                    if (point.PointState == PointStates.ThisRoom)
                    {
                        
                    }
                    foreach (var exitPos in point.ExitPositions)
                    {
                        globalMapComp.PointsArray[point.x, point.y].ExitList.Add(globalMapComp.PointsArray[exitPos.x, exitPos.y]);
                    }
                    foreach (var enterPos in point.EnterPositions)
                    {
                        globalMapComp.PointsArray[point.x, point.y].EnterList.Add(globalMapComp.PointsArray[enterPos.x, enterPos.y]);
                    }
                }
                globalMapComp.MaxLength = data.MaxLength;
                globalMapComp.MaxWidth = data.MaxWidth;
                globalMapComp.CurrentGlobalMapPointPosition = new Vector2Int(data.CurrentGlobalMapPointPosition_x, data.CurrentGlobalMapPointPosition_y);
                globalMapComp.BiomCount = data.BiomCount;
                globalMapComp.BiomsIndexes = data.BiomsIndexes;
                globalMapComp.CurrentBiomIndex = data.CurrentBiomIndex;
                globalMapComp.BiomeLenLength = data.BiomeLength;


                BattleState.Instance.UpdateGlobalMapForUI(globalMapComp.CurrentBiomIndex);

                BattleState.Instance.currentMatchTick = data.currentMatchTick;
                BattleState.Instance.currentDamage = data.currentDamage;
                BattleState.Instance.kills = data.kills;

                _globalMapGenerateComplete.Value.Add(entity);
            }
            else if (data.Points.Count == 0)
            {
                _createMapEventPool.Value.Add(entity);
            }
            if(data != null)
            {
                PlayerEntity.Instance.Map.Dispose();
                SaveModule.SaveSingleData<PlayerData>();
                SaveModule.SaveSingleData<SlotDataContainer>();
            } 
        }
    }
}