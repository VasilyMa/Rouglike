using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.AI;
using Client;
using Unity.AI.Navigation;
using UniRx;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.VFX;

public class GameState
{
    // public CompositeDisposable MainDisposable;// = new CompositeDisposable();
    // public static GameState Instance;
    // public EcsWorld World;
    // internal int PoolEntity;
    // internal int InterfaceEntity;
    // internal int InputEntity;
    // internal int PlayerEntity;
    // internal int GlobalMapEntity;
    // internal int LocalMapEntity;
    // internal int RelicPoolEntity;
    // public int CurrentIndexWave = 0;
    // public int EntityObstacles;
    // public NavMeshSurface navMeshSurface;
    // public int SlowEntity;
    // private static float _defaultFixedDT = 0.02f;
    // private static float _defaultVFXDT = 0.01666667f;
    // private PoolModule poolModule = new PoolModule();
    // public GameState()
    // {
    //     Instance = this;
    //     // currencies = Data.currenciesList; //TODO Vasya
    // }

    // public static void ResetTime()
    // {
    //     Time.timeScale = 1f;
    //     Time.fixedDeltaTime = _defaultFixedDT;
    //     VFXManager.fixedTimeStep = _defaultVFXDT;
    // }
    // public void InitGameplayState()
    // {
    //     this.navMeshSurface = GameObject.FindObjectOfType<NavMeshSurface>();
    //     MainDisposable = new CompositeDisposable();
    // }
    // public void EnableNavMeshAgent(NavMeshAgent agent, bool enable)
    // {
    //     if(agent.isOnNavMesh) agent.isStopped = !enable;
    //     agent.enabled = enable;
    // }


    // public void UpdateNavMesh()
    // {
    //     //navMeshSurface.RemoveData(navMeshSurface.navMeshData);
    //     navMeshSurface.BuildNavMesh();
    //     navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
    // }
    // public void UpdateGlobalMap(Vector2Int position)
    // {
    //     ref var globalMapComp = ref World.GetPool<GlobalMapComponent>().Get(GlobalMapEntity);
    //     GlobalMapPoint currentPoint = globalMapComp.PointsArray[position.x, position.y];
        
    //     GlobalMapPoint lastPoint = globalMapComp.PointsArray[globalMapComp.CurrentGlobalMapPointPosition.x, globalMapComp.CurrentGlobalMapPointPosition.y];
    //     lastPoint.PointState = PointStates.Completed;
    //     foreach(var link in lastPoint.ExitList)
    //     {
    //         if(link != currentPoint)
    //         {
    //             link.PointState = PointStates.Closed;
    //         }
    //     }
    //     currentPoint.PointState = PointStates.ThisRoom;
    //     globalMapComp.CurrentGlobalMapPointPosition = position;
    //     foreach(var link in currentPoint.ExitList)
    //     {
    //         link.PointState = PointStates.Open;
    //     }

    // }
}
