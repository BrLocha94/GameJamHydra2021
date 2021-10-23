using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelVFXController : TweenableBase<Color>
{
    [SerializeField] private Reel reel;
    public int bonusSlotIndex;

    private Color otherSymbolsColor = Color.white;

    public override Dictionary<int, string> TweenableMembers { get; }= new Dictionary<int, string>()
    {
        {0, "BonusSymbolColor"},
        {1, "OtherSymbolscolor"}
    };

    public override void SetTweenableValue(int index, Color value)
    {
        switch (index)
        {
            case 0: 
                reel.slots[bonusSlotIndex].SymbolSlot.color = value;
                break;
            case 1: 
                otherSymbolsColor = value;

                for (int i = 0; i < reel.slots.Count; i++)
                {
                    if(i!=bonusSlotIndex)
                    {
                        reel.slots[i].SymbolSlot.color = value;
                    }
                }
                break;
        }
    }

    public override Color GetTweenableValue(int index)
    {
        switch (index)
        {
            case 0: return reel.slots[bonusSlotIndex].SymbolSlot.color;
            case 1: return otherSymbolsColor;
        }
        return Color.clear;
    }
}
