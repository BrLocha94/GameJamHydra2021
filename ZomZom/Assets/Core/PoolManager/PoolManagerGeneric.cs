using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace JazzDev.Pool
{
    public abstract class PoolManagerGeneric<T> : MonoBehaviour where T : PoolObject
    {
        [System.Serializable]
        internal class Delay<U>
        {
            public U Clone;
            public float LifeTime;
        }

        [SerializeField] [UnityEngine.Serialization.FormerlySerializedAs("Prefab")] protected T m_prefab;
        /// <summary>Should this pool preload some clones?</summary>
        [SerializeField] protected int Preload;

        /// <summary>Should this pool have a maximum amount of spawnable clones?</summary>
        [SerializeField] protected int Capacity;

        /// <summary>Should this pool be marked as DontDestroyOnLoad?</summary>
        [SerializeField] protected bool Persist;

        protected List<T> spawnedClonesList;

        protected List<T> despawnedClones;

        /// <summary>All the delayed destruction objects.</summary>
        [SerializeField] private List<Delay<T>> delays = new List<Delay<T>>();

        protected virtual void Awake()
        {
            spawnedClonesList = new List<T>(Capacity);
            despawnedClones = new List<T>(Capacity);

            Preload = Mathf.Clamp(Preload, 0, Capacity);

            PreloadAll();

            if (Persist) { DontDestroyOnLoad(this); }
        }

        protected virtual void PreloadOneMore()
        {
            if (m_prefab != null)
            {
                // Create clone
                var clone = CreateClone(Vector3.zero, Quaternion.identity, null, false);

                // Add clone to despawned list
                despawnedClones.Add(clone);

                // Deactivate it
                clone.gameObject.SetActive(false);

                // Move it under this GO
                clone.transform.SetParent(transform, false);
            }
        }

        protected virtual void PreloadAll()
        {
            if (Preload > 0)
            {
                if (m_prefab != null)
                {
                    for (var i = Total; i < Preload; i++)
                    {
                        PreloadOneMore();
                    }
                }
            }
        }

        protected virtual T CreateClone(Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays)
        {
            if (parent != null)
            {
                if (worldPositionStays == true)
                {
                    //position = parent.InverseTransformPoint(position);
                    //rotation = Quaternion.Inverse(parent.rotation) * rotation;
                }
                else
                {
                    position = parent.TransformPoint(position);
                    rotation = parent.rotation * rotation;
                }
            }

            var clone = Instantiate(m_prefab, position, rotation, parent);

            clone.name = m_prefab.name;

            return clone;
        }

        protected virtual bool TrySpawn(Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays, ref T clone)
        {
            if (m_prefab != null)
            {
                // Spawn a previously despanwed/preloaded clone?
                for (var i = despawnedClones.Count - 1; i >= 0; i--)
                {
                    clone = despawnedClones[i];

                    despawnedClones.RemoveAt(i);

                    if (clone != null)
                    {
                        SpawnClone(clone, position, rotation, parent, worldPositionStays);
                        return true;
                    }
                }

                // Make a new clone?
                if (Capacity <= 0 || Total < Capacity)
                {
                    clone = CreateClone(position, rotation, parent, worldPositionStays);

                    spawnedClonesList.Add(clone);

                    // Activate
                    clone.gameObject.SetActive(true);
                    clone.OnSpawn();
                    return true;
                }

            }
            return false;
        }

        protected virtual void SpawnClone(T clone, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays)
        {
            spawnedClonesList.Add(clone);

            // Update transform
            var cloneTransform = clone.transform;

            cloneTransform.SetParent(parent, false);

            // Make sure it's in the current scene
            if (parent == null)
            {
                SceneManager.MoveGameObjectToScene(clone.gameObject, SceneManager.GetActiveScene());
            }

            if (worldPositionStays == true)
            {
                cloneTransform.position = position;
                cloneTransform.rotation = rotation;
            }
            else
            {
                cloneTransform.localPosition = position;
                cloneTransform.localRotation = rotation;
            }

            // Activate
            clone.gameObject.SetActive(true);
            clone.OnSpawn();
        }

        /// <summary>This will either instantly despawn the specified gameObject, or delay despawn it after t seconds.</summary>

        protected virtual void TryDespawn(T clone)
        {
            if (spawnedClonesList.Remove(clone) == true)
            {
                DespawnNow(clone);
            }
        }

        protected virtual void DespawnNow(T clone, bool register = true)
        {
            // Add clone to despawned list
            if (register == true)
            {
                despawnedClones.Add(clone);
            }

            // Deactivate it
            clone.gameObject.SetActive(false);
            clone.OnDespawn();

            // Move it under this GO
            clone.transform.SetParent(transform, false);
        }

        /// <summary>Returns the amount of spawned clones.</summary>
        public int Spawned
        {
            get
            {
                return spawnedClonesList.Count;
            }
        }

        /// <summary>Returns the amount of despawned clones.</summary>
        public int Despawned
        {
            get
            {
                return despawnedClones.Count;
            }
        }

        /// <summary>Returns the total amount of spawned and despawned clones.</summary>
        public int Total
        {
            get
            {
                return Spawned + Despawned;
            }
        }

        public virtual void Despawn(T clone, float t = 0.0f)
        {
            if (clone != null)
            {
                // Delay the despawn?
                if (t > 0.0f)
                {
                    DespawnWithDelay(clone, t);
                }
                // Despawn now?
                else
                {
                    TryDespawn(clone);

                    // If this clone was marked for delayed despawn, remove it
                    for (var i = delays.Count - 1; i >= 0; i--)
                    {
                        var delay = delays[i];

                        if (delay.Clone == clone)
                        {
                            delays.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void DespawnWithDelay(T clone, float t)
        {
            // If this object is already marked for delayed despawn, update the time and return
            for (var i = delays.Count - 1; i >= 0; i--)
            {
                var delay = delays[i];

                if (delay.Clone == clone)
                {
                    if (t < delay.LifeTime)
                    {
                        delay.LifeTime = t;
                    }

                    return;
                }
            }

            // Create delay
            var newDelay = ClassPool<Delay<T>>.Spawn() ?? new Delay<T>();

            newDelay.Clone = clone;
            newDelay.LifeTime = t;

            delays.Add(newDelay);
        }

        public virtual T Spawn(Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = true)
        {
            var clone = default(T);

            TrySpawn(position, rotation, parent, worldPositionStays, ref clone);

            return clone;
        }

        public virtual void Spawn()
        {
            Spawn(transform.position, transform.rotation);
        }

        protected virtual void Update()
		{
			// Decay the life of all delayed destruction calls
			for (var i = delays.Count - 1; i >= 0; i--)
			{
				var delay = delays[i];

				delay.LifeTime -= Time.deltaTime;

				// Skip to next one?
				if (delay.LifeTime > 0.0f)
				{
					continue;
				}

				// Remove and pool delay
				delays.RemoveAt(i); ClassPool<Delay<T>>.Despawn(delay);

				// Finally despawn it after delay
				if (delay.Clone != null)
				{
					Despawn(delay.Clone);
				}
			}
		}

    }
}
