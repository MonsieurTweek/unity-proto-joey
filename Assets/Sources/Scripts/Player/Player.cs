using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    Rigidbody2D m_body;

    public float moveForce = 5;

    // Rigidbody settings
    public float mass;
    public float linearDrag;
    public float gravityScale;

    // Collider settings
    public float minSize = 32;
    public float m_yStartPosition;

    [HideInInspector]
    public float currentSize;
    [HideInInspector]
    public float startSize;
    [HideInInspector]
    public float stepSize;
    private float m_startBoundSize;
    private float m_colliderScaleY;

    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public bool isAlive = true;

    private bool m_canDie = true;
    public bool CanDie { get => m_canDie; set => m_canDie = value; }

    public AnimationCurve curve;

    private IMovementBehaviour m_inputMovementBehaviour;
    public IMovementBehaviour InputMovementBehaviour { get => m_inputMovementBehaviour; set => m_inputMovementBehaviour = value; }

    private Animator m_animator;


    private bool m_isMoving;
    public bool IsMoving
    {
        get { return m_isMoving; }
        set
        {
            if (isAlive == true)
            {
                if(m_isMoving != value)
                {
                    m_animator.SetBool("IsMoving", value);
                    m_isMoving = value;
                }
            }
        }
    }

    private PlayerSkinController m_playerSkinController;
    public PlayerSkinController PlayerSkinController
    {
        get
        {
            return m_playerSkinController;
        }
        set
        {
            m_playerSkinController = value;
            m_animator = m_playerSkinController.GetComponent<Animator>();
        }
    }


    // Start is called before the first frame update
    private void Awake()
    {
        InputMovementBehaviour = GetComponent<IMovementBehaviour>();
        m_body = GetComponent<Rigidbody2D>();
        mass = m_body.mass;
        linearDrag = m_body.drag;
        gravityScale = m_body.gravityScale;

        startSize = gameObject.transform.localScale.x;
        currentSize = startSize;

        m_colliderScaleY = 1f / (transform.localScale.y / (Mathf.Round(GetComponent<Collider2D>().bounds.size.y * 100f) / 100f));

        m_isMoving = false;

    }

    public void Die()
    {
        if(isAlive == true && CanDie == true)
        {
            isAlive = false;
            PlayerSkinController.Die();
            m_animator.SetBool("IsAlive", false);
            //m_animator.Play("Dying");
            gameManager.GameOver();
        }
    }

    public void UpdateRigidbodySettings()
    {
        m_body = GetComponent<Rigidbody2D>();
        m_body.mass = mass;
        m_body.drag = linearDrag;
        m_body.gravityScale = gravityScale;

        // Set the player position at the top of the edge collider
        UpdateStartPositionY();

    }

    private void UpdateStartPositionY()
    {
        // Set the player position at the top of the edge collider
        float halfBoundSizeY = ((transform.localScale.y * m_colliderScaleY) / 2) - GetComponent<Collider2D>().bounds.center.y;
        float halfBoundSizeEdgeY = (gameManager.edgeColliderManager.GetComponent<Collider2D>().bounds.size).y / 2;
        transform.position = new Vector3(transform.position.x, halfBoundSizeY + halfBoundSizeEdgeY + gameManager.edgeColliderManager.gameObject.transform.position.y);
        m_yStartPosition = transform.position.y;
    }

    /* Old version with edge collider position */
    /*
    public void UpdateScale(float position)
    {
        if(isAlive == true)
        {
            float t = ((position - gameManager.edgeColliderManager.startPosition) / (gameManager.edgeColliderManager.maxTop - gameManager.edgeColliderManager.startPosition));
            t = curve.Evaluate(t);
            float newValue = Mathf.Lerp(startSize, minSize, t);
            gameObject.transform.localScale = new Vector2(newValue, newValue);
        }
    }
    */

    public void UpdateScale()
    {
        if (isAlive == true)
        {
            if(currentSize > minSize)
            {
                float previousSize = currentSize;
                float newSize = currentSize - stepSize;
                gameObject.transform.localScale = new Vector2(newSize, newSize);
                currentSize = gameObject.transform.localScale.x;
                float offset = (stepSize * m_colliderScaleY / 2);
                m_yStartPosition = m_yStartPosition - offset;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, (m_yStartPosition + gameManager.edgeColliderManager.gameObject.transform.position.y - gameManager.edgeColliderManager.startPosition));
    }

    public void SetDistanceThreshold(float distance)
    {
        InputMovementBehaviour.SetThreshold(distance);
    }

    public void FlipPlayer(PlayerSkinController.Direction facingDirection)
    {
        PlayerSkinController.FacingDirection = facingDirection;
    }

}
