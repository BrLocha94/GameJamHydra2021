using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZZ_GameManager : MonoBehaviour
{
    [SerializeField] private DirectorPlayer gridPlayer;
    [SerializeField] private ZZ_Grid_Controller gridController;
    public void OnRevealOutFinished()
    {
        Debug.Log("OnRevealOutFinished");
    }
    public void Play()
    {
        gridPlayer.Play(0, wrapMode: UnityEngine.Playables.DirectorWrapMode.Hold);
    }
}
