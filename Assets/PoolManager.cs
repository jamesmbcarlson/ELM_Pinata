using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<Transform> pool;
    public GameObject prefab;

    private void Awake()
    {
        pool = new List<Transform>();

        foreach(Transform child in GetComponentInChildren<Transform>())
        {
            pool.Add(child);
        }
    }

    public GameObject GetObject()
    {
        // return first available object
        foreach (Transform child in pool)
        {
            if(!child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }

        // OR instantiate new object to return
        GameObject newChild = Instantiate(prefab, transform);
        pool.Add(newChild.transform);
        return newChild;
    }
}
