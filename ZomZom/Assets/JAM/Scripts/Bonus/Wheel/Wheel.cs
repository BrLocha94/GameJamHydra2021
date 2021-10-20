using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Wheel : MonoBehaviour
{
    [SerializeField]private GameObject mask;
    private PlayableDirector m_PlayableDirector;
    private bool loop = true;

    private void Awake()
    {
        m_PlayableDirector = GetComponent<PlayableDirector>();
    }
    public void SetTime(float seconds)
    {
        if(!loop)return;

        if (m_PlayableDirector == null) m_PlayableDirector = GetComponent<PlayableDirector>();
        m_PlayableDirector.time = seconds;
    }

    public void Begin()
    {
        mask.SetActive(false);
        loop = true;
        m_PlayableDirector.time = 0;
        m_PlayableDirector.Play();
    }
    public void Stop()
    {
        loop = false;
    }
}
    
