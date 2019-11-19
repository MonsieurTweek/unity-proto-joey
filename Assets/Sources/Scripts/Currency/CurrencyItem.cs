using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyItem : MonoBehaviour
{

    public GameManager GameManager { get => m_gameManager; set => m_gameManager = value; }
    private GameManager m_gameManager;

    public float m_yStartPosition;
    public float m_yStartPositionEdge;

    // Start is called before the first frame update
    void Start()
    {
        m_yStartPosition = transform.position.y;
        m_yStartPositionEdge = GameManager.edgeColliderManager.gameObject.transform.position.y;
    }
    
    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, (m_yStartPosition + GameManager.edgeColliderManager.gameObject.transform.position.y - m_yStartPositionEdge));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Die();
        }

    }

    private void Die()
    {
        GameManager.Currency += 1;
        Destroy(gameObject);
    }
}
