using System;
using UnityEngine;

namespace JazzDev.Noiser
{
    public class Noiser3D
    {

        public Vector3 Frequency
        {
            get
            {
                return this.frequency;
            }
            set
            {
                this.frequency = value;
            }
        }

        public Vector3 Amplitude
        {
            get
            {
                return this.amplitude;
            }
            set
            {
                this.amplitude = value;
            }
        }

        public Vector3 Noise { get => noise; }

        public Noiser3D()
        {
            this.noiseOffset.x = UnityEngine.Random.Range(0f, 32f);
            this.noiseOffset.y = UnityEngine.Random.Range(0f, 32f);
            this.noiseOffset.z = UnityEngine.Random.Range(0f, 32f);
        }

        public Vector3 Update(float deltaTime)
        {
            this.noise = default(Vector3);
            float noiseOffsetDeltaTimeX = deltaTime * this.frequency.x;
            float noiseOffsetDeltaTimeY = deltaTime * this.frequency.y;
            float noiseOffsetDeltaTimeZ = deltaTime * this.frequency.z;
            this.noiseOffset.x = this.noiseOffset.x + noiseOffsetDeltaTimeX;
            this.noiseOffset.y = this.noiseOffset.y + noiseOffsetDeltaTimeY;
            this.noiseOffset.z = this.noiseOffset.z + noiseOffsetDeltaTimeZ;
            this.noise.x = this.noise.x + Mathf.PerlinNoise(this.noiseOffset.x, 0f);
            this.noise.y = this.noise.y + Mathf.PerlinNoise(this.noiseOffset.y, 1f);
            this.noise.z = this.noise.z + Mathf.PerlinNoise(this.noiseOffset.z, 2f);
            this.noise -= Vector3.one * 0.5f;
            this.noise = new Vector3(this.noise.x * this.amplitude.x, this.noise.y * this.amplitude.y, this.noise.z * this.amplitude.z);
            return this.noise;
        }

        private Vector3 frequency;

        private Vector3 amplitude;

        private Vector3 noiseOffset;

        private Vector3 noise;

    }
    public class Noiser2D
    {

        public Vector2 Frequency
        {
            get
            {
                return this.frequency;
            }
            set
            {
                this.frequency = value;
            }
        }

        public Vector2 Amplitude
        {
            get
            {
                return this.amplitude;
            }
            set
            {
                this.amplitude = value;
            }
        }

        public Vector2 Noise { get => noise; }

        public Noiser2D()
        {
            this.noiseOffset.x = UnityEngine.Random.Range(0f, 32f);
            this.noiseOffset.y = UnityEngine.Random.Range(0f, 32f);
        }

        public Vector2 Update(float deltaTime)
        {
            this.noise = default(Vector2);

            float noiseOffsetDeltaTimeX = deltaTime * this.frequency.x;
            float noiseOffsetDeltaTimeY = deltaTime * this.frequency.y;

            this.noiseOffset.x = this.noiseOffset.x + noiseOffsetDeltaTimeX;
            this.noiseOffset.y = this.noiseOffset.y + noiseOffsetDeltaTimeY;

            this.noise.x = this.noise.x + Mathf.PerlinNoise(this.noiseOffset.x, 0f);
            this.noise.y = this.noise.y + Mathf.PerlinNoise(this.noiseOffset.y, 1f);

            this.noise -= Vector2.one * 0.5f;
            this.noise = new Vector2(this.noise.x * this.amplitude.x, this.noise.y * this.amplitude.y);
            return this.noise;
        }

        private Vector2 frequency;

        private Vector2 amplitude;

        private Vector2 noiseOffset;

        private Vector2 noise;

    }
    public class Noiser1D
    {

        public float Frequency
        {
            get
            {
                return this.frequency;
            }
            set
            {
                this.frequency = value;
            }
        }

        public float Amplitude
        {
            get
            {
                return this.amplitude;
            }
            set
            {
                this.amplitude = value;
            }
        }

        public float Noise { get => noise; }

        public Noiser1D()
        {
            this.noiseOffset = UnityEngine.Random.Range(0f, 32f);
        }

        public float Update(float deltaTime)
        {
            this.noise = default(float);

            float noiseOffsetDeltaTime = deltaTime * this.frequency;

            this.noiseOffset = this.noiseOffset + noiseOffsetDeltaTime;

            this.noise = this.noise + Mathf.PerlinNoise(this.noiseOffset, 0f);

            this.noise -= 0.5f;
            this.noise = this.noise * this.amplitude;
            return this.noise;
        }

        private float frequency;

        private float amplitude;

        private float noiseOffset;

        private float noise;

    }
    public static class NoiserGeneretor
    {
        public static float GetNoise(float perlinNoiseTime, float amplitude = 1, float valueCenter = 0.5f, float perlinNoiseOffset = 0)
        {
            float noise = Mathf.PerlinNoise(perlinNoiseTime, perlinNoiseOffset);
            noise -= 1 * valueCenter;
            noise *= amplitude;
            return noise;
        }
        public static Vector2 GetNoise(float perlinNoiseTime, float amplitude, float valueCenter = 0.5f, float perlinNoiseOffsetX = 0, float perlinNoiseOffsetY = 1)
        {
            float x = GetNoise(perlinNoiseTime, amplitude, valueCenter, perlinNoiseOffsetX);
            float y = GetNoise(perlinNoiseTime, amplitude, valueCenter, perlinNoiseOffsetY);
            return new Vector2(x, y);
        }
        public static Vector3 GetNoise(float perlinNoiseTime, float amplitude, float valueCenter = 0.5f, float perlinNoiseOffsetX = 0, float perlinNoiseOffsetY = 1, float perlinNoiseOffsetZ = 2)
        {
            float x = GetNoise(perlinNoiseTime, amplitude, valueCenter, perlinNoiseOffsetX);
            float y = GetNoise(perlinNoiseTime, amplitude, valueCenter, perlinNoiseOffsetY);
            float z = GetNoise(perlinNoiseTime, amplitude, valueCenter, perlinNoiseOffsetZ);
            return new Vector3(x, y, z);
        }
    }
}