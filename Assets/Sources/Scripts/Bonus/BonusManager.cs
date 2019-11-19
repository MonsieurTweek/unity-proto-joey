using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{

    public Bonus entityToSpawn;

    private int m_trigger = 15;
    private int m_currentTriggerScore = 0;
    private int m_nextSpawnTrigger;

    public int bonusSpawnedCatched = 1;

    private float m_minBound;
    private float m_maxBound;

    private bool m_canSpawnEntity = false;

    public float PositionTrigger { get => m_positionTrigger; set => m_positionTrigger = value; }
    public GameManager GameManager { get => m_gameManager; set => m_gameManager = value; }
    public int NextSpawnTrigger { get => m_nextSpawnTrigger; set => m_nextSpawnTrigger = value; }
    public int Trigger { get => m_trigger; set => m_trigger = value; }
    public int CurrentTriggerScore { get => m_currentTriggerScore; set => m_currentTriggerScore = value; }

    private GameManager m_gameManager;
    private float m_positionTrigger = 1f;

    [HideInInspector]
    public float minSpeed = 1f;
    [HideInInspector]
    public float maxSpeed = 1f;


    // Start is called before the first frame update
    private void Start()
    {
        m_minBound = Camera.main.ScreenToWorldPoint(new Vector2(0 + (Screen.width * 0.1f), 0)).x;
        m_maxBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - (Screen.width * 0.1f), 0)).x;
    }

    // Update is called once per frame
    private void Update()
    {
        if(CheckCanSpawnEntity() == true)
        {
            GameObject clone = Instantiate(entityToSpawn.gameObject, new Vector2(Random.Range(m_minBound, m_maxBound), transform.position.y), transform.rotation) as GameObject;
            clone.GetComponent<Bonus>().gameManager = GameManager;
            clone.GetComponent<Bonus>().SetSpeed(Random.Range(minSpeed, maxSpeed));

            NextSpawnTrigger = CurrentTriggerScore + (Trigger * bonusSpawnedCatched);
            GameManager.CustomDebug("[BonusManager] NextSpawnTrigger : " + NextSpawnTrigger);
            GameManager.CustomDebug("[BonusManager] Current TriggerScore : " + CurrentTriggerScore);
        }
    }

    private bool CheckCanSpawnEntity()
    {
        // System stopped
        if (m_canSpawnEntity == false)
        {
            return false;
        }

        // Score check
        if(CurrentTriggerScore < NextSpawnTrigger)
        {
            return false;
        }

        // Boss check (always allow bonus spawn during boss encounter)
        if(GameManager.bossManager.EntitiesSpawned.Count > 0)
        {
            return true;
        }

        // Edge Collider Movement
        if(GameManager.edgeColliderManager.IsAscending == false)
        {
            return false;
        }

        // Edge Collider Position
        if(GameManager.edgeColliderManager.gameObject.transform.position.y < m_positionTrigger)
        {
            return false;
        }

        return true;
    }

    public void SetScoreTrigger(int value)
    {
        Trigger = value;
        if(NextSpawnTrigger == 0)
        {
            NextSpawnTrigger = Trigger;
        }
    }

    public void IncrementTriggerScore(int increment)
    {
        CurrentTriggerScore += increment;
    }

    public void SetSpawnActive()
    {
        m_canSpawnEntity = true;
    }

    public void SetSpawnInactive()
    {
        m_canSpawnEntity = false;
    }

    public void ResetBonusSpawn(bool withIncrement)
    {
        CurrentTriggerScore = 0;
        bonusSpawnedCatched = 1;
        if (withIncrement == true)
        {
            Trigger = Trigger * 2;
        }
        NextSpawnTrigger = Trigger;
        GameManager.CustomDebug("BonusManager NextSpawnTrigger : " + NextSpawnTrigger);
    }
}
