using Leopotam.EcsLite;

using UnityEngine;

namespace UI
{
    public abstract class CanvasMB : MonoBehaviour
    {
        protected EcsWorld _world;
        protected Canvas _canvas;
        public virtual void Init(EcsWorld world)
        {
            _world = world;
            _canvas = GetComponent<Canvas>();
        }

        public abstract void OnOpen();
        public abstract void OnClose();
    }
}