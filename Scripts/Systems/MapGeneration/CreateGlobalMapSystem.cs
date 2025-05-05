using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;
using Statement;

namespace Client {
    sealed class CreateGlobalMapSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<CreateGlobalMapSelfRequest, GlobalMapComponent>> _filter = default;
        readonly EcsPoolInject<GlobalMapComponent> _globalMapPool = default;
        readonly EcsPoolInject<GlobalMapGenerateCompleteEvent> _globalMapGenerateComplete = default;
        readonly EcsSharedInject<GameState> _state = default;

        private int _maxWidth = 0;
        private int _maxLength = 0;
        private int _maxLinkCount = 0;
        private int _radius = 0;
        private int _enterCount = 0;
        private GlobalMapPoint[,] _pointsArray;
        private int _targetFloor = 0;
        private int _biomeCount = 0;
        int serviceLength = 0;

        public override MainEcsSystem Clone()
        {
            return new CreateGlobalMapSystem();
        }
        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                InitArray();
                GeneratePointsInFloor(_enterCount, _targetFloor, 0);
                serviceLength = _maxLength / _biomeCount;
                if(serviceLength == _maxLength) serviceLength--;
                for (int i = _biomeCount; i > 0;i--)
                {
                    _pointsArray[_maxWidth / 2, serviceLength * i].IsEmpty = false;
                    _pointsArray[_maxWidth / 2, serviceLength * i].PointType = PointTypes.Boss;
                }
                GeneratePoints();
                CreateLinks();
                SetPointType();
                SetPointState();
                SetPointStateAvailabilityForUI();

                ref var globalMapComp = ref _globalMapPool.Value.Get(entity);
                globalMapComp.PointsArray = _pointsArray;
                globalMapComp.MaxLength = _maxLength;
                globalMapComp.MaxWidth = _maxWidth;
                globalMapComp.CurrentGlobalMapPointPosition = new Vector2Int(_maxWidth / 2, 0);
                globalMapComp.BiomeLenLength = ConfigModule.GetConfig<MapConfig>().GlobalMapGenerationConfig.MaxLength;


                globalMapComp.BiomCount = _biomeCount;
                globalMapComp.CurrentBiomIndex = 0;
                BattleState.Instance.UpdateGlobalMapForUI(globalMapComp.CurrentBiomIndex);
                globalMapComp.BiomsIndexes = ConfigModule.GetConfig<MapConfig>().
                    LocalMapGenerationConfig.GetRandomBiomsByBiomCount(_biomeCount);

