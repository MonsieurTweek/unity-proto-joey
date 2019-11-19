using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{

    public CurrencyItem entityToSpawn;

    private int m_trigger = 5;
    private int m_currentTriggerScore = 0;

    public GameManager GameManager { get => m_gameManager; set => m_gameManager = value; }
    public int Trigger { get => m_trigger; set => m_trigger = value; }

    private GameManager m_gameManager;

    private float m_minBound;
    private float m_maxBound;

    // Start is called before the first frame update
    void Start()
    {
        m_minBound = Camera.main.ScreenToWorldPoint(new Vector2(0 + (Screen.width * 0.1f), 0)).x;
        m_maxBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - (Screen.width * 0.1f), 0)).x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanSpawnEntity()
    {
        return m_currentTriggerScore >= Trigger;
    }

    public GameObject SpawnEntity()
    {
        float spawnX = 0f;

        // Player is on the left, x is negative
        if(GameManager.player.transform.position.x < 0f)
        {
            spawnX = Random.Range(0, m_maxBound);
        } else
        {
            spawnX = Random.Range(m_minBound, 0);
        }

        // Player is on the right, x is positive

        GameObject clone = Instantiate(entityToSpawn.gameObject, new Vector2(spawnX, transform.position.y), transform.rotation) as GameObject;
        clone.GetComponent<CurrencyItem>().GameManager = GameManager;
        Trigger *= 2;
        return clone;
    }

    public void IncrementTriggerScore(int increment)
    {
        m_currentTriggerScore += increment;
    }
}
