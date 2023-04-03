using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundController : MonoBehaviour
{
    // enemy types to spawn
    public GameObject basicEnemy;
    public GameObject flyerEnemy;
    public GameObject summonerEnemy;


    public GameObject gameOverScreen;
    public Text scoreText;
    public Text highScoreText;
    public Text currRound;

    public float timeBetweenWaves;
    public float timeBeforeRoundStarts;
    public float timeVar;

    public bool isRoundGoing;
    public bool isOnBreak;
    public bool isRoundStart;

    public static int round;

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Start()
    {
        isRoundGoing = false;
        isOnBreak = false;
        isRoundStart = true;

        timeVar = Time.time + timeBeforeRoundStarts;

        round = 1;
        currRound.text = "Round: " + round;
    }

    private void SpawnEnemies()
    {
        StartCoroutine("ISpawnEnemies");
    }
    IEnumerator ISpawnEnemies()
    {
        GameObject enemyToSpawn;
        for (int i = 0; i < round; i++)
        {
            // until round 3, only spawns basic enemy
            if (round < 3)
            {
                enemyToSpawn = basicEnemy;
            }
            else if (round < 5)
            {
                // Randomly select enemy type with 70% chance of basicEnemy and 30% chance of flyerEnemy
                if (Random.value < 0.7f)
                {
                    enemyToSpawn = basicEnemy;
                }
                else
                {
                    enemyToSpawn = flyerEnemy;
                }
            }
            else
            {
                // Randomly select enemy type with 50% chance of basicEnemy, 30% chance of flyerEnemy and 20% of summoner enemy
                float randomValue = Random.value;
                if (randomValue < 0.5f)
                {
                    enemyToSpawn = basicEnemy;
                }
                else if (randomValue < 0.8f)
                {
                    enemyToSpawn = flyerEnemy;
                }
                else
                {
                    enemyToSpawn = summonerEnemy;
                }
            }
            // Get a random position from the top of the map
            int randStart = Random.Range(0, MapGenerator.mapWidth);
            GridCell startTile = MapGenerator.mapTiles[MapGenerator.mapHeight - 1, randStart];
            Vector2 startPos = startTile.gameObj.transform.position;

            // instantiate the enemy at that position
            GameObject newEnemy = Instantiate(enemyToSpawn, startPos, Quaternion.identity);

            yield return new WaitForSeconds(1f);
        }
    }

    private void ResetLevel()
    {
        // remove all towers 
        for (int i = 0; i < Towers.towers.Count; i++)
        {
            GameObject tower = Towers.towers[i];
            if (tower == null)
            {
                continue;
            }
            Towers.towers[i] = null; // Set the element to null to indicate that it's removed
            Destroy(tower);
        }
        Towers.towers.RemoveAll(tower => tower == null);

        // set all tiles to walkable
        foreach (GridCell cell in MapGenerator.mapTiles)
        {
            cell.isWalkable = true;
        }


    }

    private void Update()
    {
        if (PlayerHealth.GetCurrentPlayerHealth() <= 0)
        {
            gameOverScreen.SetActive(true);
            scoreText.text = "Score: " + round;
            if (round > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", round);
            }
            highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            if (isRoundStart)
            {
                if (Time.time >= timeVar)
                {
                    isRoundStart = false;
                    isRoundGoing = true;
                    SpawnEnemies();
                    return;
                }

            }
            else if (isOnBreak)
            {
                if (Time.time >= timeVar)
                {
                    isOnBreak = false;
                    isRoundGoing = true;
                    SpawnEnemies();
                }

            }
            else if (isRoundGoing)
            {
                if (Enemies.enemies.Count == 0)
                {
                    isOnBreak = true;
                    isRoundGoing = false;

                    timeVar = Time.time + timeBetweenWaves;
                    round++;
                    if (round % 3 == 0)
                    {
                        MoneyManager.AddMoney((round-1)*10);
                    }
                    currRound.text = "Round: " + round;
                    ResetLevel();
                    return;

                }
            }
        }
    }
}
