using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTap : MonoBehaviour, IMovementBehaviour
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
        bool isMoving = false;
        if(Input.touchCount > 0) 
        {
            Touch touch = Input.GetTouch(0);
            switch(touch.phase) 
            {
                case TouchPhase.Began:
                case TouchPhase.Moved:
                    isMoving = true;
                    break;
            }
        }
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#else
        bool isMoving = Input.GetMouseButton(0);
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif

        // Handle screen touches.
        if (isMoving == true)
        {
            float newPositionX = Mathf.Min(m_maxBound, Mathf.Max(m_minBound, pos.x));
            Vector3 newPosition = new Vector3(newPositionX, transform.position.y, 0);
            float distance = Vector2.Distance(transform.position, newPosition);

            if(distance >= m_distanceThreshold)
            {
                m_player.IsMoving = true;
            } else
            {
                m_player.IsMoving = false;
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, m_player.moveForce * Time.deltaTime);

            if (newPositionX < transform.position.x)
            {
                m_player.FlipPlayer(PlayerSkinController.Direction.Left);
            }
            else
            {
                m_player.FlipPlayer(PlayerSkinController.Direction.Right);
            }
        } else
        {
            m_player.IsMoving = false;
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
