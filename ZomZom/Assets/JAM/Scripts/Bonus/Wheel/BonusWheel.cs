using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BonusWheel : MonoBehaviour
{
    [SerializeField] SO_FloatStartEndData rotationOffset;
    
    private PlayableDirector m_PlayableDirector;

    private bool loop = true;
   
    private void Awake()
    {
        m_PlayableDirector = GetComponent<PlayableDirector>();
    }

    public void SetTime(float seconds)
    {
        if (!loop) return;

        if (m_PlayableDirector == null)
            m_PlayableDirector = GetComponent<PlayableDirector>();

        m_PlayableDirector.time = seconds;
    }

    public void Begin(float endOffset)
    {
        loop = true;
        m_PlayableDirector.time = 0;
        m_PlayableDirector.Play();
        StartCoroutine(delay(endOffset));
    }
    IEnumerator delay(float endOffset)
    {
        yield return new WaitForSeconds(1f);
        rotationOffset.start = endOffset;
    }

    public void Stop()
    {
        loop = false;
    }
}

