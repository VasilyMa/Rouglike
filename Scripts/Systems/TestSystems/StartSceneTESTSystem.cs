using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.SceneManagement;

namespace Client {
    sealed class StartSceneTESTSystem : MainEcsSystem
    {
        readonly EcsPoolInject<TestGameplayComponent> _testGamePlay;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new StartSceneTESTSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name == "TestScene 1" || currentScene.name == "TestScene")
            {
                _testGamePlay.Value.Add(_world.Value.NewEntity());
            }
        }
    }
}