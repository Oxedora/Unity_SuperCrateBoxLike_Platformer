using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoundary : MonoBehaviour {


    private PlayerController playerController;

    private void Start()
    {
        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }
        if (playerController == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerController.Respawn();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
