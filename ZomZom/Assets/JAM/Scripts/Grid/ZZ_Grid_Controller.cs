using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZZ_Grid_Controller : MonoBehaviour
{
    public List<ZZ_Grid_Reel> reels = new List<ZZ_Grid_Reel>();

    [SerializeField] private SymbolsDataAsset symbolsDataAsset;

    public void Awake()
    {
        for (int i = 0; i < reels.Count; i++)
        {
            reels[i].Offset = 0;
        }
    }
    public void PrepareNextPlay(GridPlaySetup setup)
    {
        for (int reelIndex = 0; reelIndex < setup.reelsEntrys.Count; reelIndex++)
        {
            List<(ESymbol symbol, int index)> entrys = setup.reelsEntrys[reelIndex];

            ZZ_Grid_Reel reel = reels[reelIndex];

            for (int j = 0; j < entrys.Count; j++)
            {
                (ESymbol symbol, int index) entry = entrys[j];
                SymbolData symbolAsset = symbolsDataAsset.symbolsDataList.FirstOrDefault(symbol => symbol.Type == entry.symbol);
                reel.SetSymbol(symbolAsset, entry.index);
            }
        }

        for (int i = 0; i < setup.fromToReelAnimation.Count; i++)
        {
            ZZ_Grid_Reel reel = reels[i];
            reel.SetFromToAnimation(setup.fromToReelAnimation[i]);
        }
    }
}
