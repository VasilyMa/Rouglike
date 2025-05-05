using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Statement;

namespace Client
{
    sealed class InitLocalMapSystem : MainEcsSystem
    {

        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<LocalMapComponent> _localMapPool = default;
        readonly EcsPoolInject<GenerateLocalMapSelfRequest> _generateLocalMapPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitLocalMapSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //ничего не генерим если это не майнсцена
            var state = BattleState.Instance;

            if (state.IsMainScene)
            {
                int entity = _world.Value.NewEntity();

                state.RegisterNewEntity("LocalMapEntity", entity);
                ref var localMapComp = ref _localMapPool.Value.Add(entity);
                _generateLocalMapPool.Value.Add(entity);
            }
        }
    }
}