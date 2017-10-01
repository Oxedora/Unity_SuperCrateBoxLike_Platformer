using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawn : MonoBehaviour {

    public GameObject gemPrefab;
    private GameObject myGem;

    private void Start()
    {
        myGem = null;
    }

    public void SpawnGem()
    {
        if(myGem == null)
        {
            GameObject newGem = Instantiate(gemPrefab, transform.position, Quaternion.identity);
            myGem = newGem;
        }
    }

    public bool hasGem()
    {
        return myGem != null;
    }
}
