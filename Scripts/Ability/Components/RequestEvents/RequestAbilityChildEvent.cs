using AbilitySystem;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    struct RequestAbilityChildEvent : IAbilityComponent
    {
        public string InvokeAbilityName;
        public EcsPackedEntity CallingEntity;
        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var requestAbilityChildComp = ref world.GetPool<RequestAbilityChildEvent>().Add(world.NewEntity());
            requestAbilityChildComp.CallingEntity = world.PackEntity(ownerEntity);
            requestAbilityChildComp.InvokeAbilityName = InvokeAbilityName;
        }
    }
}
