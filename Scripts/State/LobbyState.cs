using System;
using System.Collections.Generic;

using UniRx;

using Unity.AI.Navigation;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class LobbyState : State
    {
        public NavMeshSurface navMeshSurface;

        bool isPause;

        protected override void Awake()
        {
            base.Awake();

            this.navMeshSurface = GameObject.FindObjectOfType<NavMeshSurface>();
        }

        protected override void Start()
        {
            base.Start();


        }

        void InvokePause()
        {
            if (isPause)
            {
                SendRequest(new GameRequest(Status.Gameplay));
            }
            else
            {
                SendRequest(new GameRequest(Status.Overlay));
            }
        }
    }
}
