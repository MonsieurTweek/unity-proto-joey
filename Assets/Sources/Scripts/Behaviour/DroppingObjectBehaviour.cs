using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingObjectBehaviour : MonoBehaviour, IMovementBehaviour
{

    public float Speed { get => m_speed; set => m_speed = value; }
    private float m_speed;


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
        transform.position -= transform.up * Time.deltaTime * Speed;
    }
}
