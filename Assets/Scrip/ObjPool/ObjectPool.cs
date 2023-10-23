using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pooledObjects;
    private int amountToPool = 30;
    [SerializeField]
    private GameObject objectContainer;
    [SerializeField]
    private GameObject objectToPool;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool,objectContainer.transform);
            obj.SetActive(false);
            obj.name = i.ToString();
            pooledObjects.Add(obj);
        }

    }

    public void DeActiveObject(int i)
    {
        if (pooledObjects[i].activeInHierarchy)
        {
            pooledObjects[i].SetActive(false);
            pooledObjects[i].transform.parent = objectContainer.transform;
        }
    }

    public GameObject GetPooledGameObj()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
