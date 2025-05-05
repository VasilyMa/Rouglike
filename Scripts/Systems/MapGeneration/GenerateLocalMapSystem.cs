using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

namespace Client {
    sealed class GenerateLocalMapSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<GenerateLocalMapSelfRequest, LocalMapComponent>> _filter = default;
        readonly EcsPoolInject<LocalMapComponent> _localMapPool = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<CreateCurrentLocalMapPointSelfRequest> _evt = default;
        private int _maxWidth = 0;
        private int _maxLength = 0;
        private int _maxLinkCount = 0;
        private int _radius = 0;
        private LocalMapPoint[,] _pointsArray;
        private Vector2Int[] _directions = new Vector2Int[4] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

        public override MainEcsSystem Clone()
        {
            return new GenerateLocalMapSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                InitArray();
                _pointsArray[_maxWidth / 2, _maxLength - 1].IsEmpty = false;
                _pointsArray[_maxWidth / 2, _maxLength - 1].RoomType = RoomTypes.End;
                _pointsArray[_maxWidth / 2, 0].IsEmpty = false;
                _pointsArray[_maxWidth / 2, 0].RoomType = RoomTypes.Start;

                GeneratePoints();
                CreateLinks();
                CreateNewPoints();
                for (int i = 0; i < _maxWidth; i++)
                {
                    for (int j = 0; j < _maxLength; j++)
                    {
                        int startP = 0;
                        if(_pointsArray[i, j].RoomType == RoomTypes.Start) startP = 1;
                        _pointsArray[i, j].InitPointSize(startP);
                        CheckNeighbours(_pointsArray[i, j]);

                    }
                }
                // CreatePoints();
                // DrawLine();

                ref var localMapComp = ref _localMapPool.Value.Get(entity);
                localMapComp.PointsArray = _pointsArray;
                //todo ???
                localMapComp.CurrentLocalMapPoint = _pointsArray[_maxWidth / 2, 0];
                //localMapComp.CurrentLocalMapPoint.ConnectArray[2] = 1;
                localMapComp.LastPosition = new Vector2Int(localMapComp.CurrentLocalMapPoint.Position.x, -1);
                localMapComp.MaxLength = _maxLength;
                localMapComp.MaxWidth = _maxWidth;

                ref var createComp = ref _evt.Value.Add(entity);
                createComp.CreatePosition = Vector3.zero;
            } 
        }
        public void InitArray()
        {
            var mapGenerationConfig = ConfigModule.GetConfig<MapConfig>().LocalMapGenerationConfig;
            _maxWidth = mapGenerationConfig.MaxWidth;
            _maxLength = mapGenerationConfig.MaxLength;
            _radius = mapGenerationConfig.Radius;
            _pointsArray = new LocalMapPoint[_maxWidth,_maxLength];
            for (int i = 0; i < _maxLength; i++)
            {
                for (int j = 0; j < _maxWidth; j++)
                {
                    _pointsArray[j, i] = new LocalMapPoint();
                    _pointsArray[j, i].Init();
                    _pointsArray[j, i].Position = new Vector2Int(j, i);
                }
            }
        }
        public void GeneratePointsInFloor(int count, int floor, int offset)
        {
            int iterations = 0;
            while(count != 0 && iterations < 1000)
            {
                var random = Random.Range(0 + offset, _maxWidth - offset);
                if(_pointsArray[random, floor].IsEmpty)
                {
                    _pointsArray[random, floor].IsEmpty = false;
                    count--;
                }
                iterations++;
                if (iterations >= 1000)
                {
                    
                }
            }
        }
        public void GeneratePoints()
        {
            for (int i = 1; i < _maxLength - 1; i++)
            {
                var randomCount = Random.Range(Mathf.RoundToInt(_maxWidth * 0.3f), Mathf.RoundToInt(_maxWidth * 0.9f));
                int offSet = 0;//(MaxWidth - randomCount) / 2;
                GeneratePointsInFloor(randomCount, i, offSet);
            }
        }
        public void CreatePoints()
        {
            for (int i = 0; i < _maxWidth; i++)
            {
                for (int j = 0; j < _maxLength; j++)
                {
                    _pointsArray[i, j].InitPointSize();
                    _pointsArray[i,j].RoomGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    _pointsArray[i,j].RoomGameObject.name = i.ToString() + " - " + j.ToString();
                    _pointsArray[i,j].RoomGameObject.transform.position = new Vector3(i, j, 0);
                    //PointsArray[i,j].GO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    if(!_pointsArray[i,j].IsEmpty)
                    {
                        Vector3 big = Vector3.one;
                        Vector3 small = new Vector3(0.3f, 0.3f, 0.3f);
                        Vector3 normal = new Vector3(0.5f, 0.5f, 0.5f);
                        Vector3 size = Vector3.zero;
                        switch(_pointsArray[i, j].PointTypeSize)
                        {
                            case PointSizeTypes.Small:
                                size = small;
                                break;
                             case PointSizeTypes.Normal:
                                size = normal;
                                break;
                                 case PointSizeTypes.Big:
                                size = big;
                                break;
                        }
                        _pointsArray[i,j].RoomGameObject.transform.localScale = size;
                    }
                    else
                    {
                        _pointsArray[i,j].RoomGameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            }   
        }
        public void CheckNeighbours(LocalMapPoint point)
        {

            for (int i = 0; i < _directions.Length; i++)
            {
                var pos = point.Position + _directions[i];
                try
                {
                    if(_pointsArray[pos.x, pos.y].IsEmpty)
                    {
                        point.ConnectArray[i] = 0;
                    }
                    else
                    {
                        if(point.ExitList.Contains(_pointsArray[pos.x, pos.y]))
                        {
                            point.ConnectArray[i] = 1;
                        }
                        if(point.EnterList.Contains(_pointsArray[pos.x, pos.y]))
                        {
                            point.ConnectArray[i] = 1;
                        }
                    }
                }
                catch
                {
                    point.ConnectArray[i] = 0;
                }
            }
            if(point.RoomType == RoomTypes.Start)
            {
                point.ConnectArray[2] = 1;
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
                    if(_pointsArray[j,i].IsEmpty) continue;
                    indexJ = j;
                    for (int k = Mathf.Clamp(j - _radius, indexK, _maxWidth); k < Mathf.Clamp(j + _radius, indexK, _maxWidth); k++)
                    {
                        if(_pointsArray[k, i + 1].IsEmpty) continue;
                        
                        if(_pointsArray[j, i].ExitList.Count >= _maxLinkCount) break;
                        
                        _pointsArray[j, i].ExitList.Add(_pointsArray[k, i + 1]);
                        _pointsArray[k, i + 1].EnterList.Add(_pointsArray[j, i]);
                        indexK = k;
                    }
                    if(_pointsArray[j, i].ExitList.Count == 0)
                    {
                        int index = 0;
                        int distance = _maxWidth;
                        for (int l = 0; l < _maxWidth; l++)
                        {
                            if(_pointsArray[l, i + 1].IsEmpty) continue;
                            if(Mathf.Abs(j - l) < distance)
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
                    if(_pointsArray[j, i + 1].IsEmpty) continue;
                    if(_pointsArray[j, i + 1].EnterList.Count == 0)
                    {
                        int index = 0;
                        int distance = _maxWidth;
                        for (int l = 0; l < _maxWidth; l++)
                        {
                            if(_pointsArray[l, i].IsEmpty) continue;
                            if(Mathf.Abs(j - l) < distance)
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
        public void DrawLine()
        {
            for (int i = 0; i < _maxLength - 1; i++)
            {
                for (int j = 0; j < _maxWidth; j++)
                {
                    if (_pointsArray[j, i].IsEmpty) continue;
                    foreach (var link in _pointsArray[j, i].ExitList)
                    {

                        //int indexColor = Mathf.Clamp(i, 0, _colors.Length - 1);
                        GameObject linkGO = new GameObject("Link " + j.ToString() + " " + i.ToString());
                        _pointsArray[j, i].Links.Add(link, linkGO);
                        LineRenderer lineRenderer = linkGO.AddComponent<LineRenderer>();
                        // lineRenderer.startColor = _colors[indexColor];
                        // lineRenderer.endColor = _colors[indexColor];
                        lineRenderer.material = default;
                        //lineRenderer.material.color = _colors[indexColor];
                        lineRenderer.positionCount = 2;
                        lineRenderer.startWidth = 0.1f;
                        lineRenderer.endWidth = 0.1f;
                        lineRenderer.SetPosition(0, _pointsArray[j, i].RoomGameObject.transform.position);
                        lineRenderer.SetPosition(1, link.RoomGameObject.transform.position);
                        var arrow = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        arrow.transform.position = link.RoomGameObject.transform.position - (link.RoomGameObject.transform.position - _pointsArray[j, i].RoomGameObject.transform.position).normalized * 0.3f;
                        arrow.transform.localScale = new Vector3(0.2f, 0.3f, 0.1f);
                        arrow.transform.SetParent(linkGO.transform);
                    }
                }
            }
        }
        public void CreateNewPoints()
        {
            List<LocalMapPoint> noEmptyServiceList = new List<LocalMapPoint>();
            for (int i = 0; i < _maxWidth; i++)
            {
                for (int j = 0; j < _maxLength; j++)
                {
                    if (!_pointsArray[i, j].IsEmpty)
                    {
                        noEmptyServiceList.Add(_pointsArray[i, j]);
                    }
                }
            }
            foreach(var point in noEmptyServiceList)
            {
                List<LocalMapPoint> serviceList = new List<LocalMapPoint>();
                foreach(var link in point.ExitList)
                {
                    serviceList.Add(link);
                }
                foreach(var link in serviceList)
                {
                    FindPathPoints(point, link);
                }
            }
        }
        public void FindPathPoints(LocalMapPoint pointA, LocalMapPoint pointB)
        {
            Vector2Int currentPosition = pointA.Position;
            Vector2Int lastPosition = pointA.Position;
            int directionValue = pointA.Position.x - pointB.Position.x;
            if(directionValue > 0) directionValue = -1;
            else if(directionValue < 0) directionValue = 1;
            pointA.ExitList.Remove(pointB);
            pointB.EnterList.Remove(pointA);

            int iterations = 0;
            while(currentPosition != pointB.Position && iterations < 1000)
            {
                lastPosition = currentPosition;
                if(currentPosition.x != pointB.Position.x)
                {
                    currentPosition = new Vector2Int(currentPosition.x + directionValue, currentPosition.y);
                }
                else if(currentPosition.y != pointB.Position.y)
                {
                    currentPosition = new Vector2Int(currentPosition.x, currentPosition.y + 1);
                }
                _pointsArray[currentPosition.x, currentPosition.y].IsEmpty = false;

                if(currentPosition == lastPosition) continue;
                if(!_pointsArray[currentPosition.x, currentPosition.y].EnterList.Contains(_pointsArray[lastPosition.x, lastPosition.y])) _pointsArray[currentPosition.x, currentPosition.y].EnterList.Add(_pointsArray[lastPosition.x, lastPosition.y]);
                if(!_pointsArray[lastPosition.x, lastPosition.y].ExitList.Contains(_pointsArray[currentPosition.x, currentPosition.y])) _pointsArray[lastPosition.x, lastPosition.y].ExitList.Add(_pointsArray[currentPosition.x, currentPosition.y]);
                //pointA.ExitList.Add(PointsArray[currentPosition.x, currentPosition.y]);
                iterations++;
            }
        }
    }
}
