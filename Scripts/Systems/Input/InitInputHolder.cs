using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class InitInputHolder : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<PlayerComponent>> _filter = default;
        readonly EcsPoolInject<InputHolder> _pool = default;

        public override MainEcsSystem Clone()
        {
            return new InitInputHolder();
        }

        public override void Init (IEcsSystems systems) 
        {
            foreach (var player in _filter.Value)
            {
                _pool.Value.Add(player);
            }
        }
    }
}