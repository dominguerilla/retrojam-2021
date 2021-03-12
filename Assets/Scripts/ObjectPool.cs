using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the controlled use of already-spawned Transforms.
/// Users are responsible for returning (using ObjectPool.AddObject()) their Transforms to the pool when finished.
/// 
/// Any Transform child object that starts with poolObjectPrefix will become part of the pool.
/// Objects disabled at the start of Scene will not be part of the pool.
/// All pool object GameObjects will be disabled at the start of the scene.
/// NOTE: Does NOT allow duplicates to be returned.
/// NOTE: NOT designed with thread safety in mind.
/// </summary>
[DisallowMultipleComponent]
public class ObjectPool : MonoBehaviour
{
    public string poolName {
        get;
        private set;
    }

    public string poolObjectPrefix = "";

    [SerializeField]
    List<Transform> pool = new List<Transform>();

    void Start()
    {
        if (pool == null || pool.Count == 0)
        {
            Transform[] childList = GetComponentsInChildren<Transform>();
            pool = new List<Transform>();

            foreach (Transform t in childList)
            {
                if (t.gameObject.name.StartsWith(poolObjectPrefix))
                {
                    t.gameObject.SetActive(false);
                    pool.Add(t);
                }
            }
        }
    }

    public Transform GetNextObject()
    {
        if (pool.Count < 1) return null;
        Transform obj = pool[0];
        pool.RemoveAt(0);
        return obj;
    }

    /// <summary>
    /// Disables the obj's GameObject and adds it back to the pool.
    /// </summary>
    /// <param name="obj"></param>
    public void AddObject(Transform obj)
    {
        if (!pool.Contains(obj))
        {
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    public int Count()
    {
        return pool.Count;
    }
}
