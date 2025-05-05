using UniRx;

using UnityEngine;

namespace Client {
    struct BossObserverComponent
    {
        public ReactiveProperty<HealthValue> BossHealthValue;
        public ReactiveProperty<ToughnessValue> BossToughnessbarValue;
        public ReactiveProperty<BossStageValue> BossStageValue;
        public ReactiveProperty<Vector2> BossPositionBarValue;
    }
}