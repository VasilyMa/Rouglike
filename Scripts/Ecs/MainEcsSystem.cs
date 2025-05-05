using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    [System.Serializable]
    public abstract class MainEcsSystem : IEcsRunSystem, IEcsInitSystem
    {
        public EcsSystemData EcsSystemData;

        public abstract MainEcsSystem Clone();
        public virtual void Init(IEcsSystems systems) { }// => 
        public virtual void Run(IEcsSystems systems) { }// => 
    }

    public struct EcsSystemData
    {

    }
}