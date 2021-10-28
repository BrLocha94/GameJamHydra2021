using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    [Header("Animation params")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private string animNameIdleRollup = "outIdleRollup";
    [SerializeField]
    private string animNameRollupFirelink = "outRollupFirelink";
    [SerializeField]
    private string animNameFirelinkIdle = "inFirelinkIdle";
    [Space]
    [SerializeField]
    private Denomination denomination;
    [SerializeField]
    private Text winText;
    [SerializeField]
    private Text cashText;

    private EButtonBarState state = EButtonBarState.Idle;

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

    private void Start()
    {
        //InvokeRepeating(nameof(CheckAnimationTransitions), 0.5f, 0.5f);
    }

    private void CheckAnimationTransitions()
    {
        GameStates currentState = GameStateMachine.Instance.currentState();

        if (state == EButtonBarState.Bonus && currentState == GameStates.Waiting)
        {
            state = EButtonBarState.Idle;
            animator.Play(animNameFirelinkIdle);
        }
        else if (state == EButtonBarState.Idle && currentState == GameStates.RollingReel)
        {
            state = EButtonBarState.Rollup;
            animator.Play(animNameIdleRollup);
        }
        else if (state == EButtonBarState.Rollup && currentState == GameStates.Bonus)
        {
            state = EButtonBarState.Bonus;
            animator.Play(animNameRollupFirelink);
        }
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

public enum EButtonBarState
{
    Null,
    Idle,
    Rollup,
    Bonus
}