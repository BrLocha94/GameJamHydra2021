using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class TMPTweenMixerBehaviour : TweenMixerBehaviour<TMPTweenBehaviour, TMP_Text, TMPTweenMixerData>
{
    Vector3 m_ScaleDefaultValue => isSubMixer && mixerDataMode == ETweenMixerDataMode.Add ? Vector3.zero : Vector3.one;
    Vector3 m_PositionDefaultValue = Vector3.zero;
    Vector3 m_RotationDefaultValue = Vector3.zero;
    Vector2 m_PivotOffsetDefaultValue = Vector2.zero;
    Color m_ColorDefaultValue = Color.clear;

    TMPTweenMixerData currentData = new TMPTweenMixerData();

    private bool hasTextChanged = true;
    private TMPTweenTrack m_MasterTrack => masterTrack as TMPTweenTrack;
    private TMPTweenTrack m_Track => track as TMPTweenTrack;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnTextChanged(System.Object obj)
    {
        if (trackBinding == null) return;
        if (obj as TMP_Text == trackBinding)
            hasTextChanged = true;
    }

    protected override void OnFirstFrame()
    {
        base.OnFirstFrame();

        if (track == masterTrack)
        {
            m_ColorDefaultValue = trackBinding.color;
            trackBinding.ForceMeshUpdate(true);
            processFrameConditions.Add(new Condition(() => (trackBinding.textInfo != null && trackBinding.textInfo.characterCount > 0)));
        }
    }

    protected override void PreProcessTweenFrameMasterTrack()
    {
        base.PreProcessTweenFrameMasterTrack();
        if (hasTextChanged)
        {
            trackBinding.ForceMeshUpdate(true);
            TMP_TextInfo textInfo = trackBinding.textInfo;
            var meshInfo = textInfo.CopyMeshInfoVertexData();
            (masterTrack as TMPTweenTrack).cachedMeshInfo = meshInfo;
            hasTextChanged = false;
        }
    }

    protected override ref TMPTweenMixerData ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        int characterCount = trackBinding.textInfo.characterCount;

        currentData.color = new Color[characterCount];
        currentData.pivotOffset = new Vector2[characterCount];
        currentData.position = new Vector3[characterCount];
        currentData.rotation = new Vector3[characterCount];
        currentData.scale = new Vector3[characterCount];

        PlayableTweenUtils.DelayedTime<TMPTweenBehaviour>(playable, characterCount, onItemProcess);

        void onItemProcess((TMPTweenBehaviour[] inputs, float[] inputsTime, float[] inputsWeight, int itemIndex) data)
        {
            //Blended values
            Vector3 blendedPosition = Vector3.zero;
            Vector3 blendedRotation = Vector3.zero;
            Vector3 blendedScale = Vector3.zero;
            Vector2 blendedPivotOffset = Vector2.zero;
            Color blendedCharColor = Color.clear;

            //Weight values
            float positionWeight = 0f;
            float rotationWeight = 0f;
            float pivotWeight = 0f;
            float scaleWeight = 0f;
            float alphaWeight = 0f;

            for (int j = 0; j < data.inputs.Length; j++)
            {
                var input = data.inputs[j];
                var inputWeight = data.inputsWeight[j];
                var tweenProgress = data.inputsTime[j];

                if (input.m_RotationParameter.enable)
                {
                    rotationWeight += inputWeight;
                    blendedRotation += input.m_RotationParameter.GetValue(tweenProgress) * inputWeight;
                }
                if (input.m_ScaleParameter.enable)
                {
                    scaleWeight += inputWeight;
                    blendedScale += input.m_ScaleParameter.GetValue(tweenProgress) * inputWeight;
                }
                if (input.m_PositionParameter.enable)
                {
                    positionWeight += inputWeight;
                    blendedPosition += input.m_PositionParameter.GetValue(tweenProgress) * inputWeight;
                }
                if (input.m_PivotOffsetParameter.enable)
                {
                    pivotWeight += inputWeight;
                    blendedPivotOffset += input.m_PivotOffsetParameter.GetValue(tweenProgress) * inputWeight;
                }
                if (input.m_GradientParameter.enable)
                {
                    alphaWeight += inputWeight;
                    blendedCharColor += input.m_GradientParameter.GetValue(tweenProgress) * inputWeight;
                }
            }

            blendedScale += m_ScaleDefaultValue * (1f - scaleWeight);
            blendedPosition += m_PositionDefaultValue * (1f - positionWeight);
            blendedRotation += m_RotationDefaultValue * (1f - rotationWeight);
            blendedPivotOffset += m_PivotOffsetDefaultValue * (1f - pivotWeight);
            blendedCharColor += m_ColorDefaultValue * (1f - alphaWeight);

            currentData.scale[data.itemIndex] = blendedScale;
            currentData.position[data.itemIndex] = blendedPosition;
            currentData.rotation[data.itemIndex] = blendedRotation;
            currentData.pivotOffset[data.itemIndex] = blendedPivotOffset;
            currentData.color[data.itemIndex] = blendedCharColor;
        }
        return ref currentData;
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);

        if (layerIndex == -1 && m_MasterTrack.cachedMeshInfo != null && trackBinding != null)
        {
            trackBinding.UpdateVertexData();
        }
    }

    protected override void ApplyProcessedData(ref TMPTweenMixerData processedData)
    {
        TMP_TextInfo textInfo = trackBinding.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {

            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            // Skip characters that are not visible and thus have no geometry to manipulate.
            if (!charInfo.isVisible)
                continue;

            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            var newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            Color32 color = processedData.color[i].ToColor32();

            newVertexColors[vertexIndex + 0] = color;
            newVertexColors[vertexIndex + 1] = color;
            newVertexColors[vertexIndex + 2] = color;
            newVertexColors[vertexIndex + 3] = color;

            // Get the cached vertices of the mesh used by this text element (character or sprite).
            Vector3[] sourceVertices = m_MasterTrack.cachedMeshInfo[materialIndex].vertices;

            // Determine the center point of each character at the baseline.
            //Vector2 charMidBasline = new Vector2((sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2, charInfo.baseLine);
            // Determine the center point of each character.
            Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

            // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
            // This is needed so the matrix TRS is applied at the origin for each character.
            Vector3 offset = charMidBasline + processedData.pivotOffset[i];

            Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

            destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
            destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
            destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
            destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

            var matrix = Matrix4x4.TRS(processedData.position[i], Quaternion.Euler(processedData.rotation[i]), processedData.scale[i]);

            destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
            destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
            destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
            destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

            destinationVertices[vertexIndex + 0] += offset;
            destinationVertices[vertexIndex + 1] += offset;
            destinationVertices[vertexIndex + 2] += offset;
            destinationVertices[vertexIndex + 3] += offset;
        }

        trackBinding.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
    protected override ref TMPTweenMixerData AddMixTrack(ref TMPTweenMixerData currentData, ref TMPTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.color.Length; i++)
        {

            if (m_Track.m_ColorGradient) currentData.color[i].AddClamped(lastData.color[i]);
            if (m_Track.m_PivotOffset) currentData.pivotOffset[i] += lastData.pivotOffset[i];
            if (m_Track.m_Position) currentData.position[i] += lastData.position[i];
            if (m_Track.m_Rotation) currentData.rotation[i] += lastData.rotation[i];
            if (m_Track.m_Scale) currentData.scale[i] += lastData.scale[i];
        }

        return ref currentData;
    }
    protected override ref TMPTweenMixerData SubtracMixTrack(ref TMPTweenMixerData currentData, ref TMPTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.color.Length; i++)
        {
            if (m_Track.m_ColorGradient) currentData.color[i] -= lastData.color[i];
            if (m_Track.m_PivotOffset) currentData.pivotOffset[i] -= lastData.pivotOffset[i];
            if (m_Track.m_Position) currentData.position[i] -= lastData.position[i];
            if (m_Track.m_Rotation) currentData.rotation[i] -= lastData.rotation[i];
            if (m_Track.m_Scale) currentData.scale[i] -= lastData.scale[i];
        }

        return ref currentData;
    }
    protected override ref TMPTweenMixerData MultiplyMixTrack(ref TMPTweenMixerData currentData, ref TMPTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.color.Length; i++)
        {

            if (m_Track.m_ColorGradient) currentData.color[i] *= lastData.color[i];
            if (m_Track.m_PivotOffset) currentData.pivotOffset[i] = Vector2.Scale(currentData.pivotOffset[i], lastData.pivotOffset[i]);
            if (m_Track.m_Position) currentData.position[i] = Vector3.Scale(currentData.position[i], lastData.position[i]);
            if (m_Track.m_Rotation) currentData.rotation[i] = Vector3.Scale(currentData.rotation[i], lastData.rotation[i]);
            if (m_Track.m_Scale) currentData.scale[i] = Vector3.Scale(currentData.scale[i], lastData.scale[i]);
        }

        return ref currentData;
    }
    protected override ref TMPTweenMixerData AverageMixTrack(ref TMPTweenMixerData currentData, ref TMPTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.color.Length; i++)
        {
            if (m_Track.m_ColorGradient)
            {
                currentData.color[i] += lastData.color[i];
                currentData.color[i] *= 0.5f;
            }

            if (m_Track.m_PivotOffset)
            {
                currentData.pivotOffset[i] += lastData.pivotOffset[i];
                currentData.pivotOffset[i] *= 0.5f;
            }

            if (m_Track.m_Position)
            {
                currentData.position[i] += lastData.position[i];
                currentData.position[i] *= 0.5f;
            }

            if (m_Track.m_Rotation)
            {
                currentData.rotation[i] += lastData.rotation[i];
                currentData.rotation[i] *= 0.5f;
            }

            if (m_Track.m_Scale)
            {
                currentData.scale[i] += lastData.scale[i];
                currentData.scale[i] *= 0.5f;
            }
        }

        return ref currentData;
    }
    protected override ref TMPTweenMixerData OverrideMixTrack(ref TMPTweenMixerData currentData, ref TMPTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.color.Length; i++)
        {
            if (!m_Track.m_ColorGradient) currentData.color[i] = lastData.color[i];
            if (!m_Track.m_PivotOffset) currentData.pivotOffset[i] = lastData.pivotOffset[i];
            if (!m_Track.m_Position) currentData.position[i] = lastData.position[i];
            if (!m_Track.m_Rotation) currentData.rotation[i] = lastData.rotation[i];
            if (!m_Track.m_Scale) currentData.scale[i] = lastData.scale[i];
        }

        return ref currentData;
    }
}
