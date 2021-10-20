using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class ParticleSystemActivatorMixerPlayable : PlayableBehaviour
{
    private ParticleSystem m_ParticleSystem;
    private bool defaultValue;
    public ParticleSystemActivatorTrack track;
    

    public static ScriptPlayable<ParticleSystemActivatorMixerPlayable> Create(PlayableGraph graph, int inputCount)
    {
        return ScriptPlayable<ParticleSystemActivatorMixerPlayable>.Create(graph, inputCount);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (m_ParticleSystem == null)
        {
            m_ParticleSystem = playerData as ParticleSystem;
            defaultValue = m_ParticleSystem != null && m_ParticleSystem.isPlaying;
        }

        if (m_ParticleSystem == null)
            return;

        int inputCount = playable.GetInputCount();
        bool hasInput = false;
        for (int i = 0; i < inputCount; i++)
        {
            if (playable.GetInputWeight(i) > 0)
            {
                hasInput = true;
                break;
            }
        }

        SetPlayingState(hasInput);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        if (m_ParticleSystem == null)
            return;

        SetPlayingState(defaultValue);
    }

    private void SetPlayingState(bool play)
    {
        if (play && !m_ParticleSystem.isPlaying)
        {
            m_ParticleSystem.Play(track.includeChildren);
        }
        else if(!play && m_ParticleSystem.isPlaying)
        {
            m_ParticleSystem.Stop(track.includeChildren, track.stopBehaviour);
        }
    }

}