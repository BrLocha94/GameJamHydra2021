using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZZ_Grid_Slot : MonoBehaviour
{
    public ZZ_Slot_Symbol symbol;
    public ESymbol symbolType {get; private set;}
    public void SetSymbol(SymbolData symbolData)
    {
        symbolType = symbolData.Type;
        symbol.SetSprite(symbolData.Sprite);
    }
}


