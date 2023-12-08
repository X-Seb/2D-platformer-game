using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Header("Important:")]
    [SerializeField] private GameObject m_object;
    [SerializeField] private int m_numberOfObjects;
    [SerializeField] private List<GameObject> m_objects;
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log("Create fireballs");

        GameObject tmp;
        m_objects = new List<GameObject>();

        for (int i = 0; i < m_numberOfObjects; i++)
        {
            tmp = Instantiate(m_object, Vector3.zero, Quaternion.identity, gameObject.transform);
            tmp.SetActive(false);
            m_objects.Add(tmp);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < m_objects.Count; i++)
        {
            if (!m_objects[i].activeInHierarchy)
            {
                return m_objects[i];
            }
        }
        return null;
    }
}