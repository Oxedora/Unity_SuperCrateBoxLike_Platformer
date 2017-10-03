using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject enemy;
    public GameObject enemyJumper;
    public int hazardCount = 5;
    public float spawnEnemyWait = 4.0f;
    public float minSpawnEnemyWait = 0.2f;
    public float spawnGemWait = 2.0f;
    public float minSpawnGemWait = 0.5f;
    public float startWait = 5.0f;

    public Text scoreText;
    public Text gameOverText;
    public Text bestScoreText;
    public GameObject restartButton;

    private int score;
    private int bestScore;
    private string bestScoreName = "Highscore";
    private AudioSource scoreSound;
    private List<GemSpawn> gemSpawners = new List<GemSpawn>();
    private bool gameOver;
    private Vector2 spawnValue;


    void Start()
    {
        score = 0;
        bestScore = PlayerPrefs.GetInt(bestScoreName);
        gameOver = false;
        gameOverText.text = "";
        restartButton.SetActive(false);
        scoreSound = GetComponent<AudioSource>();
        spawnValue = new Vector2(0f, 5f);

        GameObject[] gemSpawnersObject = GameObject.FindGameObjectsWithTag("GemSpawner");
        for(int i = 0; i < gemSpawnersObject.Length; i++)
        {
            gemSpawners.Add(gemSpawnersObject[i].GetComponent<GemSpawn>());
            if(gemSpawners[i] == null)
            {
                Debug.Log("Couldn't find GemSpawn script on gameObject " + gemSpawnersObject[i].name);
            }
        }

        UpdateScore();
        StartCoroutine(SpawnWaves());
        StartCoroutine(SpawnGems());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            int roll = Random.Range(0, 6);
            int nbSpawn = (roll > 4 ? 3 : (roll > 2 ? 2 : 1));

            for (int i = 0; i < nbSpawn; i++)
            {
                Vector3 spawnPosition = new Vector2(spawnValue.x, spawnValue.y);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(enemy, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(minSpawnEnemyWait);
            }

            bool jumper = Random.value > 0.7f;

            if (jumper)
            {
                Vector3 spawnPosition = new Vector2(spawnValue.x, spawnValue.y);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(enemyJumper, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(minSpawnEnemyWait);
            }

            if (gameOver)
            {
                // restartText.text = "Press 'R' for Restart";
                // restart = true;
                break;
            }

            yield return new WaitForSeconds(spawnEnemyWait);
            spawnEnemyWait = (spawnEnemyWait - 0.1f < minSpawnEnemyWait*2 ? minSpawnEnemyWait*2 : spawnEnemyWait - 0.1f);
        }
    }

    IEnumerator SpawnGems()
    {
        while(true)
        {
            List<GemSpawn> emptySpawn = spawnerWithoutGem();
            int nbEmptySpawn = emptySpawn.Count;
            if (nbEmptySpawn > 0)
            {
                int spawnSelected = Random.Range(0, nbEmptySpawn);
                emptySpawn[spawnSelected].SpawnGem();
            }
            yield return new WaitForSeconds(spawnGemWait);
            if (gameOver)
            {
                break;
            }
        }
    }

    private List<GemSpawn> spawnerWithoutGem()
    {
        List<GemSpawn> emptySpawn = new List<GemSpawn>();

        for(int i = 0; i < gemSpawners.Count; i++)
        {
            if(!gemSpawners[i].hasGem())
            {
                emptySpawn.Add(gemSpawners[i]);
            }
        }

        return emptySpawn;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        scoreSound.Play();
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score : " + score;

        if(score > bestScore)
        {
            PlayerPrefs.SetInt(bestScoreName, score);
            bestScore = score;
        }
        bestScoreText.text = "Best score : " + bestScore;
    }

    public void GameOver()
    {
        gameOver = true;
        restartButton.SetActive(true);
        gameOverText.text = "Game Over !";
    }

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
