// using Leopotam.EcsLite;
// using UnityEngine;
// using Leopotam.EcsLite.Di;
// using System.Collections.Generic;

// namespace Client {
//     sealed class CreateCurrentLocalMapPoint : IEcsRunSystem {
//         readonly EcsSharedInject<GameState> _state = default;
//         readonly EcsFilterInject<Inc<LocalMapComponent, CreateCurrentLocalMapPointEvent>> _filter = default;
//         readonly EcsPoolInject<GlobalMapComponent> _globalMapPool = default;
//         readonly EcsPoolInject<LocalMapComponent> _localMapPool = default;
//         readonly EcsPoolInject<CreateCurrentLocalMapPointEvent> _createPool = default;
//         LocalMapGenerationConfig localMapConfig = null;
//         public void Run (IEcsSystems systems) {
//             foreach(var entity in _filter.Value)
//             {
//                 ref var localMapComp = ref _localMapPool.Value.Get(entity);
//                 ref var createComp = ref _createPool.Value.Get(entity);
//                 localMapConfig = _state.Value.GetConfig<LocalMapGenerationConfig>();

//                 //взяли подходящую по типу случайную комнату из конфиги
//                 GameObject pivot = null;
//                 if(localMapComp.CurrentLocalMapPoint.RoomType == RoomTypes.Start)
//                 {
//                     pivot = SpawnRoom(localMapComp.CurrentLocalMapPoint, ref localMapComp, createComp.CreatePosition);
//                 }
//                 else
//                 {
//                     //todo найти нужную комнату в Куррент
//                     pivot = localMapComp.CurrentLocalMapPoint.PivotGameObject;
//                     pivot.transform.SetParent(null);
//                     pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
//                 }

//                 _state.Value.CurrentRoom = pivot.GetComponent<RoomMB>();


                
//                 #region corridorSpawn
//                 //todo create links
//                 localMapComp.CurrentLocalMapPoint.ExitTransforms = new Dictionary<Vector2Int, Transform>();
//                 foreach(var link in localMapComp.CurrentLocalMapPoint.ExitList)
//                 {
//                     //todo SpawnRoom, взять комнату на основании линка!!!
//                     Vector2Int linkDirection = link.Position - localMapComp.CurrentLocalMapPoint.Position;
//                     Vector3 direction = new Vector3(linkDirection.x, 0, linkDirection.y).normalized;
//                     Transform linkStartPointTransform = null;
//                     foreach (var item in localMapComp.CurrentLocalMapPoint.RoomExitList) //todo вынести в отдельную функцию
//                     {
//                         if(item.forward.normalized * -1 == direction)
//                         {
//                             linkStartPointTransform = item;
//                         }
//                     }
//                     var exitMB = linkStartPointTransform.GetComponent<ExitPointMB>();
//                     exitMB.IsExit = true;
//                     exitMB.SetActiveFalseWall();
                    
//                     exitMB._collider.center = new Vector3(0, 0, -9);
//                     exitMB.LockExitGameObject.transform.localPosition = new Vector3(0, 0, 0);



//                     var dock = GameObject.Instantiate(localMapConfig.GetDock());
//                     dock.transform.SetParent(linkStartPointTransform);
//                     dock.transform.localPosition = Vector3.zero;

//                     dock.transform.localRotation = Quaternion.Euler(0, 180, 0);

//                     var corridor = GameObject.Instantiate(localMapConfig.GetCorridor());
//                     corridor.transform.SetParent(dock.transform);
//                     corridor.transform.localPosition = new Vector3(0,0,10);
//                     corridor.transform.localRotation = dock.transform.localRotation;

//                     var exitDock = GameObject.Instantiate(localMapConfig.GetDock());
//                     exitDock.transform.SetParent(corridor.transform);
//                     exitDock.transform.localPosition = new Vector3(0, 0, -10);
//                     exitDock.transform.localRotation = Quaternion.Euler(0, 0, 0);

//                     localMapComp.CurrentLocalMapPoint.ExitTransforms.Add(linkDirection, exitDock.transform);

//                     var newPivot = SpawnRoom(link, ref localMapComp, exitDock.transform.position);
//                     //newPivot.transform.rotation = Quaternion.Euler(0, 45, 0);
//                     newPivot.transform.SetParent(exitDock.transform);
//                     localMapComp.PointsArray[localMapComp.CurrentLocalMapPoint.Position.x + linkDirection.x, localMapComp.CurrentLocalMapPoint.Position.y + linkDirection.y].PivotGameObject = newPivot;

//                     //todo RoomSpawn
//                 }
//                 if(localMapComp.CurrentLocalMapPoint.RoomType == RoomTypes.Start) //создание входа в данж
//                 {
//                     var enter = GameObject.Instantiate(localMapConfig.EnterDungeon);
//                     Vector2Int intDirection = localMapComp.CurrentLocalMapPoint.Position - localMapComp.LastPosition;
                    
//                     Vector3 direction = new Vector3(intDirection.x, 0, intDirection.y);
//                     Transform connectTransform = null;
//                     foreach (var item in localMapComp.CurrentLocalMapPoint.RoomExitList) //todo вынести в отдельную функцию
//                     {
//                         if(item.forward.normalized == direction)
//                         {
//                             connectTransform = item;
//                         }
//                     }
//                     enter.transform.SetParent(connectTransform);
//                     enter.transform.localPosition = Vector3.zero;
//                     enter.transform.localRotation = Quaternion.Euler(0, 0, 0);

//                     localMapComp.StartTransform = enter.transform;
//                 }
//                 if(localMapComp.CurrentLocalMapPoint.RoomType == RoomTypes.End)
//                 {
//                     //todo родить выход из комнаты с босом
//                     var exit = GameObject.Instantiate(localMapConfig.ExitDungeon);
//                     Vector2Int intDirection = new Vector2Int(localMapComp.CurrentLocalMapPoint.Position.x, localMapComp.CurrentLocalMapPoint.Position.y + 1) - localMapComp.CurrentLocalMapPoint.Position;

//                     Vector3 direction = new Vector3(intDirection.x, 0, intDirection.y);
//                     Transform connectTransform = null;
//                     foreach (var item in localMapComp.CurrentLocalMapPoint.RoomExitList) //todo вынести в отдельную функцию
//                     {
//                         if(item.forward.normalized * -1 == direction)
//                         {
//                             connectTransform = item;
//                         }
//                     }
//                     var mb = connectTransform.GetComponent<ExitPointMB>();
//                     mb.IsExit = true;
//                     mb._collider.center = new Vector3(0, 0, -9);
//                     exit.transform.SetParent(connectTransform);
//                     exit.transform.localPosition = Vector3.zero;
//                     exit.transform.localRotation = Quaternion.Euler(0, 180, 0);

//                     //localMapComp.StartTransform = exit.transform;

//                 }

//                 pivot.transform.rotation = Quaternion.Euler(0, 45, 0);

//                 #endregion
//                 //todo родить по экситам коридоры и предбанники, засунуть их в пивот ГО
//                 //todo к первой и последей точке родить Гига вход и гига выход
//                 pivot.transform.SetParent(_state.Value.navMeshSurface.transform);
//                 _state.Value.UpdateNavMesh();
//             }
//         }
        
//     }
// }