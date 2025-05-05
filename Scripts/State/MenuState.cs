using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class MenuState : State
    {
        private EventInstance _ambientInstance;
        protected override void Start()
        {
            SoundEntity.Instance.PlayAmbientAttached(SoundEntity.Instance.GetSoundConfig()._mainMenuAmbient);
        }
        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SaveModule.GetData<SlotDataContainer>().SetCurrentSlotData(0);
            }if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SaveModule.GetData<SlotDataContainer>().SetCurrentSlotData(1);
            }if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SaveModule.GetData<SlotDataContainer>().SetCurrentSlotData(2);
            }
        }
        protected override void FixedUpdate()
        {

        }
        protected override void LateUpdate()
        {

        }
        protected override void OnDestroy()
        {

        }

        protected override void HandleStateChange(Status requestStatus)
        {
            switch (requestStatus)
            {
                case Status.Lobby:
                    UpdateStatus(temporary: Status.Loading, targetStatus: Status.Lobby);
                    SceneManager.LoadScene(2);
                    SoundEntity.Instance.PlayAmbientAttached(SoundEntity.Instance.GetSoundConfig()._lobbyAmbient);
                    break;
                case Status.MainMenu:
                    UpdateStatus(temporary: Status.none, targetStatus: Status.MainMenu);
                    ObserverEntity.Instance.SetNextStatus();
                    break;
                case Status.Gameplay:
                    UpdateStatus(temporary: Status.Loading, targetStatus: Status.Gameplay);
                    SceneManager.LoadScene(3);
                    break;
            }
        }
    }
}