using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour
{
    [SerializeField]
    private List<GameStates> allowedGameStates = new List<GameStates>();
    [Space]
    [SerializeField]
    private UnityEvent onClickCallback;

    public void OnClick()
    {
        GameStates currentState = GameStateMachine.Instance.currentState();

        if (allowedGameStates.Contains(currentState))
        {
            onClickCallback?.Invoke();
        }
    }
}
