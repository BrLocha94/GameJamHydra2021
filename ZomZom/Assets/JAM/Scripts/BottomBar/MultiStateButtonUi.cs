using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiStateButtonUi : UiButtonBase
{
    [SerializeField]
    private List<CustomStateCallback> customStateCallbacks = new List<CustomStateCallback>();

    public override void OnClick()
    {
        if (!canExecuteCall) return;

        GameStates currentState = GameStateMachine.Instance.currentState();

        CustomStateCallback target = customStateCallbacks.Find(e => e.state == currentState);

        if (target != null)
            StartCoroutine(DelayCustomCallbackRoutine(target.callback, delay));
    }

    private IEnumerator DelayCustomCallbackRoutine(UnityEvent callback, float time)
    {
        yield return new WaitForSeconds(time);

        callback?.Invoke();
    }


    [System.Serializable]
    private class CustomStateCallback
    {
        public GameStates state;
        public UnityEvent callback;
    }
}
