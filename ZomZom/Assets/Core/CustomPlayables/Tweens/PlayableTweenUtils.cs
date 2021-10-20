using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Profiling;

public static class PlayableTweenUtils
{
    public static void DelayedTime<T>(Playable playable, int itensCount, Action<(T[] inputs, float[] inputsTime, float[] inputsWeight, int itemIndex)>onItemProcess) where T : DelayedTweenBehaviour, new()
    {
        int inputsCount = playable.GetInputCount();
        float[] inputsTweenProgress = new float[inputsCount];
        float[] inputsWeight = new float[inputsCount];
        T[] behaviourInputs = new T[inputsCount];

        ScriptPlayable<T>[] playableInputs = new ScriptPlayable<T>[inputsCount];
        double[] originalPlayableTimes = new double[inputsCount];

        for (int j = 0; j < inputsCount; j++)
        {
            playableInputs[j] = (ScriptPlayable<T>)playable.GetInput(j);
            behaviourInputs[j] = playableInputs[j].GetBehaviour();
            originalPlayableTimes[j] = playableInputs[j].GetTime();
        }
  
        for (int i = 0; i < itensCount; i++)
        {
            for (int j = 0; j < inputsCount; j++)
            {
                var originalT = originalPlayableTimes[j];
                var playableInput = playableInputs[j];
                var input = behaviourInputs[j];
               
                var calculatedDelay = input.delay * ((itensCount - 1));
                var index = input.reverse? (itensCount-1)-i: i;
                var delayedTime = (originalT * (1 + calculatedDelay)) - input.delay * index * input.clipDuration;
                playableInput.SetTime(delayedTime);
                var time = playableInput.GetTime();
                var normalizedTime = (float)(time / input.clipDuration);
                var normalizedTimeWithDirection = input.backwards?1-normalizedTime:normalizedTime;

                inputsTweenProgress[j] = input.EvaluateCurrentCurve(normalizedTimeWithDirection);
                inputsWeight[j] = playable.GetInputWeight(j);
                playableInput.SetTime(originalT);
            }
            onItemProcess?.Invoke((behaviourInputs, inputsTweenProgress, inputsWeight, i));
        }
    }
}