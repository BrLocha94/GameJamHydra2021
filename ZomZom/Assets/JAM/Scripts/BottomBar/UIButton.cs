using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : UiButtonBase
{
    [SerializeField]
    private UnityEvent onClickCallback;
    [Space]
    [SerializeField]
    private List<GameStates> allowedGameStates = new List<GameStates>();

    public override void OnClick()
    {
        if (!canExecuteCall) return;

        GameStates currentState = GameStateMachine.Instance.currentState();

        if (allowedGameStates.Contains(currentState))
        {
            canExecuteCall = false;
            Invoke(nameof(OnCLickDelayedCall), delay);
        }
    }

    protected override void OnCLickDelayedCall()
    {
        base.OnCLickDelayedCall();
        onClickCallback?.Invoke();
    }
}
