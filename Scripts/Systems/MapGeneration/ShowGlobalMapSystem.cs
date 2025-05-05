using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client {
    sealed class ShowGlobalMapSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ShowGlobalMapEvent>> _filter = default;
        readonly EcsPoolInject<ShowGlobalMapEvent> _showPool = default;
        private ScreenComponent.Screen _screen;
        public override MainEcsSystem Clone()
        {
            return new ShowGlobalMapSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                var world = BattleState.Instance.EcsRunHandler.World;
                int mapEntity = BattleState.Instance.GetEntity("GlobalMapEntity");
                ref var globalMapComp = ref world.GetPool<GlobalMapComponent>().Get(mapEntity);
                ref var showComp = ref _showPool.Value.Get(entity);
                UIMapData uiMapData = new UIMapData();
                uiMapData.MapData = GenerateCompleteGlobalMapSystem.ConvertGlobalMapToUIMapData(globalMapComp.PointsArray);
                UIManagerRitualist.GetUIManager.UIMapManagerGlobal.UIMapDataVisualise(uiMapData.MapData);
                UIManagerRitualist.GetUIManager.ChangeOverlayState(OverlayComponent.Overlay.MapLayer);
                //UIManagerRitualist.GetUIManager.UIMapManagerGlobal.ShowMapScreen();
                //UIManagerRitualist.GetUIManager.ChangeScreenState(showComp.screen);
                UIManagerRitualist.GetUIManager.UIInGameInterfaceComponents.ClearWorldspaceLayer();
                PlayerEntity.Instance.Map.ProcessUpdataData();
                //UIManagerRitualist.GetUIManager.UILobbyScreenComponents.ClearLobbyWorldspaceLayer();
            }
        }
    }
}