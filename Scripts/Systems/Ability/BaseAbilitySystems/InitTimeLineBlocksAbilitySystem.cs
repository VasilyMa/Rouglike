using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using AbilitySystem;

namespace Client {
    sealed class InitTimeLineBlocksAbilitySystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<InitAbilityEvent, AbilityComponent>> _filter = default;
        readonly EcsPoolInject<TimeLineBlocksListAbilityComponent> _timeLineBlocks = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        public override MainEcsSystem Clone()
        {
            return new InitTimeLineBlocksAbilitySystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                ref var fxBlockComp = ref _timeLineBlocks.Value.Add(entity);
                fxBlockComp.BlockList = new System.Collections.Generic.List<TimeLineBlock>();
                TimeLineBlock[] arrayBlock = new TimeLineBlock[abilityComp.Ability.SourceAbility.TimeLineBlocks.Count];
                for (int i = 0; i < arrayBlock.Length; i++)
                {
                    arrayBlock[i] = abilityComp.Ability.SourceAbility.TimeLineBlocks[i];
                }
                arrayBlock = BubbleSort(arrayBlock);

                for (int i = 0; i < arrayBlock.Length; i++)
                {
                    fxBlockComp.BlockList.Add(arrayBlock[i]);
                }
            }
        }
        void Swap(ref TimeLineBlock e1, ref TimeLineBlock e2)
        {
            var temp = e1;
            e1 = e2;
            e2 = temp;
        }
        TimeLineBlock[] BubbleSort(TimeLineBlock[] array)
        {
            var len = array.Length;
            for (var i = 1; i < len; i++)
            {
                for (var j = 0; j < len - i; j++)
                {
                    if (array[j].Timer > array[j + 1].Timer)
                    {
                        Swap(ref array[j], ref array[j + 1]);
                    }
                }
            }
            return array;
        }

    }
}