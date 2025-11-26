using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPoolingSystem : MonoBehaviour
{
    [Header("Pooling")]
    public GameObject prefab;
    public int initialPoolSize = 10;

    protected List<GameObject> pool = new List<GameObject>();

    [Space]
    [SerializeField] private UnityEvent<GameObject> onCreate = new UnityEvent<GameObject>();

    protected virtual Transform PoolParentTransform => this.transform;

    protected GameObject CreateNewObject()
    {
        GameObject newObj = Instantiate(prefab, PoolParentTransform);
        newObj.name = prefab.name + "_Pooled";
        onCreate?.Invoke(newObj);
        return newObj;
    }


    // ========================================
    // Get Object
    // ========================================
    protected GameObject GetPooledObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeSelf)
                return obj;
        }

        // No free object → expand pool
        var newObj = CreateNewObject();
        pool.Add(newObj);
        return newObj;
    }

    // ========================================
    // Clear Objects (disable only)
    // ========================================
    public virtual void ClearPool()
    {
        foreach (var obj in pool)
            obj.SetActive(false);
    }


    // ========================================
    // Destroy All Objects (Editor Safe)
    // ========================================
    [ContextMenu("Destroy All Pooled Objects")]
    public virtual void DestroyAllPooled()
    {
        for (int i = pool.Count - 1; i >= 0; i--)
        {
            var obj = pool[i];
#if UNITY_EDITOR
            UnityEditor.Undo.DestroyObjectImmediate(obj);
#else
            Destroy(obj);
#endif
        }

        pool.Clear();
    }
}
