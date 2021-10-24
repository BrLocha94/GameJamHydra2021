using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UiTextMesh : MonoBehaviour
{
    [SerializeField]
    private string prefix = "";
    [SerializeField]
    private string sufix = "";

    TextMeshProUGUI targetText;

    private void Awake()
    {
        targetText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(string text)
    {
        targetText.text = prefix + text + sufix;
    }

    public void UpdateTextFormatedCash(int value)
    {
        targetText.text = prefix + value.FormatStringCashNoCents() + sufix;
    }
}
