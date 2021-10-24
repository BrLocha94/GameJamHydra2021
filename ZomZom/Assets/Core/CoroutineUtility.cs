using JazzDev.Easing;
using System.Collections;
using UnityEngine;

public static class CoroutineUtility
{

    #region CurveRoutine
    public static IEnumerator CurveRoutine(float duration, AnimationCurve curve, System.Action startAction, System.Action<float, float> midAction, System.Action endAction)
    {
        if (curve == null)
        {
            curve = new AnimationCurve();
            curve.AddKey(0f, 0f);
            curve.AddKey(1f, 1f);
        }

        startAction?.Invoke();
        var t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / duration;
            t = Mathf.Clamp01(t);
            midAction?.Invoke(t, curve.Evaluate(t));

            yield return null;
        }
        endAction?.Invoke();
    }
    public static IEnumerator CurveRoutine(float duration, AnimationCurve curve, System.Action startAction, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, curve, startAction, midAction, null);
    }
    public static IEnumerator CurveRoutine(float duration, AnimationCurve curve, System.Action<float, float> midAction, System.Action endAction)
    {
        return CurveRoutine(duration, curve, null, midAction, endAction);
    }
    public static IEnumerator CurveRoutine(float duration, AnimationCurve curve, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, curve, null, midAction, null);
    }

    public static IEnumerator CurveRoutine(float duration, System.Action<float, float> midAction, System.Action endAction)
    {
        return CurveRoutine(duration, null, () => { }, midAction, endAction);
    }
    public static IEnumerator CurveRoutine(float duration, System.Action startAction, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, null, startAction, midAction, () => { });
    }
    public static IEnumerator CurveRoutine(float duration, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, null, () => { }, midAction, () => { });
    }

    #endregion

    #region EaseRoutine
    public static IEnumerator EaseRoutine(float duration, EaseStyle easeStyle, System.Action startAction, System.Action<float, float> midAction, System.Action endAction)
    {
        Ease ease = EaseMethods.GetEase(easeStyle);

        startAction?.Invoke();
        var t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / duration;
            t = Mathf.Clamp01(t);
            midAction?.Invoke(t, EaseMethods.EasedLerp(ease, 0, 1, t));
            yield return null;
        }
        endAction?.Invoke();
    }
    public static IEnumerator EaseRoutine(float duration, EaseStyle easeStyle, System.Action startAction, System.Action<float, float> midAction)
    {
        return EaseRoutine(duration, easeStyle, startAction, midAction, null);
    }
    public static IEnumerator EaseRoutine(float duration, EaseStyle easeStyle, System.Action<float, float> midAction, System.Action endAction)
    {
        return EaseRoutine(duration, easeStyle, null, midAction, endAction);
    }
    public static IEnumerator EaseRoutine(float duration, EaseStyle easeStyle, System.Action<float, float> midAction)
    {
        return EaseRoutine(duration, easeStyle, null, midAction, null);
    }
    #endregion

}
