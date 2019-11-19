using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDownObjectBehaviour : MonoBehaviour
{

    private float m_speed;

    public void SetSpeed(float speed)
    {
        m_speed = speed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position -= transform.up * Time.deltaTime * m_speed;
    }
}
