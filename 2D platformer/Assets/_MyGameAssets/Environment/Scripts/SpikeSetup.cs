using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSetup : MonoBehaviour
{
    [SerializeField] private float m_length;
    [SerializeField] private float m_height;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private BoxCollider2D m_collider;

    public void AdjustSize()
    {
        Sprite sprite = m_spriteRenderer.sprite;
        Texture2D texture = sprite.texture;
        float width = texture.width;
        float height = texture.height;

        texture.Reinitialize((int)m_length, (int)m_height);
        m_collider.bounds.extents.Set(m_length * 0.5f, m_height * 0.5f, 0);
    }
}