
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorPlayer : MonoBehaviour
{
    [SerializeField] private TimelineAsset[] timelines;
    private Dictionary<string, TimelineAsset> timelineDict;
    private PlayableDirector m_Director;
    private IEnumerator m_PlayRoutine;
    private IEnumerator m_PreDelayRoutine;
    private void Awake()
    {
        m_Director = GetComponent<PlayableDirector>();
        timelineDict = new Dictionary<string, TimelineAsset>();

        for (int i = 0; i < timelines.Length; i++)
        {
            var tml = timelines[i];
            timelineDict.Add(tml.name, tml);
        }

        m_Director.playableAsset = timelines[0].asset;

    }
    public void Play(string timelineName = "", double startTime = 0, float startDelay = 0, float endDelay = 0, DirectorWrapMode wrapMode = DirectorWrapMode.None, System.Action OnEnd = null)
    {
        if (timelineName != string.Empty)
        {
            TimelineAsset tAsset;

            if (timelineDict.TryGetValue(timelineName, out tAsset))
            {
                PlayInternal(tAsset.asset, startTime, startDelay+tAsset.preDelay, endDelay+tAsset.postDelay,wrapMode,OnEnd);
            }
            else
            {
                OnEnd?.Invoke();
            }
        }
    }
    public void Play(int timelineIndex = 0, double startTime = 0, float startDelay = 0, float endDelay = 0, DirectorWrapMode wrapMode = DirectorWrapMode.None, System.Action OnEnd = null)
    {
        if(timelineIndex>=0 && timelineIndex<timelines.Length)
        {
            TimelineAsset tAsset = timelines[timelineIndex];
            PlayInternal(tAsset.asset, startTime, startDelay+tAsset.preDelay, endDelay+tAsset.postDelay,wrapMode,OnEnd);
        }
        else
        {
            OnEnd?.Invoke();
        }
    }
    private void PlayInternal(PlayableAsset timeline, double startTime = 0, float startDelay = 0, float endDelay = 0, DirectorWrapMode wrapMode = DirectorWrapMode.None, System.Action OnEnd = null)
    {
        if (m_PreDelayRoutine != null) { StopCoroutine(m_PreDelayRoutine); }
        m_PreDelayRoutine = PreDelay();
        StartCoroutine(m_PreDelayRoutine);

        IEnumerator PreDelay()
        {
            if (startDelay > 0) yield return new WaitForSeconds(startDelay);
            if (m_PlayRoutine != null) { StopCoroutine(m_PlayRoutine); }
            m_PlayRoutine = PlayRoutine();
            StartCoroutine(m_PlayRoutine);
        }

        IEnumerator PlayRoutine()
        {
            m_Director.time = startTime;
            m_Director.extrapolationMode = wrapMode;
            m_Director.timeUpdateMode = DirectorUpdateMode.GameTime;
            m_Director.playableAsset = timeline;
            yield return m_Director.PlayDelayed(0, endDelay);
            OnEnd?.Invoke();
        }
    }
    public void Stop()
    {
        m_Director.Stop();
    }

    [System.Serializable]
    private class TimelineAsset
    {
        public string name;
        public PlayableAsset asset;
        [Min(0)] public float preDelay;
        [Min(0)] public float postDelay;
    }
}

public static class TimelineExtensions
{
    public static IEnumerator PlayDelayed(this PlayableDirector timeline, float startDelay = 0, float endDelay = 0)
    {
        if(startDelay > 0)
            yield return new WaitForSeconds(startDelay);

        timeline.Play();
        yield return new WaitForTimeline(timeline);

        if (endDelay > 0)
            yield return new WaitForSeconds(endDelay);
    }
}

public class WaitForTimeline : CustomYieldInstruction
{
    private PlayableDirector director;
    private double lastTime;

    public WaitForTimeline(PlayableDirector director)
    {
        this.director = director;
        lastTime = this.director.time;
    }
    public override bool keepWaiting
    {
        get
        {
            if(director.extrapolationMode == DirectorWrapMode.None)
            {
                if(lastTime>director.time) return false;
                lastTime = director.time;
                return true;
            }
            else if(director.extrapolationMode == DirectorWrapMode.Loop)
            {
                return director.state == PlayState.Playing;
            }
            else
            {
                return director.time<director.duration;
            }
        }
    }
}
