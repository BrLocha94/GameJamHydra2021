using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolsDataSet", menuName = "Data/SymbolsDataSet", order = 0)]
public class SymbolsDataAsset : ScriptableObject
{ 
    [field: SerializeField]public List<SymbolData> symbolsDataList {get; private set;}
}
