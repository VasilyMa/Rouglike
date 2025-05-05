using System.Collections.Generic;

using Leopotam.EcsLite;

using UI;

namespace Client 
{
    struct InterfaceComponent 
    {
        public UIManager UIManager;

        public void Init(UIManager uiManager)
        {
            UIManager = uiManager;

            //UIManager.BaseInit();
        }

    }
}