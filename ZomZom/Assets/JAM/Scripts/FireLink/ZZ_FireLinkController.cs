using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ZZ_FireLinkController : MonoBehaviour
{
    [SerializeField] private FireLinkColumn[] fireLinkColumns;
    [SerializeField] private SymbolsDataAsset symbolDataAsset;
    [SerializeField] private SpriteRendererGroup gridSpriteRendererGroup;

    private List<ZZ_FireLink_Grid_Slot> earnedSymbolsList = new List<ZZ_FireLink_Grid_Slot>();
    private List<ZZ_FireLink_Grid_Slot> earnedRevealedSymbolsList = new List<ZZ_FireLink_Grid_Slot>();
    private List<ZZ_FireLink_Grid_Slot> earnedAnimatedSymbolsList = new List<ZZ_FireLink_Grid_Slot>();
    private List<FireLinkPlay> plays = new List<FireLinkPlay>();
    private FireLinkPlay currentPlay => plays[currentPlayIndex];

    private int currentPlayIndex = 0;

    public ESymbol winSymbolType = ESymbol.P5;
    public int valuePerSymbol = 15;

    public float delayBetweenSymbol = 0.1f;
    public float delayBetweenColumn = 0.2f;
    public float delayBeforeHideSymbols = 2f;
    public float delayBetweenPlays = 1.5f;
    public float symbolAppearDuration = 0.1f;
    public float symbolDisappearDuration = 0.1f;
    public JazzDev.Easing.EaseStyle symbolInStyle = JazzDev.Easing.EaseStyle.CircleEaseIn;
    public Vector3 startPosOffset = new Vector3(0, 0.2f, 0);

    public UnityEvent OnSymbolRevealInStarted;
    public UnityEvent OnPlayEnded;
    public UnityEvent OnSymbolsRevealOut;
    public UnityEvent OnSymbolWinRevealInEnded;

    private void Awake()
    {
        plays.Add(new FireLinkPlay()
        {
            winSymbolsCoords = new (int columnIndex, int rowIndex)[]
            {
                (0,2),
                (1,1),
                (1,2),
                (3,0),
            },
        });

        plays.Add(new FireLinkPlay()
        {
            winSymbolsCoords = new (int columnIndex, int rowIndex)[]
            {
                (0,0),
                (2,1),
            },
        });

        plays.Add(new FireLinkPlay()
        {
            winSymbolsCoords = new (int columnIndex, int rowIndex)[]
            {
                (0,1),
                (0,1),
            },
        });

        plays.Add(new FireLinkPlay()
        {
            winSymbolsCoords = new (int columnIndex, int rowIndex)[]
            {
                (4,0),
                (4,1),
            },
        });

        plays.Add(new FireLinkPlay()
        {
            winSymbolsCoords = new (int columnIndex, int rowIndex)[]
            {
                (2,2),
                (4,2),
            },
        });
    }

    private void Start()
    {
        // StartCoroutine(StartFireLink());

        gridSpriteRendererGroup.groupAlpha = 0;

        for (int i = 0; i < fireLinkColumns.Length; i++)
        {
            for (int j = 0; j < fireLinkColumns[i].Slots.Count; j++)
            {
                ZZ_FireLink_Grid_Slot slot = fireLinkColumns[i].Slots[j];
                slot.symbol.SetOpacity(0);
            }
        }
    }

    public IEnumerator StartFireLink()
    {

        currentPlayIndex = 0;

        earnedSymbolsList.Clear();
        earnedRevealedSymbolsList.Clear();
        earnedAnimatedSymbolsList.Clear();

        gridSpriteRendererGroup.groupAlpha = 1;

        for (int i = 0; i < plays.Count; i++)
        {
            SetPlaySymbols(currentPlay);
            yield return SymbolsInRoutine(i != 0);
            yield return new WaitForSeconds(delayBeforeHideSymbols);
            yield return SymbolsOutRoutine();
            yield return new WaitForSeconds(delayBetweenPlays);
            earnedAnimatedSymbolsList.Clear();
            currentPlayIndex++;
        }
        earnedSymbolsList.Clear();
        earnedRevealedSymbolsList.Clear();
        gridSpriteRendererGroup.groupAlpha = 0;

        for (int i = 0; i < fireLinkColumns.Length; i++)
        {
            for (int j = 0; j < fireLinkColumns[i].Slots.Count; j++)
            {
                ZZ_FireLink_Grid_Slot slot = fireLinkColumns[i].Slots[j];
                slot.HideText();
            }
        }
    }

    private void SetPlaySymbols(FireLinkPlay play)
    {
        for (int i = 0; i < play.winSymbolsCoords.Length; i++)
        {
            (int column, int rowIndex) coord = play.winSymbolsCoords[i];
            ZZ_FireLink_Grid_Slot slot = fireLinkColumns[coord.column].Slots[coord.rowIndex];

            if (!earnedSymbolsList.Contains(slot))
            {
                earnedSymbolsList.Add(slot);
            }
            slot.SetSymbol(symbolDataAsset.GetDataByType(winSymbolType));
        }

        for (int i = 0; i < fireLinkColumns.Length; i++)
        {
            for (int j = 0; j < fireLinkColumns[i].Slots.Count; j++)
            {
                ZZ_FireLink_Grid_Slot slot = fireLinkColumns[i].Slots[j];
                if (earnedSymbolsList.Contains(slot)) continue;
                slot.SetSymbol(symbolDataAsset.GetDataByType(GetRandomSymbol()));
            }
        }
    }

    private ESymbol GetRandomSymbol()
    {
        int winSymbolIndex = (int)winSymbolType;
        int randomSymbolIndex = winSymbolIndex;
        while (randomSymbolIndex == winSymbolIndex)
        {
            randomSymbolIndex = Random.Range(1, 6);
        }
        return (ESymbol)randomSymbolIndex;
    }

    IEnumerator SymbolsInRoutine(bool ignoreEarnedSymbols = true)
    {
        for (int i = 0; i < fireLinkColumns.Length; i++)
        {
            for (int j = 0; j < fireLinkColumns[i].Slots.Count; j++)
            {
                ZZ_FireLink_Grid_Slot slot = fireLinkColumns[i].Slots[j];

                bool earnedSymbolIsRevealed = earnedRevealedSymbolsList.Contains(slot);
                bool isEarnedSymbols = earnedSymbolsList.Contains(slot);

                if (isEarnedSymbols)
                {
                    if (earnedSymbolIsRevealed)
                    {
                        continue;
                    }
                    else
                    {
                        earnedRevealedSymbolsList.Add(slot);
                        earnedAnimatedSymbolsList.Add(slot);
                    }

                }

                OnSymbolRevealInStarted?.Invoke();

               
                StartCoroutine(CoroutineUtility.EaseRoutine(symbolAppearDuration, symbolInStyle, (v, t) =>
                {
                    slot.symbol.SetOpacity(t);
                    slot.symbol.SetPosition(Vector3.LerpUnclamped(startPosOffset, Vector3.zero, t));
                }, endAction: () =>
                  {
                      if (isEarnedSymbols)
                      {
                          OnSymbolWinRevealInEnded?.Invoke();
                      }
                  }));

                if (j < fireLinkColumns[i].Slots.Count - 1) yield return new WaitForSeconds(delayBetweenSymbol);
            }

            yield return new WaitForSeconds(delayBetweenColumn);
        }




        yield return EndPlayRoutine();

        OnPlayEnded?.Invoke();
    }

    private IEnumerator EndPlayRoutine()
    {
        float duration = 1.2f;
        float showTextDuration = 1f;

        yield return CoroutineUtility.EaseRoutine(duration, JazzDev.Easing.EaseStyle.QuadEaseOut, (v, t) =>
        {
            for (int i = 0; i < earnedAnimatedSymbolsList.Count; i++)
            {
                earnedAnimatedSymbolsList[i].symbol.SetTopColor(Color.Lerp(Color.black, Color.white, Mathf.PingPong(t * 4f, 1)));
            }
        });

        for (int i = 0; i < earnedAnimatedSymbolsList.Count; i++)
        {
            earnedAnimatedSymbolsList[i].ShowText(valuePerSymbol, 1f);
        }

       yield return new WaitForSeconds(showTextDuration);
    }

    private IEnumerator SymbolsOutRoutine()
    {
        for (int i = 0; i < fireLinkColumns.Length; i++)
        {
            for (int j = 0; j < fireLinkColumns[i].Slots.Count; j++)
            {
                ZZ_FireLink_Grid_Slot slot = fireLinkColumns[i].Slots[j];

                if (!earnedSymbolsList.Contains(slot))
                {
                    StartCoroutine(CoroutineUtility.EaseRoutine(symbolDisappearDuration, JazzDev.Easing.EaseStyle.Linear, (v, t) =>
                    {
                        slot.symbol.SetOpacity(1 - t);
                    }));
                }
            }
        }
        OnSymbolsRevealOut?.Invoke();
        yield return new WaitForSeconds(symbolDisappearDuration);
    }

    [System.Serializable]
    private class FireLinkColumn
    {
        [field: SerializeField] public List<ZZ_FireLink_Grid_Slot> Slots { get; private set; }
    }
}

public struct FireLinkPlay
{
    public (int columnIndex, int rowIndex)[] winSymbolsCoords;
}
