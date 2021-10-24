using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class UiButtonBase : MonoBehaviour
{
    [SerializeField]
    protected float delay = 0f;

    protected bool canExecuteCall = true;

    public abstract void OnClick();

    protected virtual void OnCLickDelayedCall()
    {
        canExecuteCall = true;
    }
}
