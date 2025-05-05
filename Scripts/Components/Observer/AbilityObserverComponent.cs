using UniRx;

namespace Client 
{
    struct AbilityObserverComponent 
    {
        public AbilityObserver AbilityObserver;
        public ReactiveProperty<CooldownValue> CooldownValue;
        public ReactiveProperty<ChargeValue> ChargeValue;
    }
}