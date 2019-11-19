using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public Enemy entityToSpawn;
    public List<GameObject> EntitiesSpawned { get => m_entitiesSpawned; set => m_entitiesSpawned = value; }
    private List<GameObject> m_entitiesSpawned;

    public float minDelay = 0.5f;
    public float maxDelay = 3f;
    private float currentDelay;

    public float timer;

    private float m_minBound;
    private float m_maxBound;

    private bool m_canSpawnEntity = false;

    public GameManager GameManager { get => m_gameManager; set => m_gameManager = value; }

    private GameManager m_gameManager;

    [HideInInspector]
    public float minSpeed = 1f;
    [HideInInspector]
    public float maxSpeed = 1f;

    

    // Start is called before the first frame update
    private void Start()
    {
        EntitiesSpawned = new List<GameObject>();
        timer = minDelay;
        currentDelay = maxDelay;
        m_minBound = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
        m_maxBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_canSpawnEntity == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                GameObject clone = Instantiate(entityToSpawn.gameObject, new Vector2(Random.Range(m_minBound, m_maxBound), transform.position.y), transform.rotation) as GameObject;
                clone.GetComponent<Enemy>().gameManager = GameManager;
                clone.GetComponent<Enemy>().SetSpeed(Random.Range(minSpeed, maxSpeed));
                EntitiesSpawned.Add(clone);

                if(GameManager.currencyManager.CanSpawnEntity() == true)
                {
                    GameObject newCurrencyItem = GameManager.currencyManager.SpawnEntity();
                   // newCurrencyItem.gameObject.transform.position = new Vector3(clone.transform.position.x, newCurrencyItem.gameObject.transform.position.y);
                }

                currentDelay = Mathf.Max(currentDelay - 0.1f, minDelay);
                timer = Random.Range(minDelay, currentDelay);
            }
        }
    }

    public void KillThemAll(bool giveDeathPoints)
    {
        List<GameObject> enemies = EntitiesSpawned;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().Die(giveDeathPoints);
        }
    }

    public void SetSpawnActive()
    {
        m_canSpawnEntity = true;
        timer = Random.Range(minDelay, currentDelay);
    }

    public void SetSpawnInactive()
    {
        m_canSpawnEntity = false;
    }
}
