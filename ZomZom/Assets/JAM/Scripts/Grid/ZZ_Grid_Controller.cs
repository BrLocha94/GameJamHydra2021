using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ZZ_Grid_Controller : MonoBehaviour
{
    public List<ZZ_Grid_Reel> reels = new List<ZZ_Grid_Reel>();
    [SerializeField] private DirectorPlayer gridPlayer;
    [SerializeField] private SymbolsDataAsset symbolsDataAsset;
    public UnityEvent OnReviewEndedEvent;

    public void Awake()
    {
        for (int i = 0; i < reels.Count; i++)
        {
            reels[i].Offset = 0;
        }
    }
    public void Play(GridPlaySetup setup)
    {
        for (int reelIndex = 0; reelIndex < setup.reelsEntrys.Count; reelIndex++)
        {
            List<ESymbol> entrys = setup.reelsEntrys[reelIndex];
            List<SymbolData> symbolsData = new List<SymbolData>();
            ZZ_Grid_Reel reel = reels[reelIndex];

            for (int j = 0; j < entrys.Count; j++)
            {
                ESymbol entry = entrys[j];
                symbolsData.Add(symbolsDataAsset.symbolsDataList.FirstOrDefault(symbol => symbol.Type == entry));
            }
            reel.SetSymbols(symbolsData);
        }

    
        gridPlayer.Play("SymbolsIn", wrapMode: UnityEngine.Playables.DirectorWrapMode.Hold, OnEnd: OnReviewEnded);
    }

    private void OnReviewEnded()
    {
        OnReviewEndedEvent?.Invoke();
    }
}
