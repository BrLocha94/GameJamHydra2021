using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZZ_Grid_Reel : TweenableBase<float>
{
    [SerializeField] private Material material;
    [SerializeField] public List<ZZ_Grid_Slot> slots = new List<ZZ_Grid_Slot>();
    [SerializeField] private SO_FloatStartEndData fromToReelAnimation;
    private int offsetParameter = Shader.PropertyToID("_SlotOffset");
    private int ReelSize => slots.Count;

    private void Awake()
    {
        fromToReelAnimation.end = 0;
        fromToReelAnimation.start = 0;
    }
    public float Offset
    {
        get => material.GetVector(offsetParameter).y;
        set => material.SetVector(offsetParameter, new Vector4(0, Mathf.Repeat(value, ReelSize), 0, 0));
    }
    public override Dictionary<int, string> TweenableMembers { get; } = new Dictionary<int, string>
    {
        {0, "Offset"}
    };
    public override void SetTweenableValue(int index, float value)
    {
        if (index == 0)
        {
            Offset = value;
        }
    }
    public override float GetTweenableValue(int index)
    {
        if (index == 0)
        {
            return Offset;
        }
        return 0;
    }

    public void ResetFromToAnimation()
    {
        fromToReelAnimation.start = 0;
        fromToReelAnimation.end = 3;
    }
    public void SetSymbols(List<SymbolData> symbolsData)
    {
        int slotsCount = symbolsData.Count;
        int end = (int)fromToReelAnimation.end + slotsCount;

        if (end > ReelSize)
        {
            end -= ReelSize;
        }
        fromToReelAnimation.end = end;
        fromToReelAnimation.start = fromToReelAnimation.end - slotsCount;

        for (int i = 0; i < slotsCount; i++)
        {
            slots[(int)Mathf.Repeat(fromToReelAnimation.end + i, ReelSize)].SetSymbol(symbolsData[i]);
        }
        
    }
}

[System.Serializable]
public struct SymbolData
{
    [field:SerializeField]public ESymbol Type {get; private set;}
    [field:SerializeField]public Sprite Sprite {get; private set;}
}

public enum ESymbol : int
{
    Bonus = 0,
    P1 = 1,
    P2 = 2,
    P3 = 3,
    P4 = 4,
    P5 = 5,
    P6 = 6,
    Null = -1
}