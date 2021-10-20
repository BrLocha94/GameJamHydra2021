using System;
using UnityEngine;

[Serializable]
public class CameraMatrix2TweenBehaviour: PlayableTweenBehaviourBase
{
    public Vector3TweenParameter position = new Vector3TweenParameter(Vector3.zero, Vector3.zero, false);
    public Vector3TweenParameter rotation = new Vector3TweenParameter(Vector3.zero, Vector3.zero, false);
    public Vector3TweenParameter scale = new Vector3TweenParameter(Vector3.zero, Vector3.zero, false);
    public Vector2TweenParameter projectionPosition = new Vector2TweenParameter(Vector2.zero, Vector2.zero, false);
    public Vector3TweenParameter projectionRotation = new Vector3TweenParameter(Vector3.zero, Vector3.zero, false);
    public FloatTweenParameter fieldOfView = new FloatTweenParameter(60,0, false);
}