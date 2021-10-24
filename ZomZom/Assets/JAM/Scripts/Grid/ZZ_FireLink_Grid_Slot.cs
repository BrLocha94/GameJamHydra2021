using TMPro;

public class ZZ_FireLink_Grid_Slot : ZZ_Grid_Slot
{
    private TextMeshPro textComponent;
    private void Awake()
    {
        symbol = GetComponentInChildren<ZZ_Slot_Symbol>();
        textComponent = GetComponentInChildren<TextMeshPro>();
        textComponent.alpha = 0;
    }

    public void ShowText(int value, float duration)
    {
        textComponent.text = "$"+value.ToString();
        StartCoroutine(CoroutineUtility.EaseRoutine(duration, JazzDev.Easing.EaseStyle.QuadEaseInOut, (v,t)=>
        {
            textComponent.alpha = t;
            symbol.SetOpacity(1-t);
        }));
    }
    public void HideText()
    {
         textComponent.alpha = 0;
    }
}


