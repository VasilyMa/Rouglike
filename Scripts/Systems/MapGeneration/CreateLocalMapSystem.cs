using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Splines;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using Statement;
using FMODUnity;
using FMOD.Studio;

namespace Client {
    sealed class CreateLocalMapSystem : MainEcsSystem 
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<GenerateLocalMapSelfRequest>> _filter = default;
        readonly EcsPoolInject<LocalMapComponent> _localMapPool = default;
        readonly EcsPoolInject<GlobalMapComponent> _globalMapPool = default;
        readonly EcsPoolInject<UpdateSplineInstances> _updateInstances = default;
        readonly EcsPoolInject<LightSettingChangeRequest> _postProcessingPool = default;
        readonly EcsWorldInject _world = default;
        private int _tier = -1;
        private RoomGOConfig _biomeConfig;
        private EventInstance _ambientInstance;
        public override MainEcsSystem Clone()
        {
            return new CreateLocalMapSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                var state = BattleState.Instance;

                ref var localMapComp = ref _localMapPool.Value.Get(entity);
                ref var globalMapComp = ref _globalMapPool.Value.Get(state.GetEntity("GlobalMapEntity"));
                LocalMapGenerationConfig localMapGenerationConfig = ConfigModule.GetConfig<MapConfig>().LocalMapGenerationConfig;
                _biomeConfig = localMapGenerationConfig.GetBiomByIndex(globalMapComp.BiomsIndexes[globalMapComp.CurrentBiomIndex]);

                SoundEntity.Instance.PlayAmbientAttached(_biomeConfig.BiomAmbient);
                int floor = globalMapComp.CurrentGlobalMapPointPosition.y - 1;
                int round = ConfigModule.GetConfig<MapConfig>().GlobalMapGenerationConfig.MaxLength;
                _tier = (floor % (round) < (round / 2f)) ? 0 : 1;

                

                ref var updateInstances = ref _updateInstances.Value.Add(_world.Value.NewEntity());
                
                //todo получить конфигу
                for (int x = 0; x < localMapComp.MaxWidth; x++)
                {
                    for (int y = 0; y < localMapComp.MaxLength; y++)
                    {
                        Vector3 position = new Vector3(localMapComp.PointsArray[x, y].Position.x, 0, localMapComp.PointsArray[x, y].Position.y) * _biomeConfig.DistanceBetweenRooms;
                        if(!localMapComp.PointsArray[x,y].IsEmpty) 
                        {
                            //todo родить каждую комнату в правильной позиции
                            //Vector3 position = new Vector3(localMapComp.PointsArray[x, y].Position.x, 0, localMapComp.PointsArray[x, y].Position.y) * 50;
                            GameObject room = SpawnRoom(localMapComp.PointsArray[x, y], ref localMapComp, position, _biomeConfig);
                        }
                        else
                        {
                            GameObject backDrop = _biomeConfig.GetBackDrop();
                            if(backDrop)
                            {
                                var go = GameObject.Instantiate(backDrop, position, Quaternion.identity);
                                go.transform.SetParent(state.navMeshSurface.transform);
                            } 
                        }
                        
                    }
                }
                for (int x = 0; x < localMapComp.MaxWidth; x++)
                {
                    for (int y = 0; y < localMapComp.MaxLength; y++)
                    {
                        if(!localMapComp.PointsArray[x,y].IsEmpty) 
                        {
                            //делаем коридоры
                            foreach(var link in localMapComp.PointsArray[x,y].ExitList)
                            {
                                Vector2Int directionInt = link.Position - localMapComp.PointsArray[x, y].Position;

                                Transform start = localMapComp.PointsArray[x,y].GetExitPosition(directionInt);
                                Transform end = link.GetExitPosition(directionInt * -1);
                                SplineDrawer drawerPrefab = _biomeConfig.GetCorridor();
                                var generator = GameObject.Instantiate(localMapGenerationConfig.MeshCreator).GetComponent<MeshCreator>();
                                generator.GenerateBridge(_biomeConfig, start, end, drawerPrefab);
                                generator.transform.SetParent(start);
                                //отключить стенки
                                var exitMB = start.GetComponent<ExitPointMB>();
                                exitMB.SetActiveFalseWall();

                                var exitMBend = end.GetComponent<ExitPointMB>();
                                exitMBend.SetActiveFalseWall();

                                exitMB.Init(true);
                            }
                            
                            if(localMapComp.PointsArray[x,y].RoomType == RoomTypes.Start)
                            {
                                Transform connectPoint = localMapComp.PointsArray[x, y].GetExitPosition(Vector2Int.down);
                                var enterGO = GameObject.Instantiate(_biomeConfig.EnterDungeon);
                                enterGO.transform.SetParent(connectPoint);
                                enterGO.transform.rotation = Quaternion.Euler(0, 0, 0);
                                enterGO.transform.localPosition = Vector3.zero;

                                localMapComp.StartTransform = enterGO.transform;

                                var exit = connectPoint.GetComponent<ExitPointMB>();
                                exit.SetActiveFalseWall();
                                exit._collider.center = new Vector3(0, 0, 4);
                            }
                            if(localMapComp.PointsArray[x,y].RoomType == RoomTypes.End)
                            {   
                                Transform connectPoint = localMapComp.PointsArray[x, y].GetExitPosition(Vector2Int.up);
                                var exitGO = GameObject.Instantiate(_biomeConfig.ExitDungeon);
                                exitGO.transform.SetParent(connectPoint);
                                exitGO.transform.rotation = Quaternion.Euler(0, 0, 0);
                                exitGO.transform.localPosition = Vector3.zero;

                                var exit = connectPoint.GetComponent<ExitPointMB>();
                                exit.Init(true);
                                //exit.SetActiveFalseWall();
                                exit._collider.center = new Vector3(0, 0, -4);
                            }

                            

                        }
                    }
                }
                for (int x = 0; x < localMapComp.MaxWidth; x++)
                {
                    for (int y = 0; y < localMapComp.MaxLength; y++)
                    {
                        foreach(var exit in localMapComp.PointsArray[x,y].ExitPointMBs)
                        {
                            exit.SetActiveFalseEnter();
                        }
                    }
                }
                state.navMeshSurface.transform.rotation = Quaternion.Euler(0, 45, 0);
                state.UpdateNavMesh();

                //пост процессинг
                ref var postProcessingRequestComp = ref _postProcessingPool.Value.Add(_world.Value.NewEntity());
                postProcessingRequestComp.TargetSettings = _biomeConfig.RenderSettingsSO;
                postProcessingRequestComp.PostProcessingGO = _biomeConfig.PostProcessingGO;

            }
        }
        private GameObject SpawnRoom(LocalMapPoint point, ref LocalMapComponent localMapComp, Vector3 createPosition, RoomGOConfig localMapConfig) //
        {
            var state = BattleState.Instance;
            GameObject roomGo = null;
            ref var globalMapComp = ref _globalMapPool.Value.Get(state.GetEntity("GlobalMapEntity"));
            if(point.RoomType == RoomTypes.End)
            {
                
                roomGo = GameObject.Instantiate(localMapConfig.GetLastRoomByPointType(globalMapComp.PointsArray[globalMapComp.CurrentGlobalMapPointPosition.x,globalMapComp.CurrentGlobalMapPointPosition.y])); //globalMapComp.PointsArray[globalMapComp.CurrentGlobalMapPointPosition.x,globalMapComp.CurrentGlobalMapPointPosition.y].PointType
            }
            else
            {
                roomGo = GameObject.Instantiate(localMapConfig.GetRoomBySizeType(point.PointTypeSize));
            }
            

            var blockoutPrefabs = roomGo.GetComponentsInChildren<BlockoutPrefabMB>();
            foreach (BlockoutPrefabMB blockoutPrefab in blockoutPrefabs)
            {
                blockoutPrefab.PlaceProps();
            }
            point.RoomGameObject = roomGo;

            point.RoomExitList = new List<Transform>();
            point.RoomSpawnList = new List<Transform>();
            point.RoomGateList = new List<Transform>();

            // Vector2Int enterDirection =   localMapComp.CurrentLocalMapPoint.Position - point.Position;
            // if(enterDirection == Vector2Int.zero)
            // {
            //     enterDirection =  localMapComp.LastPosition - localMapComp.CurrentLocalMapPoint.Position;
            // }
            //Vector3 center = Vector3.zero;
            foreach(var child in roomGo.transform.GetComponentsInChildren<Transform>())
            {
                var name = child.name;
                if(name.Contains("RoomExit"))
                {
                    point.RoomExitList.Add(child);
                }
                if(name.Contains("SpawnPoint"))
                {
                    point.RoomSpawnList.Add(child);
                }
                if(name.Contains("Gate"))
                {
                    point.RoomGateList.Add(child);
                }
            }
            //установка выходов
            point.ExitTransforms = new Dictionary<Vector2Int, Transform>();
            foreach(var exit in point.RoomExitList)
            {
                var exitPointMB = exit.gameObject.AddComponent<ExitPointMB>();
                exitPointMB.CurrentLocalMapPoint = point;
                exitPointMB.LockExitGameObject = GameObject.Instantiate(localMapConfig.LockExitGameObject);
                
                exitPointMB.LockExitGameObject.transform.SetParent(exit);
                exitPointMB.LockExitGameObject.transform.localPosition = new Vector3(0,0,0);
                exitPointMB.LockExitGameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                exitPointMB.LockExitGameObject.SetActive(false);

                exitPointMB.LockEnterGameObject = GameObject.Instantiate(localMapConfig.LockEnterGameObject);
                exitPointMB.LockEnterGameObject.transform.position = exit.gameObject.transform.position;
                exitPointMB.LockEnterGameObject.transform.SetParent(exit);
                exitPointMB.LockEnterGameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                exitPointMB.LockEnterGameObject.SetActive(false);

                point.ExitPointMBs.Add(exitPointMB);
                var collider = exit.gameObject.AddComponent<BoxCollider>();
                collider.size = new Vector3(20, 4, 2);
                collider.center = new Vector3(0, 0, 4);
                exitPointMB._collider = collider;
                collider.isTrigger = true;
            }

            //понимаем с какой стороны выходы у комнаты
            int[] serviceArray = new int[4];
            
            foreach (var item in point.RoomExitList)
            {
                var forward = item.forward.normalized * -1;
                int index = localMapConfig.GetIndexDirectionByVector3(forward);
                serviceArray[index] = 1;
            }
            //point.ConnectArray[localMapConfig.GetIndexDirectionByVector2Int(enterDirection)] = 1;

            bool isDone = false;
            int multiply = 0;
            //определяем правильный поворот для нашей комнаты
            int iterations = 0;
            while(!isDone && iterations < 5) 
            {
                bool recalculateServiceArray = false;
                for (int i = 0; i < serviceArray.Length; i++)
                {
                    if(point.ConnectArray[i] == 1 && point.ConnectArray[i] != serviceArray[i])
                    {
                        recalculateServiceArray = true;
                        multiply++;
                        break;
                    }
                }
                if(recalculateServiceArray)
                {
                    int lastElement = serviceArray[serviceArray.Length - 1];
                    for (int i = serviceArray.Length - 1; i > 0; i--)
                    {
                        serviceArray[i] = serviceArray[i - 1];
                    }
                    serviceArray[0] = lastElement;
                }
                else
                {
                    isDone = true;
                }
                iterations++;
                if (iterations >= 5)
                {
                    
                }
            }

            roomGo.transform.rotation = Quaternion.Euler(0, multiply * 90, 0);



            var pivot = new GameObject("Pivot");

            foreach (var exit in point.RoomExitList)
            {
                //center += exit.transform.position;
                Vector2Int direction = new Vector2Int((int)exit.forward.normalized.x * -1, (int)exit.forward.normalized.z * -1);

                point.ExitTransforms.Add(direction, exit);
            }
            // center /= point.RoomExitList.Count;
            // center = new Vector3(center.x, 0, center.z);
            pivot.transform.position = roomGo.transform.position;
            roomGo.transform.SetParent(pivot.transform);
            pivot.transform.position = new Vector3(createPosition.x, 0, createPosition.z);
            //roomGo.transform.position = new Vector3(createPosition.x, 0, createPosition.z);

            //навешимаем на комнату румМБ и заполняем список спавнпойнтов
            RoomMB roomMB =  pivot.AddComponent<RoomMB>();
            roomMB.SpawnPoints = new Transform[point.RoomSpawnList.Count];
            int spawnPointIndex = 0;
            foreach(var spawnPoint in point.RoomSpawnList)
            {
                roomMB.SpawnPoints[spawnPointIndex] = spawnPoint;
                spawnPointIndex++;
            }
            roomMB.Init(_biomeConfig, _tier, globalMapComp.PointsArray[globalMapComp.CurrentGlobalMapPointPosition.x,globalMapComp.CurrentGlobalMapPointPosition.y].PointType == PointTypes.Boss && point.RoomType == RoomTypes.End);
            //pivot.transform.rotation = Quaternion.Euler(0, 45, 0);
            pivot.transform.SetParent(state.navMeshSurface.transform);
            point.RoomMB = roomMB;
            //foreach(var statcBatchingMB in roomGo.GetComponentsInChildren<StaticBatchingUtilityMB>())
            //{
            //    statcBatchingMB.StaticBatchingUtilityFunc();
            //}

            return pivot;
        }

        private void CombineMeshes(GameObject rootObject)
        {
            MeshFilter[] meshFilters = rootObject.GetComponentsInChildren<MeshFilter>();
            Material materialToShare = rootObject.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            MeshFilter mf = rootObject.AddComponent<MeshFilter>();
            MeshRenderer mr = rootObject.AddComponent<MeshRenderer>();
            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine);
            mf.sharedMesh = mesh;
            mr.sharedMaterial = materialToShare;
            rootObject.SetActive(true);
        }
    }
}