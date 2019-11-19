using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{

    public Boss entityToSpawn;
    public List<GameObject> EntitiesSpawned { get => m_entitiesSpawned; set => m_entitiesSpawned = value; }
    private List<GameObject> m_entitiesSpawned;

    [HideInInspector]
    public int scoreTrigger = 75;
    private int m_initialScoreTrigger = 0;

    private bool m_canSpawnEntity = false;

    public GameManager GameManager { get => m_gameManager; set => m_gameManager = value; }
    private GameManager m_gameManager;

    public int NextScoreStep { get => m_nextScoreStep; set => m_nextScoreStep = value; }
    private int m_nextScoreStep = 0;


    // Start is called before the first frame update
    private void Start()
    {
        EntitiesSpawned = new List<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(
            m_canSpawnEntity == true &&
            GameManager.Score >= NextScoreStep &&
            GameManager.edgeColliderManager.transform.position.y <= GameManager.edgeColliderManager.startPosition &&
            EntitiesSpawned.Count == 0
        )
        {
            GameManager.CustomDebug("[BossManager] NextScoreStep = " + NextScoreStep + " / Current Score = " + GameManager.Score);

            GameObject clone = Instantiate(entityToSpawn.gameObject, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<Boss>().GameManager = GameManager;
            clone.GetComponent<Boss>().SetSpeed(0f);
            clone.GetComponent<Boss>().Spawn();
            EntitiesSpawned.Add(clone);

            // We report the spawn of the next bonus if necessary
            GameManager.bonusManager.NextSpawnTrigger = (GameManager.bonusManager.CurrentTriggerScore + GameManager.bonusManager.Trigger);
            GameManager.CustomDebug("BonusManager NextSpawnTrigger : " + GameManager.bonusManager.NextSpawnTrigger);

            GameManager.edgeColliderManager.SignState = EdgeColliderManager.SignPostState.Boss;
        }
    }

    public void SetScoreTrigger(int value)
    {
        scoreTrigger = value;
        if (NextScoreStep == 0)
        {
            NextScoreStep = scoreTrigger;
            m_initialScoreTrigger = scoreTrigger;
        }
    }

    public void KillThemAll()
    {
        if(EntitiesSpawned.Count > 0)
        {

            GameManager.CustomDebug("Boss : KillThemAll");

            List<GameObject> boss = EntitiesSpawned;
            for (int i = 0; i < boss.Count; i++)
            {
                boss[i].GetComponent<Boss>().Die();
            }

            // Update boss Spawn
            SetScoreTrigger(scoreTrigger + m_initialScoreTrigger);
            NextScoreStep = GameManager.Score + (scoreTrigger * GameManager.ScoreMultiplier);
            GameManager.CustomDebug("BossManager NextScoreStep : " + NextScoreStep);
            // Restart bonus Spawn
            GameManager.bonusManager.ResetBonusSpawn(true);

        }
    }

    public void SetSpawnActive()
    {
        m_canSpawnEntity = true;
    }

    public void SetSpawnInactive()
    {
        m_canSpawnEntity = false;
    }
}
