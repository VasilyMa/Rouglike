using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Client;
using Statement;

public class TeleportSceneMB : MonoBehaviour
{
    [SerializeField] private int loadSceneIndex;
    [SerializeField] private ScreenComponent.Screen _screen;
    private void OnTriggerEnter(Collider other)
    {
        if(UIManagerRitualist.GetUIManager.GetScreenState() != ScreenComponent.Screen.LobbyScreen)
        {
            var world = BattleState.Instance.EcsRunHandler.World;
            int mapEntity = BattleState.Instance.GetEntity("GlobalMapEntity");
            ref var globalMapComp = ref world.GetPool<GlobalMapComponent>().Get(mapEntity);
            if(globalMapComp.PointsArray[globalMapComp.CurrentGlobalMapPointPosition.x,globalMapComp.CurrentGlobalMapPointPosition.y].PointType == PointTypes.Boss && globalMapComp.CurrentBiomIndex >= globalMapComp.BiomCount)
            {
                Statement.State.Instance.SendRequest(new GameRequest(Status.EndGame));
            }
            else if(globalMapComp.PointsArray[globalMapComp.CurrentGlobalMapPointPosition.x,globalMapComp.CurrentGlobalMapPointPosition.y].PointType == PointTypes.Boss && globalMapComp.CurrentBiomIndex < globalMapComp.BiomCount)
            {
                Statement.State.Instance.SendRequest(new GameRequest(Status.EndChapter));
            }
            else
            {
                if (other.name.Contains("MainCharacter"))
                {
                    Statement.State.Instance.SendRequest(new GameRequest(Status.EndLevel));
                }
            }
            //if (!BattleState.Instance.EcsRunHandler.World.GetPool<RequestSwithControllerEvent>().Has(BattleState.Instance.GetEntity("PlayerEntity")))
            //{ BattleState.Instance.EcsRunHandler.World.GetPool<RequestSwithControllerEvent>().Add(BattleState.Instance.GetEntity("PlayerEntity")).InputActionPreset = InputActionPreset.NonPlayerControl; }
        }
    }
    
}
