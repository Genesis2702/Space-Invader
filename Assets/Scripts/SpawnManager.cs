using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float xBound = 12;
    private float yBound = 5;
    public GameObject enemyPrefab;
    private int enemyCount;
    private int enemyToSpawn = 3;
    private PlayerController playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        SpawningEnemy(enemyToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.isAlive == true)
        {
            enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
            if (enemyCount == 0)
            {
                enemyToSpawn++;
                SpawningEnemy(enemyToSpawn);
            }
        }
    }

    private Vector2 GenerateRandomPosition()
    {
        return new Vector2(Random.Range(-xBound, xBound + 1), Random.Range(1, yBound));
    }

    private void SpawningEnemy(int numberOfEnemy)
    {
        for (int i = 0; i < numberOfEnemy; i++)
        {
            Instantiate(enemyPrefab, GenerateRandomPosition(), enemyPrefab.transform.rotation);
        }
    }
}
