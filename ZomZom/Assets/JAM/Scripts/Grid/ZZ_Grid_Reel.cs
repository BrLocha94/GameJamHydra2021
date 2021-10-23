using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZZ_Grid_Reel : TweenableBase<float>
{
    [SerializeField] private Material material;
    [SerializeField] private List<ZZ_Grid_Slot> slots = new List<ZZ_Grid_Slot>();
    private int offsetParameter = Shader.PropertyToID("_SlotOffset");
    public float Offset
    {
        get => material.GetVector(offsetParameter).y;
        set => material.SetVector(offsetParameter, new Vector4(0, value, 0, 0));
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
    public void SetSymbol(SymbolData symbolData, int slotIndex)
    {
        slots[slotIndex].SetSymbol(symbolData);
    }
}

[CreateAssetMenu(fileName = "SymbolsDataSet", menuName = "Data/SymbolsDataSet", order = 0)]
public class SymbolsDataAsset : ScriptableObject
{ 
    [field: SerializeField]public List<SymbolData> symbolsDataList {get; private set;}
}

[System.Serializable]
public struct SymbolData
{
    [field:SerializeField]public ESymbol Type {get; private set;}
    [field:SerializeField]public Sprite Sprite {get; private set;}
}

public enum ESymbol
{
    P1,
    P2,
    P3,
    P4,
    P5,
    P6,
    Bonus,
}