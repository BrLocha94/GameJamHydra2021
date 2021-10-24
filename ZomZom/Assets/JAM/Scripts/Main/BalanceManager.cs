using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BalanceEventHandler(int value);

public static class BalanceManager
{
    public static event BalanceEventHandler onBalanceChange;
    public static event BalanceEventHandler onWinChange;

    private static int balance = 0;
    private static int win = 0;
    private static int denomination = 0;

    public static void UpdateDenomination(int value)
    {
        denomination = value;
    }

    public static void UpdateWinAmount(int value)
    {
        win = value;
        AddBalance(win);
        onWinChange?.Invoke(win);
    }

    public static void AddBalance(int value)
    {
        balance += value;
        onBalanceChange?.Invoke(balance);
    }

    public static bool ExecutePlay()
    {
        if(balance < denomination)
        {
            Debug.Log("No cash avaliable");
            return false;
        }

        balance -= denomination;
        onBalanceChange?.Invoke(balance);

        return true;
    }
}
