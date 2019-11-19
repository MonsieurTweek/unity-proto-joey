using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public IMovementBehaviour movementBehaviour;

    [HideInInspector]
    public IDeathBehaviour deathBehaviour;

    private void Awake()
    {
        movementBehaviour = GetComponent<IMovementBehaviour>();
        deathBehaviour = GetComponent<IDeathBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            // Cleans enemies
            gameManager.enemyManager.KillThemAll(true);
            // Clean Bosses
            gameManager.bossManager.KillThemAll();
            // Start descending
            gameManager.edgeColliderManager.IsAscending = false;
            gameManager.edgeColliderManager.SignState = EdgeColliderManager.SignPostState.Down;
            // Update catched bonus
            gameManager.bonusManager.bonusSpawnedCatched++;
            // Stop shake on camera
            Camera.main.GetComponent<CameraShakeBehaviour>().Stop();
            // Explode
            Die();
        }

    }

    public void SetSpeed(float speed)
    {
        movementBehaviour.SetSpeed(speed);
    }

    public void Die()
    {
        deathBehaviour.Die();
        Destroy(gameObject);
    }

}
