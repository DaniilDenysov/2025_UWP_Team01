using System;
using System.Collections.Generic;
using TowerDeffence.Utilities;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Zenject.SpaceFighter;

namespace TowerDeffence.ObjectPools
{
    public abstract class ObjectPoolWrapper<T> : MonoInstaller, IObjectPool<T> where T : MonoBehaviour
    {

        [SerializeField] protected PoolObject[] poolObjects;

        private Dictionary<string, ObjectPool<T>> mappings = new Dictionary<string, ObjectPool<T>>();
        private Dictionary<T, string> reverseMappings = new Dictionary<T, string>();
        private Dictionary<T, string> buffer = new Dictionary<T, string>();

        [System.Serializable]
        public class PoolObject
        {
            public T Prefab;
            [Range(0, 100000)] public int DefaultCapacity = 100;
            [Range(0, 100000)] public int MaxCapacity = 200;
        }

        private void Awake()
        {
            InitializePools();
        }

        public override void InstallBindings()
        {
            Container.Bind<IObjectPool<T>>().To<ObjectPoolWrapper<T>>().FromInstance(this).AsSingle().NonLazy();
        }

        private void InitializePools()
        {
            foreach (var poolObj in poolObjects)
            {
                if (poolObj.Prefab == null)
                {
                    Debug.LogWarning("Prefab is null in PoolObject configuration.");
                    continue;
                }

                if (!reverseMappings.ContainsKey(poolObj.Prefab))
                {
                    var guid = Guid.NewGuid().ToString();
                    var newPool = new ObjectPool<T>(
                        createFunc: () =>
                        {
                            var obj = Container.InstantiatePrefab(poolObj.Prefab.gameObject);
                          // Container.Inject(obj);
                            return obj.GetComponent<T>();
                        },
                        actionOnGet: OnTakenFromPool,
                        actionOnRelease: OnReturnedToPool,
                        actionOnDestroy: OnDestroyPoolObject,
                        collectionCheck: true,
                        defaultCapacity: poolObj.DefaultCapacity,
                        maxSize: poolObj.MaxCapacity
                    );
                    mappings.Add(guid, newPool);
                    reverseMappings.Add(poolObj.Prefab, guid);
                }
                else
                {
                    DebugUtility.PrintWarning($"Pool for {poolObj.ToString()} is already initialized.");
                }
            }
        }

        public virtual void OnDestroyPoolObject(T obj)
        {
            Destroy(obj.gameObject);
        }

        public virtual void OnTakenFromPool(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        public virtual void OnReturnedToPool(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        public T Get(T prefab)
        {
            if (prefab == null || prefab.Equals(default(T)))
            {
                DebugUtility.PrintError("Provided prefab is null or default.");
                return null;
            }

            if (reverseMappings.TryGetValue(prefab, out string guid))
            {
                if (mappings.TryGetValue(guid, out ObjectPool<T> pool))
                {
                    T instance = pool.Get();

                    int safetyCounter = 0;
                    while (instance == null && safetyCounter < 10)
                    {
                        instance = pool.Get();
                        safetyCounter++;
                    }

                    if (instance == null)
                    {
                        DebugUtility.PrintWarning($"Unable to retrieve a valid instance from pool for GUID {guid} after several attempts.");
                        return null;
                    }

                    buffer.Add(instance, guid);
                    return instance;
                }
                else
                {
                    DebugUtility.PrintWarning($"No pool found for GUID: {guid}");
                }
            }
            else
            {
                DebugUtility.PrintError($"Failed to get GUID for prefab: {prefab.name}");
            }

            return null;
        }


        public void Release(T obj)
        {
            if (obj == null) return;
            if (buffer.TryGetValue(obj, out string guid))
            {
                if (mappings.TryGetValue(guid, out ObjectPool<T> pool))
                {
                    buffer.Remove(obj);
                    pool.Release(obj);
                }
                else
                {
                    DebugUtility.PrintWarning("No pool found for GUID: " + guid);
                }
            }
            else
            {
                DebugUtility.PrintWarning("Failed to get GUID for object: " + obj.name);
            }
        }

        public T GetObject(T prefab)
        {
            return Get(prefab);
        }

        public void ReleaseObject(T prefab)
        {
            Release(prefab);
        }
    }

}
