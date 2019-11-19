using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingObjectBehaviour : MonoBehaviour, IDeathBehaviour
{

    public ParticleSystem particleSystemPrefab;

    private ParticleSystem m_currentParticleSystem;

    public void Die()
    {
        m_currentParticleSystem = Instantiate(particleSystemPrefab);
        m_currentParticleSystem.transform.position = transform.position;
    }
}
