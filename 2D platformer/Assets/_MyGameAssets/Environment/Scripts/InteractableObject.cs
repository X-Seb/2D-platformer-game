using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] Type m_type;

    private enum Type
    {
        forever,
        onOff,
        timer
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player triggered: " + gameObject.name);
        }
    }
}
