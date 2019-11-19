using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObjectBehaviour : MonoBehaviour, IMovementBehaviour
{


    private float m_minBound;
    private float m_maxBound;

    private bool m_moveLeft;

    public float Speed { get => m_speed; set => m_speed = value; }
    private float m_speed;

    private Vector3 m_colliderBoundsSize;


    // Start is called before the first frame update
    private void Start()
    {
        m_moveLeft = true;
        m_colliderBoundsSize = GetComponent<Collider2D>().bounds.size;
        m_minBound = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x + (m_colliderBoundsSize.x / 2);
        m_maxBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x - (m_colliderBoundsSize.x / 2);
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public void SetThreshold(float trheshold)
    {
        // Do nothing
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if(m_moveLeft == true)
        {
            transform.position -= transform.right * Time.deltaTime * Speed;
            
        } else
        {
            transform.position += transform.right * Time.deltaTime * Speed;
        }

        if (transform.position.x <= m_minBound || transform.position.x >= m_maxBound)
        {
            m_moveLeft = !m_moveLeft;
        }


    }
}
