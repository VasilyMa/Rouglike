using Client;
using Statement;
using System;
using UnityEngine;

public class InteractiveMainPortalObject : InteractiveLobbyObject
{
    public Action ActionOpenMap;

    protected override void Init()
    {
        base.Init();

        ActionOpenMap = InputButton;
    }

    protected override void InputButton()
    {
        base.InputButton();
        if (!BattleState.Instance.EcsRunHandler.World.GetPool<RequestSwithControllerEvent>().Has(BattleState.Instance.GetEntity("PlayerEntity")))
        { BattleState.Instance.EcsRunHandler.World.GetPool<RequestSwithControllerEvent>().Add(BattleState.Instance.GetEntity("PlayerEntity")).InputActionPreset = InputActionPreset.NonPlayerControl; }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        ActionOpenMap = null;
    }
}
