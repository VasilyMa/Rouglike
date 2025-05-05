using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using UI;

namespace Client 
{
    sealed class SetNewCurrentAbility : IEcsRunSystem
    {
        /*readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<SetNewCurrentAbilityEvent>, Exc<PreCastAbilityEvent>> _filter = default;
        readonly EcsPoolInject<SetNewCurrentAbilityEvent> _filterPool = default;
        readonly EcsPoolInject<AbilityContainer> _abilityContainerPool = default;
        readonly EcsPoolInject<StaminaComponent> _staminaComponentPool = default;
        readonly EcsPoolInject<PreCastAbilityEvent> _precastEventPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;*/
        //readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        public void Run(IEcsSystems systems)
        {
            /*foreach(var entity in _filter.Value)
            {
                ref var abilityContainerComp = ref _abilityContainerPool.Value.Get(entity);
                ref var filterComp = ref _filterPool.Value.Get(entity);
                float remainingStamina = 1000;
                bool isCast = true;

                if (_staminaComponentPool.Value.Has(entity)) remainingStamina = _staminaComponentPool.Value.Get(entity).GetValue();


                if (_playerPool.Value.Has(entity))
                {

                    //var canvas = _interfacePool.Value.Get(_state.Value.InterfaceEntity).GetCanvas<CombatCanvas>();
                    var InGameScreen = UIManagerRitualist.GetUIManager.SusApp.InGameScreen;

                    if (abilityContainerComp.CheckCooldown(filterComp.AbilityType))
                    {
                        //canvas.InfoMessagePanel.SendMessage(new PlayerMessage("Cooldown", MessageInfoType.cooldown, abilityCooldown: abilityContainerComp.GetAbility(filterComp.AbilityType)));

                        //canvas.GetAbilitySlot(abilityContainerComp.GetAbility(filterComp.AbilityType)).DeniedAbilityCast();
                        InGameScreen.GetAbilitySlot(abilityContainerComp.GetAbility(filterComp.AbilityType)).DeniedAbilityCast();
                        isCast = false;
                    }

                    if (!abilityContainerComp.IsEnoughStamina(filterComp.AbilityType, remainingStamina))
                    {
                        //canvas.InfoMessagePanel.SendMessage(playerMessage: new PlayerMessage("Not enough <b>stamina</b>", messageType: MessageInfoType.title));

                        //canvas.GetAbilitySlot(abilityContainerComp.GetAbility(filterComp.AbilityType)).DeniedAbilityCast();
                        InGameScreen.GetAbilitySlot(abilityContainerComp.GetAbility(filterComp.AbilityType)).DeniedAbilityCast();
                        isCast = false;
                    }

                    if (isCast)
                    {
                        abilityContainerComp.SetNewCurrentAbility(filterComp.AbilityType);
                        //_precastEventPool.Value.Add(entity);
                    }

                    continue;
                }

                isCast = abilityContainerComp.CheckCooldown(filterComp.AbilityType);

                isCast = abilityContainerComp.IsEnoughStamina(filterComp.AbilityType, remainingStamina);

                if (isCast)
                {
                    abilityContainerComp.SetNewCurrentAbility(filterComp.AbilityType);
                    //_precastEventPool.Value.Add(entity);
                }*/
        }
    }
}