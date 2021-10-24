using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BonusWheelController : MonoBehaviour
{
    [SerializeField] BonusWheel[] bonusWheels;
    [SerializeField] ZZ_Grid_Slot[] targetSlots;
    [SerializeField] SymbolsDataAsset symbolsDataAsset;
    public event Action OnWheelsEndedEvent;

    private int currentWheel = 0;
    private bool stopping = false;

    public void Begin(List<ESymbol> symbolsList, List<Vector2> fromToReelAnimationOffset)
    {
        currentWheel = 0;
        stopping = false;

        for (int i = 0; i < bonusWheels.Length; i++)
        {
            bonusWheels[i].Begin(fromToReelAnimationOffset[i].y);
            SymbolData symbolData = symbolsDataAsset.GetDataByType(symbolsList[i]);
            targetSlots[i].SetSymbol(symbolData);
        }
    }

    public void OnWheelStopped()
    {
        stopping = false;

        if(currentWheel>=bonusWheels.Length)
        {
            OnWheelsEndedEvent?.Invoke();
        }
    }

    public void OnPressToStop()
    {
        if(stopping || currentWheel> bonusWheels.Length-1)return;
        bonusWheels[currentWheel].Stop();
        currentWheel++;
        stopping = true;
    }
}
