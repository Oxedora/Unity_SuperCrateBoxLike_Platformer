using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGem : MonoBehaviour {

    public int scoreValue = 10;
    private GameController gameController;


    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            gameController.AddScore(scoreValue);
        }

        if(collider.CompareTag("Enemy"))
        {
            EnemyController enemyController = collider.GetComponent<EnemyController>();
            
            if (enemyController == null)
            {
                Debug.Log("Cannot find 'EnemyController' script");
            }
            else
            {
                enemyController.AddScore(scoreValue);
            }
        }

        Destroy(gameObject);
    }
}
