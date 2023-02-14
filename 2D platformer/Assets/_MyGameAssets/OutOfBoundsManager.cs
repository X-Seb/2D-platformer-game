using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Platform") && collision.gameObject.isStatic) ||
            (collision.CompareTag("Enemy") && collision.gameObject.isStatic) ||
            (collision.CompareTag("Ground") && collision.gameObject.isStatic) ||
            (collision.CompareTag("decoration") && collision.gameObject.isStatic) ||
            (collision.CompareTag("Collectible") && collision.gameObject.isStatic) ||
            (collision.CompareTag("Acid") && collision.gameObject.isStatic))
        {
            collision.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Platform") && collision.gameObject.isStatic) ||
            (collision.CompareTag("Enemy") && collision.gameObject.isStatic) ||
            (collision.CompareTag("Ground") && collision.gameObject.isStatic) ||
            (collision.CompareTag("decoration") && collision.gameObject.isStatic) ||
            (collision.CompareTag("Collectible") && collision.gameObject.isStatic) ||
            (collision.CompareTag("Acid") && collision.gameObject.isStatic))
        {
            collision.gameObject.SetActive(false);
        }
    }
}