using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinController : MonoBehaviour
{

    public Sprite faceAlive;
    public Sprite faceDead;

    public Sprite leftArmAlive;
    public Sprite leftArmDead;

    public SpriteRenderer face;
    public SpriteRenderer leftArm;

    public enum Direction
    {
        Left,
        Right,
    }

    public Direction FacingDirection
    {
        get
        {
            return m_facingDirection;
        }
        set
        {
            if(m_facingDirection != value)
            {
                m_facingDirection = value;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
    private Direction m_facingDirection;

    private void Start()
    {
        m_facingDirection = Direction.Right;
        face.sprite = faceAlive;
        leftArm.sprite = leftArmAlive;
    }

    public void Die()
    {
        face.sprite = faceDead;
        leftArm.sprite = leftArmDead;
    }

}
