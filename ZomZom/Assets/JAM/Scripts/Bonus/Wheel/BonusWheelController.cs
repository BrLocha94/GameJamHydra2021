using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BonusWheelController : MonoBehaviour
{
    [SerializeField] BonusWheel[] bonusWheels;

    private int currentWheel = 0;
    private bool stopping = false;

    public void Begin(List<ESymbol> symbolsList)
    {
        currentWheel = 0;
        stopping = false;

        for (int i = 0; i < bonusWheels.Length; i++)
        {
           bonusWheels[i].Begin((int)symbolsList[i]);
        }
    }

    public void OnWheelStopped()
    {
        stopping = false;
    }

    public void OnPressToStop()
    {
        if(stopping || currentWheel> bonusWheels.Length-1)return;
        bonusWheels[currentWheel].Stop();
        currentWheel++;
        stopping = true;
    }
}
