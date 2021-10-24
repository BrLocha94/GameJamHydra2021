using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZZ_GameManager : MonoBehaviour
{
    [SerializeField] private BonusWheelController bonusWheelController;
    [SerializeField] private DirectorPlayer gridPlayer;
    [SerializeField] private ZZ_Grid_Controller gridController;

    private bool isBonus;

    private void Awake()
    {
        
    }

    GameStateMachine stateMachine=> GameStateMachine.Instance;
        
    public void OnRevealOutFinished()
    {
        Debug.Log("OnRevealOutFinished");
        stateMachine.ChangeState(GameStates.RollingReel);

        isBonus = true;

        if(isBonus)
        {
            gridPlayer.Stop();
            bonusWheelController.Begin();
        }
    }
    public void Play()
    {
        if(stateMachine.currentState() == GameStates.Waiting)
        {
            GridPlaySetup nextPlaySetup = new GridPlaySetup() 
            {
                 reelsEntrys = new List<List<(ESymbol symbol, int index)>>()
                 {
                    new List<(ESymbol symbol, int index)>()
                    {
                       (ESymbol.P1, 3),
                       (ESymbol.P2, 4),
                       (ESymbol.Bonus, 5)
                    },
                    new List<(ESymbol symbol, int index)>()
                    {
                       (ESymbol.P5, 3),
                       (ESymbol.Bonus, 4),
                       (ESymbol.P5, 5)
                    },
                    new List<(ESymbol symbol, int index)>()
                    {
                       (ESymbol.Bonus, 3),
                       (ESymbol.P3, 4),
                       (ESymbol.P6, 5)
                    }
                 },
                  fromToReelAnimation = new List<Vector2>()
                  {
                      new Vector2(gridController.reels[0].Offset,gridController.reels[0].Offset+3),
                      new Vector2(gridController.reels[1].Offset,gridController.reels[1].Offset+3),
                      new Vector2(gridController.reels[2].Offset,gridController.reels[2].Offset+3),
                  }
            };


            gridController.PrepareNextPlay(nextPlaySetup);
            gridPlayer.Play("SymbolsIn", wrapMode: UnityEngine.Playables.DirectorWrapMode.Hold);
            stateMachine.ChangeState(GameStates.RollingReel);
        }
    }
}

public struct GridPlaySetup
{
    public List<List<(ESymbol symbol, int index)>> reelsEntrys;
    public List<Vector2> fromToReelAnimation;
}