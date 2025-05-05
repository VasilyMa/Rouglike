using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Sirenix.Utilities;
using UnityEngine;
namespace Client {
    sealed class UpdateSplineInstancesSystem : MainEcsSystem 
    { 
        readonly EcsFilterInject<Inc<UpdateSplineInstances>> _updateFilter = default;
        readonly EcsPoolInject<UpdateSplineInstances> _instancesPool = default;
        private int frameCounter = 25;
        public override MainEcsSystem Clone()
        {
            return new UpdateSplineInstancesSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach (int entity in _updateFilter.Value)
            {
                frameCounter--;
                if (frameCounter > 0) continue;
                var instances = Object.FindObjectsOfType<MeshCreator>();
                foreach (MeshCreator instance in instances)
                {
                    if (instance.DrawerInstance.GenerateProps)
                    {
                        instance.DrawerInstance.UpdateInstance();
                    }
                    else
                    {
                        instance.DrawerInstance.DisableSplineInstantiate();
                    }
                }
                _instancesPool.Value.Del(entity);
                var findObj = Object.FindObjectsOfType<StaticBatchingUtilityMB>();
                findObj.ForEach(x => x.StaticBatchingUtilityFunc());
            }
        }
    }
}