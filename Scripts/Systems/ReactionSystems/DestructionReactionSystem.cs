using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DestructionReactionSystem : IEcsRunSystem {
        private readonly EcsFilterInject<Inc<ReactionEvent>> _filter = default;
        private readonly EcsPoolInject<ReactionEvent> _reactionEvent = default;
        private readonly EcsPoolInject<KnockbackComponent> _knockBackPool = default;
        private readonly EcsPoolInject<StunComponent> _stunPool = default;
        private readonly EcsPoolInject<ReactionDataComponent> _reactionDataComponent = default;
        public void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                if (_stunPool.Value.Has(entity) && _knockBackPool.Value.Has(entity)) continue;
                _reactionEvent.Value.Del(entity);
                _reactionDataComponent.Value.Del(entity);
            }
            //At the moment when the StunComponent and KnockBackComponent will be deleted, you need to do:
            //if(_reactionEvent.Value.Has(entity))
            // { ref var reactionComponent = ref _reactionEvent.Value.Get(entity)
            // reactionComponent.IsAnimationInvoke = true;
            // }
            ////           ________
            //            /    |   \
            //           /     |    \ 
            //          / _____|_____\
            //          |            |
            //          |            |
            //          |            |                   It's a rocket and what are you thinking about?
            //          |            |
            //          |            |
            //          |            |
            //          |            | 
            //   _______|____    ____|______
            //  |           |   |           |
            //  |           |   |           |
            //  |           |   |           |
            //  |           |   |           |       
            //  |___________|   |___________|
        }
    }
}