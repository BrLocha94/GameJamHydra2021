using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolsDataSet", menuName = "Data/SymbolsDataSet", order = 0)]
public class SymbolsDataAsset : ScriptableObject
{ 
    [field: SerializeField]public List<SymbolData> symbolsDataList {get; private set;}

    public SymbolData GetDataByType(ESymbol type)
    {
        return symbolsDataList.FirstOrDefault(symbol=>symbol.Type == type);
    }
}
