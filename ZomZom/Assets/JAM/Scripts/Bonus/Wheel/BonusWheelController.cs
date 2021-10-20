using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BonusWheelController : MonoBehaviour
{
    [SerializeField] Wheel[] wheels;
    private int currentWheel = 0;

    void Start()
    {
        Begin();
    }

   public void Begin()
    {
        currentWheel = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Begin();
        }
    }


    public void OnPressToStop()
    {
        if(currentWheel>wheels.Length-1)return;

        wheels[currentWheel].Stop();
        currentWheel++;
    }
}
