using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Denomination denomination;
    [SerializeField]
    private Text winText;
    [SerializeField]
    private Text cashText;

    private bool onIdle = false;

    public void EnterIdleAnimationEvent() => onIdle = true;
    public void EnterOutAnimationEvent() => onIdle = false;

    private void Awake()
    {
        OnWinChange(0);
        OnBalanceChange(0);
    }

    private void OnEnable()
    {
        BalanceManager.onBalanceChange += OnBalanceChange;
        BalanceManager.onWinChange += OnWinChange;
    }

    private void OnDisable()
    {
        BalanceManager.onBalanceChange -= OnBalanceChange;
        BalanceManager.onWinChange -= OnWinChange;
    }

    private void Update()
    {
        GameStates currentState = GameStateMachine.Instance.currentState();

        if (!onIdle && currentState == GameStates.Waiting)
            animator.Play("idle");
        else if (onIdle && currentState == GameStates.Bonus)
            animator.Play("out");
    }

    private void OnWinChange(int newWin)
    {
        winText.text = newWin.FormatStringCashNoCents();
    }

    private void OnBalanceChange(int newBalance)
    {
        cashText.text = newBalance.FormatStringCashNoCents();
    }
}
