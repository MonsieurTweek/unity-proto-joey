using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToTap : MonoBehaviour, IMovementBehaviour
{
    private Player m_player;

    private float m_minBound;
    private float m_maxBound;

    private float m_distanceThreshold = 0.5f;

    public float Speed => throw new System.NotImplementedException();

    private void Start()
    {
        m_player = GetComponent<Player>();
        m_minBound = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
        m_maxBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if(m_player.gameManager.State != GameManager.GameState.Play)
        {
            return;
        }

#if !UNITY_EDITOR
        bool isTouching = Input.touchCount > 0;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#else
        bool isTouching = Input.GetMouseButton(0);
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif

        // Handle screen touches.
        if (isTouching == true)
        {
            float newPositionX = Mathf.Min(m_maxBound, Mathf.Max(m_minBound, pos.x));
            Vector3 newPosition = new Vector3(newPositionX, transform.position.y, 0);
            float distance = Vector2.Distance(transform.position, newPosition);
            if (distance >= m_distanceThreshold)
            {
                transform.position = newPosition;
            }
        }

    }

    public void SetDistanceThreshold(float threshold)
    {
        m_distanceThreshold = threshold;
    }

    public void SetSpeed(float speed)
    {
        m_player.moveForce = speed;
    }

    public void SetThreshold(float threshold)
    {
        m_distanceThreshold = threshold;
    }
}