                _globalMapGenerateComplete.Value.Add(entity);
            }
        }
        public void InitArray()
        {
            var mapGenerationConfig = ConfigModule.GetConfig<MapConfig>().GlobalMapGenerationConfig;
            
            _maxWidth = mapGenerationConfig.MaxWidth;
            _enterCount = mapGenerationConfig.EnterCount;
            _radius = mapGenerationConfig.Radius;
            _enterCount = mapGenerationConfig.EnterCount;
            _biomeCount = mapGenerationConfig.BiomCount;
            _maxLength = mapGenerationConfig.MaxLength * _biomeCount + 1;
            _pointsArray = new GlobalMapPoint[_maxWidth, _maxLength];
            for (int i = 0; i < _maxLength; i++)
            {
                for (int j = 0; j < _maxWidth; j++)
                {
                    _pointsArray[j, i] = new GlobalMapPoint();
                    _pointsArray[j, i].Init();
                    _pointsArray[j, i].Position = new Vector2Int(j, i);
                }
            }
        }
        public void GeneratePointsInFloor(int count, int floor, int offset)
        {
            if (floor == 0)
            {
                _pointsArray[_maxWidth / 2, floor].IsEmpty = false;
            }
            else
            {
                int iterations = 0;
                while (count != 0 && iterations < 1000)
                {
                    var random = Random.Range(0 + offset, _maxWidth - offset);
                    if (_pointsArray[random, floor].IsEmpty)
                    {
                        _pointsArray[random, floor].IsEmpty = false;
                        count--;
                    }
                    iterations++;
                }
            }

        }
        public void GeneratePoints()
        {
            for (int i = 1; i < _maxLength - 1; i++)
            {
                if(_pointsArray[_maxWidth / 2, i].PointType == PointTypes.Boss) continue;
                var randomCount = Random.Range(Mathf.RoundToInt(_maxWidth * 0.3f), Mathf.RoundToInt(_maxWidth * 0.9f));
                int offSet = 0;//(MaxWidth - randomCount) / 2;
                GeneratePointsInFloor(randomCount, i, offSet);
            }
        }
        public void CreateLinks()
        {
            int indexJ = -1;
            int indexK = -1;
            for (int i = 0; i < _maxLength - 1; i++)
            {
                indexK = 0;
                for (int j = 0; j < _maxWidth; j++)
                {
                    if (_pointsArray[j, i].IsEmpty) continue;
                    indexJ = j;
                    for (int k = Mathf.Clamp(j - _radius, indexK, _maxWidth); k < Mathf.Clamp(j + _radius, indexK, _maxWidth); k++)
                    {
                        if (_pointsArray[k, i + 1].IsEmpty) continue;

                        if (_pointsArray[j, i].ExitList.Count >= _maxLinkCount) break;

                        _pointsArray[j, i].ExitList.Add(_pointsArray[k, i + 1]);
                        _pointsArray[k, i + 1].EnterList.Add(_pointsArray[j, i]);
                        indexK = k;
                    }
                    if (_pointsArray[j, i].ExitList.Count == 0)
                    {
                        int index = 0;
                        int distance = _maxWidth;
                        for (int l = 0; l < _maxWidth; l++)
                        {
                            if (_pointsArray[l, i + 1].IsEmpty) continue;
                            if (Mathf.Abs(j - l) < distance)
                            {
                                distance = Mathf.Abs(j - l);
                                index = l;
                            }
                        }
                        _pointsArray[j, i].ExitList.Add(_pointsArray[index, i + 1]);
                        _pointsArray[index, i + 1].EnterList.Add(_pointsArray[j, i]);
                    }
                }
                for (int j = 0; j < _maxWidth; j++)
                {
                    if (_pointsArray[j, i + 1].IsEmpty) continue;
                    if (_pointsArray[j, i + 1].EnterList.Count == 0)
                    {
                        int index = 0;
                        int distance = _maxWidth;
                        for (int l = 0; l < _maxWidth; l++)
                        {
                            if (_pointsArray[l, i].IsEmpty) continue;
                            if (Mathf.Abs(j - l) < distance)
                            {
                                distance = Mathf.Abs(j - l);
                                index = l;
                            }
                        }
                        _pointsArray[j, i + 1].EnterList.Add(_pointsArray[index, i]);
                        _pointsArray[index, i].ExitList.Add(_pointsArray[j, i + 1]);
                    }
                }
            }

        }
        public void SetPointType()
        {
            for (int i = 0; i < _maxWidth; i++)
            {
                for (int j = 0; j < _maxLength; j++)
                {
                    GlobalMapPoint point = _pointsArray[i, j];
                    if (!point.IsEmpty && point.PointType == PointTypes.Empty)
                    {
                        point.PointType = (PointTypes)Random.Range(1, 5);
                        if (point.PointType == PointTypes.Altar)
                        {
                            //todo altar выбор типа для алтаря
                            point.RandomAltarType();
                        }
                    }
                }
            }
        }
        public void SetPointState()
        {
            for (int i = 0; i < _maxWidth; i++)
            {
                for (int j = 0; j < _maxLength; j++)
                {
                    GlobalMapPoint point = _pointsArray[i, j];
                    point.PointState = PointStates.Closed;
                }
            }
            _pointsArray[_maxWidth / 2, 0].PointState = PointStates.ThisRoom;

            foreach (var link in _pointsArray[_maxWidth / 2, 0].ExitList)
            {
                link.PointState = PointStates.Open;
            }
        }
        public void SetPointStateAvailabilityForUI()
        {
            for (int i = 0; i < _maxWidth; i++)
            {
                for (int j = 0; j < _maxLength; j++)
                {
                    _pointsArray[i, j].BiomeIndex = (j - 1) / serviceLength;
                }
            }
        }
    }
}