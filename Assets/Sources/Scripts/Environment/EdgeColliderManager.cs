using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeColliderManager : MonoBehaviour
{

    [HideInInspector]
    public GameManager gameManager;

    public float speed = 0.1f;
    public float maxTop = 3f;

    public float startPosition;
    private float m_minTop;

    public bool IsAscending { get => m_isAscending; set => m_isAscending = value; }
    private bool m_isAscending;

    public bool CanMove { get => m_canMove; set => m_canMove = value; }
    private bool m_canMove;

    public Sprite spriteSignUp;
    public Sprite spriteSignDown;
    public Sprite spriteSignBoss;
    public GameObject sign;

    public enum SignPostState
    {
        Up,
        Down,
        Boss
    }
    private SignPostState m_signState;
    public SignPostState SignState
    {
        get { return m_signState; }
        set
        {
            m_signState = value;
            switch(m_signState)
            {
                case SignPostState.Up:
                    sign.GetComponent<SpriteRenderer>().sprite = spriteSignUp;
                    break;

                case SignPostState.Down:
                    sign.GetComponent<SpriteRenderer>().sprite = spriteSignDown;
                    break;

                case SignPostState.Boss:
                    sign.GetComponent<SpriteRenderer>().sprite = spriteSignBoss;
                    break;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        CanMove = false;
        IsAscending = true;
        startPosition = transform.position.y;
        m_minTop = startPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(CanMove == false)
        {
            return;
        }

        if(IsAscending == true)
        {
            transform.position += transform.up * Time.deltaTime * speed;
            if (transform.position.y >= maxTop)
            {
                IsAscending = false;
                SignState = SignPostState.Down;
            }
        } else
        {
            transform.position -= transform.up * Time.deltaTime * speed;
            if (transform.position.y <= m_minTop)
            {
                IsAscending = true;
                SignState = SignPostState.Up;
            }            
        }
        //gameManager.player.UpdateScale(transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch(collision.gameObject.tag)
        {
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.Die(true);
                break;

            case "Bonus":
                Bonus bonus = collision.gameObject.GetComponent<Bonus>();
                bonus.Die();
                break;


        }

    }

    

}
