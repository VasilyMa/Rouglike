namespace Client {
    struct EnemyComponent {
        public EnemyTypes EnemyType;
        public float AgroDistance;
        public float AvoidDistance;

    }
}
public enum EnemyTypes{
    Melee, Range
}