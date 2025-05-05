using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class AttackLineSystem : IEcsRunSystem
    {

        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<EnableAttackLineEvent>> _enableFilter = default;
        readonly EcsFilterInject<Inc<DisableAttackLineEvent>> _disableFilter = default;
        readonly EcsPoolInject<EnableAttackLineEvent> _Pool = default;
        readonly EcsPoolInject<DisableAttackLineEvent> _DPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;
        public float minLength = 4f;
        public float maxLength = 5f;
        public float tweenDuration = 1f;
        public DG.Tweening.Sequence sequence;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _disableFilter.Value)
            {
                ref var attackComp = ref _attackPool.Value.Get(entity);
                if (attackComp.AttackLine != null)
                {
                    
                    GameObject.Destroy(attackComp.AttackLine);
                    sequence.Kill();
                }
                _DPool.Value.Del(entity);
            }
            foreach (var entity in _enableFilter.Value)
            {
                ref var enableComp = ref _enableFilter.Pools.Inc1.Get(entity);
                ref var attackComp = ref _attackPool.Value.Get(entity);
                ref var transfromComp = ref _transfromPool.Value.Get(entity);
                minLength = enableComp.MinDistance;
                maxLength = enableComp.MaxDistance;
                tweenDuration = enableComp.Duration;


                

                

                var lineRegion = attackComp.AttackLine.GetComponent<LineRegion>();
                lineRegion.Length = 0;
                lineRegion.Length = minLength;
                StartTween(lineRegion);
                _Pool.Value.Del(entity);
            }
            
        }
        private void StartTween(LineRegion lineRegion)
        {
            sequence = DOTween.Sequence()
            .Append(DOTween.To(() => lineRegion.Length, x => lineRegion.Length = x, maxLength, tweenDuration))
            .SetEase(Ease.Linear)
            ;
        }
    }
}