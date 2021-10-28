using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZZ_GameManager : MonoBehaviour
{
    [SerializeField] private BonusWheelController bonusWheelController;
    [SerializeField] private DirectorPlayer gridPlayer;
    [SerializeField] private ZZ_Grid_Controller gridController;
    [SerializeField] private ZZ_FireLinkController fireLinkController;

    GameStateMachine stateMachine => GameStateMachine.Instance;

    private bool isBonus;
    private GridPlaySetup playSetup;

    private void Start()
    {
        BalanceManager.AddBalance(50000);
        bonusWheelController.OnWheelsEndedEvent += BonusWheelController_OnWheelsEndedEvent;
    }

    private void BonusWheelController_OnWheelsEndedEvent()
    {
        gridPlayer.Play("FireLinkIn", wrapMode: UnityEngine.Playables.DirectorWrapMode.Hold, OnEnd: () => StartCoroutine(FireLinkRoutine()));
    }

    IEnumerator FireLinkRoutine()
    {
        bonusWheelController.RevertChangedSprites();
        GameStateMachine.Instance.ChangeState(GameStates.Bonus);
        yield return fireLinkController.StartFireLink();
        gridPlayer.Play("FireLinkOut", wrapMode: UnityEngine.Playables.DirectorWrapMode.None, OnEnd: () => 
        {
            stateMachine.ChangeState(GameStates.Waiting);
           // BonusWheelController_OnWheelsEndedEvent();
        });
    }

    public void OnRevealOutFinished()
    {
        Debug.Log("OnRevealOutFinished");

        //isBonus = true;

        if (isBonus)
        {
            gridPlayer.Play("BonusComemoration", wrapMode: UnityEngine.Playables.DirectorWrapMode.Hold, OnEnd: () =>
            {
                 bonusWheelController.Begin(new List<Vector2>());
            });
        }
        else
        {
            stateMachine.ChangeState(GameStates.Waiting);
        }
    }

    public void Play()
    {
        if (stateMachine.currentState() == GameStates.Waiting)
        {
            if (!BalanceManager.ExecutePlay()) return;

            playSetup = CreateGridPlaySetup();

            gridController.Play(playSetup);
            
            stateMachine.ChangeState(GameStates.RollingReel);
        }
    }

    private GridPlaySetup CreateGridPlaySetup()
    {
        GridPlaySetup nextPlaySetup = new GridPlaySetup()
        {
            reelsEntrys = new List<List<ESymbol>>()
                 {
                   new List<ESymbol>()
                    {
                       GetRandomSymbol(),
                       GetRandomSymbol(),
                       GetRandomSymbol(),
                    },
                    new List<ESymbol>()
                    {
                       GetRandomSymbol(),
                       GetRandomSymbol(),
                       GetRandomSymbol(),
                    },
                    new List<ESymbol>()
                    {
                       GetRandomSymbol(),
                       GetRandomSymbol(),
                       GetRandomSymbol(),
                    },
                 },
        };

        return nextPlaySetup;
    }

    private ESymbol GetRandomSymbol()
    {
        return (ESymbol)Random.Range(0, 6);
    }
}

public struct GridPlaySetup
{
    public List<List<ESymbol>> reelsEntrys;
}
