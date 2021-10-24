using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Denomination : MonoBehaviour
{
    [SerializeField]
    private Text outputText;
    [SerializeField]
    private int[] allowedDenominations = { 25, 50, 100, 150, 200 };

    private int index = 0;

    private void Start()
    {
        UpdateDenomination(0);
    }

    public int GetCurrentDenomination() => allowedDenominations[index];

    public void UpdateDenomination(int factor)
    {
        index += factor;

        if (index < 0)
            index = allowedDenominations.Length - 1;
        else if (index >= allowedDenominations.Length)
            index = 0;

        BalanceManager.UpdateDenomination(allowedDenominations[index]);
        UpdateOutputText();
    }

    private void UpdateOutputText()
    {
        outputText.text = allowedDenominations[index].FormatStringCashNoCents();
    }
}
