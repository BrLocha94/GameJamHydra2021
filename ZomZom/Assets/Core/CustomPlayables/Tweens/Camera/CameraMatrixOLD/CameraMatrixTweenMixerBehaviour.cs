using System;
using UnityEngine;
using UnityEngine.Playables;

public class CameraMatrixTweenMixerBehaviour : PRSMixerBehaviour<Camera, PRSTweenBehaviour>
{
    protected Vector3 m_CalculatedPosition;
    protected Vector3 m_CalculatedRotation;
    protected Vector3 m_CalculatedScale;
    protected override PRSTweenTrack<PRSTweenBehaviour, Camera, TransformTweenMixerData> m_Track
    {
        get
        {
            return track as CameraMatrixTweenTrack;
        }
    }
    protected override PRSTweenTrack<PRSTweenBehaviour, Camera, TransformTweenMixerData> m_MasterTrack
    {
        get
        {
            return masterTrack as CameraMatrixTweenTrack;
        }
    }
    protected override void PreProcessTweenFrameMasterTrack()
    {
        base.PreProcessTweenFrameMasterTrack();
        trackBinding.ResetWorldToCameraMatrix();
    }
    protected override void ResetToDefaultValues()
    {
        trackBinding.ResetWorldToCameraMatrix();
    }
    protected override void SetPosition(Vector3 pos) => m_CalculatedPosition = pos;
    protected override void SetRotation(Vector3 rot) => m_CalculatedRotation = rot;
    protected override void SetScale(Vector3 scale) => m_CalculatedScale = scale;
    protected override void ApplyProcessedData(ref TransformTweenMixerData processedData)
    {
        base.ApplyProcessedData(ref processedData);

        var transformationMatrix = Matrix4x4.Rotate(Quaternion.Euler(m_CalculatedRotation));
        m_CalculatedScale += Vector3.one;
        m_CalculatedScale = Vector3.Scale(m_CalculatedScale, new Vector3(1, 1, -1));
        if (Vector3.Magnitude(m_CalculatedScale) > 0) transformationMatrix *= Matrix4x4.Scale(m_CalculatedScale);
        else transformationMatrix *= Matrix4x4.Scale(new Vector3(1, 1, -1));

        transformationMatrix *= Matrix4x4.Translate(m_CalculatedPosition);


        if (transformationMatrix.ValidTRS())
        {
            trackBinding.worldToCameraMatrix = transformationMatrix * trackBinding.transform.worldToLocalMatrix;
        }
        else
        {
            trackBinding.ResetWorldToCameraMatrix();
        }
    }
    protected override void GetDefaultValues() { }
}