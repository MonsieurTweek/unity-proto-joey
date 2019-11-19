using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [HideInInspector]
    public IMovementBehaviour movementBehaviour;

    public GameManager GameManager { get => m_gameManager; set => m_gameManager = value; }
    private GameManager m_gameManager;

    private float m_delayBeforeActiveSpawners = 2f;
    private float m_timer;
    private bool m_isActive = false;

    private void Awake()
    {
        movementBehaviour = GetComponent<IMovementBehaviour>();
        m_timer = m_delayBeforeActiveSpawners;
    }

    private void Update()
    {
        if(m_isActive == false)
        {
            m_timer -= Time.deltaTime;
            if (m_timer <= 0f)
            {
                GameManager.StartSpawners();
                SetSpeed(1f);
                m_isActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.SetSpeed(enemy.GetSpeed() * 2);
            enemy.GetComponent<SpriteRenderer>().color = new Color32(200, 0, 0, 255);
        }

    }

    public void SetSpeed(float speed)
    {
        movementBehaviour.SetSpeed(speed);
    }

    public void Die()
    {
        GameManager.edgeColliderManager.CanMove = true;
        GameManager.ScoreMultiplier += 1;
        //GameManager.player.UpdateScale();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.bossManager.EntitiesSpawned.Remove(gameObject);
        GameManager.StartSpawners();
    }

    public void Spawn()
    {
        GameManager.enemyManager.KillThemAll(false);
        GameManager.edgeColliderManager.CanMove = false;
        GameManager.PauseSpawners();
    }

}
