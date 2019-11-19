using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeBehaviour : MonoBehaviour
{

    // Transform of the camera to shake. Grabs the gameObject's transform if null
    public Transform camTransform;

    // How long the object should share for.
    public float m_shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmountMin = 0.025f;
    public float shakeAmountMax = 0.075f;
    public float decreaseFactor = 1.0f;

    private float m_currentShakeAmount = 0.0f;

    Vector3 originalPos;

    private void Awake()
    {
        if(camTransform == null)
        {
            camTransform = GetComponent<Transform>();
        }
    }

    private void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void Update()
    {
        if(m_shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * m_currentShakeAmount;
            m_shakeDuration -= Time.deltaTime * decreaseFactor;
        } else
        {
            m_shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public void Shake()
    {
        if(m_shakeDuration == 0f)
        {
            m_shakeDuration = 0.25f;
        }
        m_currentShakeAmount = Random.Range(shakeAmountMin, shakeAmountMax);
    }

    public void Stop()
    {
        m_shakeDuration = 0f;
    }
}
