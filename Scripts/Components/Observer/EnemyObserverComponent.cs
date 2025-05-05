using UniRx;

using UnityEngine;

namespace Client {
    struct EnemyObserverComponent 
    {
        public EnemyObserver Observer;
        public ReactiveProperty<HealthValue> EnemyHealthbarValue;
        public ReactiveProperty<ToughnessValue> EnemyToughnessbarValue;
        public ReactiveProperty<Vector2> EnemyPositionBarValue;
    }
}