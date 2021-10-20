using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayableCallbackBehaviour: PlayableBehaviour
{
    public double clipDuration=> clip.duration * clip.timeScale;

    [SerializeField] protected List<TweenParameterCallback<Scriptable.Events.Void>> callbackVoidEvents = new List<TweenParameterCallback<Scriptable.Events.Void>>();
    [SerializeField] protected List<TweenParameterCallback<bool>> callbackBoolEvents = new List<TweenParameterCallback<bool>>();
    [SerializeField] protected List<TweenParameterCallback<float>> callbackFloatEvents = new List<TweenParameterCallback<float>>();
    [SerializeField] protected List<TweenParameterCallback<int>> callbackIntEvents = new List<TweenParameterCallback<int>>();
    [SerializeField] protected List<TweenParameterCallback<string>> callbackStringEvents = new List<TweenParameterCallback<string>>();
    [SerializeField] protected List<TweenAudioCallback> callbackAudioEvents = new List<TweenAudioCallback>();

    public List<List<TweenCallbackBase>> callbacks = new List<List<TweenCallbackBase>>();

    protected float lastTime;
    protected float lastTimeDeltaDir;

    public TimelineClip clip;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        callbacks.Clear();
        InitCallbacks(callbackVoidEvents, playable);
        InitCallbacks(callbackBoolEvents, playable);
        InitCallbacks(callbackFloatEvents, playable);
        InitCallbacks(callbackIntEvents, playable);
        InitCallbacks(callbackStringEvents, playable);
        InitCallbacks(callbackAudioEvents, playable);

        lastTime = (float)(playable.GetTime() / clipDuration);
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);
        float time = (float)(playable.GetTime() / clipDuration);
        EvaluateCallbacks(time);
    }
    protected virtual void ProcessCallbacks(float time)
    {
        ProcessCallbackList(callbackVoidEvents, time);
        ProcessCallbackList(callbackBoolEvents, time);
        ProcessCallbackList(callbackFloatEvents, time);
        ProcessCallbackList(callbackIntEvents, time);
        ProcessCallbackList(callbackStringEvents, time);
        ProcessCallbackList(callbackAudioEvents, time);
    }
    protected void InitCallbacks<T>(List<T> callbackList, Playable playable) where T : TweenCallbackBase
    {
        callbacks.Add(new List<TweenCallbackBase>(callbackList));

        for (int i = 0; i < callbackList.Count; i++)
        {
            callbackList[i].Initialize(playable);
        }

    }
    private void EvaluateCallbacks(float time)
    {
        float delt = time - lastTime;

        if (delt == 0) return;

        float actualTimeDeltaDir = Mathf.Sign(delt);

        ///We need to reset lastTime if timeline changes its evaluation direction to avoid fire events incorrectly
        if (actualTimeDeltaDir != lastTimeDeltaDir && lastTimeDeltaDir != 0)
        {
            lastTime = time;
        }

        ProcessCallbacks(time);

        delt = time - lastTime;

        if (delt != 0)
        {
            lastTimeDeltaDir = Mathf.Sign(time - lastTime);
        }
        else
        {
            lastTimeDeltaDir = 0;
        }

        lastTime = time;
    }
    private void ProcessCallbackList<T>(List<T> callbackList, float time) where T : TweenCallbackBase
    {
        for (int i = 0; i < callbackList.Count; i++)
        {
            var currentEvent = callbackList[i];

            if (lastTime < time)
            {
                if (currentEvent.time == 0)
                {
                    if (lastTime <= currentEvent.time && time > currentEvent.time)
                    {
                        currentEvent.Fire();
                    }
                }
                else
                {
                    if (lastTime < currentEvent.time && time >= currentEvent.time)
                    {
                        currentEvent.Fire();
                    }
                }
            }
            else if (lastTime > time)
            {
                if (currentEvent.time == 0)
                {
                    if (lastTime > currentEvent.time && time <= currentEvent.time)
                    {
                        currentEvent.Fire();
                    }
                }
                else
                {
                    if (lastTime >= currentEvent.time && time < currentEvent.time)
                    {
                        currentEvent.Fire();
                    }
                }
            }
        }
    }
}

