using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public IMovementBehaviour movementBehaviour;

    [HideInInspector]
    public IDeathBehaviour deathBehaviour;

    public List<Sprite> sprites;

    private void Awake()
    {
        movementBehaviour = GetComponent<IMovementBehaviour>();
        deathBehaviour = GetComponent<IDeathBehaviour>();
    }

    private void Start()
    {
        // Select a random sprite
        Sprite currentSprite = sprites[Random.Range(0, sprites.Count)];
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = currentSprite;
        if(Random.Range(0, 1) == 0)
        {
            sr.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Die();
            Die(false);
        }

    }

    public void SetSpeed(float speed)
    {
        movementBehaviour.SetSpeed(speed);
    }

    public float GetSpeed()
    {
        return movementBehaviour.Speed;
    }

    public void Die(bool giveDeathPoints)
    {
        if(giveDeathPoints == true)
        {
            gameManager.Score += (1 * gameManager.ScoreMultiplier);
            gameManager.currencyManager.IncrementTriggerScore(1);
            gameManager.bonusManager.IncrementTriggerScore(1);
        }
        deathBehaviour.Die();
        Camera.main.GetComponent<CameraShakeBehaviour>().Shake();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        gameManager.enemyManager.EntitiesSpawned.Remove(gameObject);
    }

}
