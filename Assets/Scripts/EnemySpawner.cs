using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Camera mainCamera;
    public float maxSpawnDistance = 0.1f;
    public float spawnInterval = 3f;
    private Coroutine spawnEnemiesCoroutine;

    private void Start()
    {
        if(spawnEnemiesCoroutine == null)
        {
            spawnEnemiesCoroutine = StartCoroutine(SpawnEnemiesWithDelay());
        }
        else
        {
            StopCoroutine(SpawnEnemiesWithDelay());
        }        
    }

    private IEnumerator SpawnEnemiesWithDelay()
    {
        SpawnEnemy();
        yield return new WaitForSeconds(spawnInterval);

        yield return StartCoroutine(SpawnEnemiesWithDelay());
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemyPrefab = GetRandomEnemyPrefab();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calculate the random spawn position
        float randomX = Random.Range(-cameraWidth - maxSpawnDistance, cameraWidth + maxSpawnDistance);
        float randomY = Random.Range(-cameraHeight - maxSpawnDistance, cameraHeight + maxSpawnDistance);

        // Determine the spawn position quadrant based on randomX and randomY signs
        int quadrantX = (int)Mathf.Sign(randomX);
        int quadrantY = (int)Mathf.Sign(randomY);

        // Calculate the actual spawn position based on the quadrant
        float spawnX = quadrantX * (cameraWidth + Mathf.Abs(randomX));
        float spawnY = quadrantY * (cameraHeight + Mathf.Abs(randomY));

        // Return the spawn position
        return new Vector3(spawnX, spawnY, 0f);
    }

    private GameObject GetRandomEnemyPrefab()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("No enemy prefabs assigned!");
            return null;
        }

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[randomIndex];
    }
}

