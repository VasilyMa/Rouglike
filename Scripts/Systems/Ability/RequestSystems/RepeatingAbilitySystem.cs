using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RepeatingAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RepeatingComponent>> _filter;
        readonly EcsPoolInject<RepeatingComponent> _repeatingPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new RepeatingAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var repeatingComp = ref _repeatingPool.Value.Get(entity);
                if(repeatingComp.Delay <=0)
                {
                    for(int i = 0;i < repeatingComp.Count; i++)
                    {
                        InvokeRepetition(repeatingComp);
                    }
                    _repeatingPool.Value.Del(entity);
                }
                else
                {
                    repeatingComp.TimeDelay += Time.deltaTime;
                    if(repeatingComp.TimeDelay >= repeatingComp.Delay)
                    {
                        InvokeRepetition(repeatingComp);
                        repeatingComp.TimeDelay = 0;
                        repeatingComp.Count--;
                    }
                    if (repeatingComp.Count == 0)
                        _repeatingPool.Value.Del(entity);
                }
            }
        }
        public void InvokeRepetition(RepeatingComponent repeatingComp)
        {
            foreach (var component in repeatingComp.FXComponents)
            {
                if (repeatingComp.AbilityEntity.Unpack(_world.Value, out int abilityEntity))
                    if (repeatingComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                        component.Invoke(ownerEntity, abilityEntity, _world.Value, repeatingComp.Charge);//Sorry for the nesting
            }
        }
    }
}