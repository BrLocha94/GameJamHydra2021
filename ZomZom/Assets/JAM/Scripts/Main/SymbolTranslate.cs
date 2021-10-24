using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SymbolTranslate
{
    public static ESymbol TranslateToSymbol(string target)
    {
        switch (target)
        {
            case "p1":
                return ESymbol.Bonus;
            case "p2":
                return ESymbol.P2;
            case "p3":
                return ESymbol.P3;
            case "p4":
                return ESymbol.P4;
            case "p5":
                return ESymbol.P5;
        }

        Debug.Log("There is no " + target + " mapped in conversion method");
        return ESymbol.Null;
    }
}
