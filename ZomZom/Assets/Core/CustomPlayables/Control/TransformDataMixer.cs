using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class TransformDataMixer : MonoBehaviour
{
    public Transform target;
    public enum EUpdadeMode { Update, LateUpdade }
    public enum EMixMode { Add, Avarage }

    [SerializeField] private EUpdadeMode updateMode = EUpdadeMode.LateUpdade;

    [SerializeField] private EMixMode mixMode = EMixMode.Avarage;

    private List<Vector3> positionDataBuffer = new List<Vector3>();
    private List<Quaternion> rotationDataBuffer = new List<Quaternion>();
    private List<Vector3> scaleDataBuffer = new List<Vector3>();

    public void AddPositionData(Vector3 position)
    {
        positionDataBuffer.Add(position);
    }
    public void AddRotationData(Quaternion rotation)
    {
        rotationDataBuffer.Add(rotation);
    }
    public void AddScaleData(Vector3 scale)
    {
        scaleDataBuffer.Add(scale);
    }

    private void ProcessBuffer()
    {
        if (target == null) return;

        int posBufferSize = positionDataBuffer.Count;
        int rotBufferSize = rotationDataBuffer.Count;
        int scaleBufferSize = scaleDataBuffer.Count;

        if (posBufferSize > 0)
        {
            Vector3 position = positionDataBuffer[0];

            for (int i = 1; i < positionDataBuffer.Count; i++)
            {
                position += positionDataBuffer[i];
            }

            if (mixMode == EMixMode.Avarage)
            {
                position /= posBufferSize;
            }

            target.localPosition = position;

            positionDataBuffer.Clear();
        }

        if (rotBufferSize > 0)
        {
            Quaternion rotation = rotationDataBuffer[0];

            if (mixMode == EMixMode.Add)
            {
                for (int i = 1; i < rotationDataBuffer.Count; i++)
                {
                    rotation*= rotationDataBuffer[i];
                }

                target.localRotation = rotation;
            }
            else if (mixMode == EMixMode.Avarage)
            {
                Quaternion q_average = Quaternion.identity;

                float averageWeight = 1f / rotationDataBuffer.Count;

                for (int i = 0; i < rotationDataBuffer.Count; i++)
                {
                    Quaternion q = rotationDataBuffer[i];

                    q_average *= Quaternion.Slerp(Quaternion.identity, q, averageWeight);
                }

                // output rotation - attach to some object, or whatever
                target.localRotation  = q_average;
            }

            rotationDataBuffer.Clear();
        }

        if (scaleBufferSize > 0)
        {
            Vector3 scale = scaleDataBuffer[0];

            for (int i = 1; i < scaleDataBuffer.Count; i++)
            {
                scale += scaleDataBuffer[i];
            }

            if (mixMode == EMixMode.Avarage)
            {
                scale /= scaleBufferSize;
            }

            target.localScale = scale;

            scaleDataBuffer.Clear();
        }
    }

    private void Update()
    {
        if (updateMode == EUpdadeMode.Update) ProcessBuffer();
    }
    private void LateUpdate()
    {
        if (updateMode == EUpdadeMode.LateUpdade) ProcessBuffer();
    }
}
