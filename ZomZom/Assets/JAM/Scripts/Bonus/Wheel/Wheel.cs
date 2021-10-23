using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Wheel : MonoBehaviour
{
   
    private PlayableDirector m_PlayableDirector;
    private bool loop = true;
   

    private void Awake()
    {
        m_PlayableDirector = GetComponent<PlayableDirector>();
    }

   /* public void SetTime(float seconds)
    {
        if (!loop) return;

        if (m_PlayableDirector == null)
            m_PlayableDirector = GetComponent<PlayableDirector>();

        m_PlayableDirector.time = seconds;
    }

    public void Begin(int targetSymbolIndex)
    {
        loop = true;
        m_PlayableDirector.time = 0;
        m_PlayableDirector.Play();
        material.SetVector(offsetParameter, new Vector2(0, targetSymbolIndex));
    }

    public void Stop()
    {
        loop = false;
    }*/

  
}

