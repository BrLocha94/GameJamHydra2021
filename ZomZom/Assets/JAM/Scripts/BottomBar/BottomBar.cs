using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    [SerializeField]
    private Denomination denomination;
    [SerializeField]
    private Text winText;
    [SerializeField]
    private Text cashText;

    private void Awake()
    {
        PlayerMoney.instance.setListener(OnBalanceChange);
    }

    private void OnBalanceChange(int previous, int balance)
    {
        cashText.text = PlayerMoney.format(balance);
    }
}
