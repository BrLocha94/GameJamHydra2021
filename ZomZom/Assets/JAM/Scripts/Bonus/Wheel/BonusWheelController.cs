using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BonusWheelController : MonoBehaviour
{

    [SerializeField] Sprite[] targetSymbol;
    [SerializeField] Sprite[] moneyAmount;
    [SerializeField] Sprite[] gridExpansion;

    [SerializeField] ZZ_Grid_Reel gridReel1;
    [SerializeField] ZZ_Grid_Reel gridReel2;
    [SerializeField] ZZ_Grid_Reel gridReel3;

    [SerializeField] BonusWheel[] bonusWheels;
    [SerializeField] ZZ_Grid_Slot[] targetSlots;
    [SerializeField] SymbolsDataAsset symbolsDataAsset;
    public event Action OnWheelsEndedEvent;

    private int currentWheel = 0;
    private bool stopping = false;

    public void Begin(List<Vector2> fromToReelAnimationOffset)
    {
        currentWheel = 0;
        stopping = false;

        bonusWheels[0].Begin(fromToReelAnimationOffset[0].y);
        bonusWheels[1].Begin(fromToReelAnimationOffset[1].y);
        bonusWheels[2].Begin(fromToReelAnimationOffset[2].y);

       StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);

         for (int i = 0; i < gridReel1.slots.Count; i++)
        {
            gridReel1.slots[i].symbol.SetSprite(targetSymbol[i]);
        }

        for (int i = 0; i < gridReel2.slots.Count; i++)
        {
            gridReel2.slots[i].symbol.SetSprite(moneyAmount[(int)Mathf.Repeat(i, moneyAmount.Length - 1)]);
        }
        for (int i = 0; i < gridReel3.slots.Count; i++)
        {
            gridReel3.slots[i].symbol.SetSprite(gridExpansion[(int)Mathf.Repeat(i, gridExpansion.Length - 1)]);
        }
    }

    public void RevertChangedSprites()
    {
        for (int i = 0; i < gridReel2.slots.Count; i++)
        {
            gridReel2.slots[i].symbol.RevertToLastSprite();
        }

        for (int i = 0; i < gridReel3.slots.Count; i++)
        {
            gridReel3.slots[i].symbol.RevertToLastSprite();
        }

    }

    public void OnWheelStopped()
    {
        stopping = false;

        if (currentWheel >= bonusWheels.Length)
        {
            OnWheelsEndedEvent?.Invoke();
        }
    }

    public void OnPressToStop()
    {
        if (stopping || currentWheel > bonusWheels.Length - 1) return;
        bonusWheels[currentWheel].Stop();
        currentWheel++;
        stopping = true;
    }
}
