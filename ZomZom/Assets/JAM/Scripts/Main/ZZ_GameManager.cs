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

    private PlayManager.Ticket currentTicket = null;
    private bool isBonus;
    private GridPlaySetup playSetup;
    private int playTurn = 0;

    private void Awake()
    {
        PlayManager.instance.initialize("math");
    }

    private void Start()
    {
        PlayerMoney.instance.addToBalance(50000);
        bonusWheelController.OnWheelsEndedEvent += BonusWheelController_OnWheelsEndedEvent;
       // BonusWheelController_OnWheelsEndedEvent();
    }


    private void BonusWheelController_OnWheelsEndedEvent()
    {
        gridPlayer.Play("FireLinkIn", wrapMode: UnityEngine.Playables.DirectorWrapMode.Hold, OnEnd: () => StartCoroutine(FireLinkRoutine()));
    }

    IEnumerator FireLinkRoutine()
    {
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
        isBonus = true;

        if (isBonus)
        {
            gridPlayer.Play("BonusComemoration", wrapMode: UnityEngine.Playables.DirectorWrapMode.Hold, OnEnd: () =>
            {
                 bonusWheelController.Begin(/*SymbolTranslate.TranslateToSymbol(currentTicket.symbols)*/ new List<ESymbol>(){ ESymbol.Bonus, ESymbol.Bonus, ESymbol.Bonus}, playSetup.fromToReelAnimation);
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
            currentTicket = PlayManager.instance.play(0.5);
            Debug.Log("Play ticket: " + currentTicket);

            gridController.reels[0].Offset = 0;
            gridController.reels[1].Offset = 0;
            gridController.reels[2].Offset = 0;
            gridController.ResetFromToReelsOffset();

            playSetup = CreateGridPlaySetup();

            if (playTurn == 0)
                playTurn = 1;
            else
                playTurn = 0;

            gridController.PrepareNextPlay(playSetup);
            gridPlayer.Play("SymbolsIn", wrapMode: UnityEngine.Playables.DirectorWrapMode.Hold);
            stateMachine.ChangeState(GameStates.RollingReel);
        }
    }

    private GridPlaySetup CreateGridPlaySetup()
    {
        int add = playTurn == 0 ? 3 : 0;
        int index1 = 0 + add;
        int index2 = 1 + add;
        int index3 = 2 + add;

        GridPlaySetup nextPlaySetup = new GridPlaySetup()
        {
            reelsEntrys = new List<List<(ESymbol symbol, int index)>>()
                 {
                    new List<(ESymbol symbol, int index)>()
                    {
                       (GetRandomSymbol(), index1),
                       (GetRandomSymbol(), index2),
                       (GetRandomSymbol(), 2)
                    },
                    new List<(ESymbol symbol, int index)>()
                    {
                       (GetRandomSymbol(), index1),
                       (GetRandomSymbol(), index2),
                       (GetRandomSymbol(), index3)
                    },
                    new List<(ESymbol symbol, int index)>()
                    {
                       (GetRandomSymbol(), index1),
                       (GetRandomSymbol(), index2),
                       (GetRandomSymbol(), index3)
                    }
                 },
            fromToReelAnimation = new List<Vector2>()
                 {
                     new Vector2(gridController.reels[0].Offset,gridController.reels[0].Offset+3),
                     new Vector2(gridController.reels[1].Offset,gridController.reels[1].Offset+3),
                     new Vector2(gridController.reels[2].Offset,gridController.reels[2].Offset+3),
                 }
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
    public List<List<(ESymbol symbol, int index)>> reelsEntrys;
    public List<Vector2> fromToReelAnimation;
}
