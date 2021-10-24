using System;
using System.Globalization;

public class PlayerMoney
{
    private static PlayerMoney _instance;
    public static PlayerMoney instance { get { if (_instance == null) _instance = new PlayerMoney(); return _instance; } }

    private int _balance;

    public int getBalance() { return _balance; }

    public void addToBalance(int money)
    {
        int previous = _balance;
        _balance += money;
        _onBalanceChange?.Invoke(previous, _balance);
    }

    public void subtractFromBalance(int money)
    {
        int previous = _balance;
        if (PlayerMoney.instance.getBalance() < money)
            throw new Exception("No money!");
        _balance -= money;
        _onBalanceChange?.Invoke(previous, _balance);
    }

    public void resetBalance()
    {
        int previous = _balance;
        _balance = 0;
        _onBalanceChange?.Invoke(previous, _balance);
    }

    private Action<int, int> _onBalanceChange;
    public void setListener(Action<int, int> onBalanceChange)
    {
        _onBalanceChange = onBalanceChange;
    }

    public static string format(float value, bool divideBy100 = true)
    {
        if (divideBy100)
            value /= 100;
        var formatted = string.Format(CultureInfo.GetCultureInfo("en-US"), "{0:C}", value);
        return "$ " + formatted.Substring(1);
    }

}
