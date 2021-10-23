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

    private int currentDenominationIndex = 0;

    private void Start()
    {
        
    }

    private void UpdateOutputText()
    {

    }
}
