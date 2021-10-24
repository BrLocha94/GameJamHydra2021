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
        PlayerMoney.instance.setListener(OnBalanceChange);
    }

    private void Update()
    {
        GameStates currentState = GameStateMachine.Instance.currentState();

        if (!onIdle && currentState == GameStates.Waiting)
            animator.Play("idle");
        else if (onIdle && currentState == GameStates.Bonus)
            animator.Play("out");
    }

    private void OnBalanceChange(int previous, int balance)
    {
        cashText.text = PlayerMoney.format(balance);
    }
}
