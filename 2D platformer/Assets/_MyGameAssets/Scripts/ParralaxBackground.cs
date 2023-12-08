using UnityEngine;
using System.Collections;

public class ParralaxBackground : MonoBehaviour
{
    [Header("Setup:")]
    [SerializeField] private GameObject m_player;
    [Header("Modify the speed of the object :")]
    [SerializeField] private float m_xMultiplier;
    [SerializeField] private float m_yMultiplier;
    [Header("Should the object scroll forever on x or y axis:")]
    [SerializeField] private bool m_infinityX;
    [SerializeField] private bool m_infinityY;
    [Header("For reference only:")]
    [SerializeField] private Vector3 m_lastPlayerPos;
    [SerializeField] private Vector3 m_deltaMovement;
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private Texture2D m_texture;
    [SerializeField] private float m_textureSizeX;
    [SerializeField] private float m_textureSizeY;


    private void Start()
    {
        m_lastPlayerPos = m_player.transform.position;
        m_sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        m_texture = m_sprite.texture;
        m_textureSizeX = (m_texture.width / m_sprite.pixelsPerUnit) * transform.localScale.x;
        m_textureSizeY = (m_texture.height / m_sprite.pixelsPerUnit) * transform.localScale.y;
    }

    private void LateUpdate()
    {
        if (GameManager.instance.GetState() != GameManager.GameState.lose)
        {
            // Figure out how much you've moved and move the object accordingly * the multipliers
            m_deltaMovement = m_player.transform.position - m_lastPlayerPos;
            m_deltaMovement.y *= m_yMultiplier;
            m_deltaMovement.x *= m_xMultiplier;
            transform.position += m_deltaMovement;
            m_lastPlayerPos = m_player.transform.position;

            if (m_infinityX && Mathf.Abs(m_player.transform.position.x - transform.position.x) >= m_textureSizeX)
            {
                float offsetPosX = (m_player.transform.position.x - transform.position.x) % m_textureSizeX;
                transform.position = new Vector3(m_player.transform.position.x + offsetPosX, transform.position.y, 0);
            }

            if (m_infinityY && Mathf.Abs(m_player.transform.position.y - transform.position.y) >= m_textureSizeY)
            {
                float offsetPosY = (m_player.transform.position.y - transform.position.y) % m_textureSizeY;
                transform.position = new Vector3(transform.position.x, m_player.transform.position.y + offsetPosY, 0);
            }
        }
    }
}