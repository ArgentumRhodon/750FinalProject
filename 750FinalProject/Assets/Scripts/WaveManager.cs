using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    public int numWaves = 3;

    private int enemiesInWave = 0;
    private int currentWave = 0;
    private float secondsPerEnemy = 0;
    private IEnumerator spawnRoutine;
    private bool waveInProgress = false;

    [SerializeField]
    public float timeBetweenWaves = 3f;
    private float timeSinceLastWave = 0.0f;

    public float enemyCoef = 1;
    public float SPECoef = 1;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject bossPrefab;

    public int numSpawnedEnemies = 0;

    public List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        spawnRoutine = SpawnEnemies(secondsPerEnemy);
        StartNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        if (numSpawnedEnemies == enemiesInWave)
        {
            StopCoroutine(spawnRoutine);

            if(enemies.Count == 0)
            {
                EndWave();
            }
        }

        if (!waveInProgress)
        {
            timeSinceLastWave += Time.deltaTime;
        }
    }

    private void StartNextWave()
    {

        currentWave++;
        enemiesInWave = Mathf.RoundToInt(currentWave * 5 * enemyCoef);
        secondsPerEnemy = -currentWave + 4 - (SPECoef - 1) + 1;
        waveInProgress = true;
        timeSinceLastWave = 0.0f;

        spawnRoutine = SpawnEnemies(secondsPerEnemy);

        UnityEngine.Debug.Log("Wave " + currentWave + " Starting");
        StartCoroutine(spawnRoutine);
    }

    private void EndWave()
    {
        UnityEngine.Debug.Log("Wave " + currentWave + " Complete");
        waveInProgress = false;
        numSpawnedEnemies = 0;
        StopCoroutine(spawnRoutine);
        if(currentWave < numWaves)
        {
            StartCoroutine(WaveCountdown());
        }
        else
        {
            UnityEngine.Debug.Log("Waves complete");
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        Instantiate(bossPrefab, new Vector3(-11, 0, 0), Quaternion.identity);
    }

    private IEnumerator WaveCountdown()
    {
        WaitForSeconds wait = new WaitForSeconds(timeBetweenWaves / 3);

        while (timeSinceLastWave < timeBetweenWaves)
        {
            AudioManager.Instance.PlaySFX("Countdown");

            yield return wait;
        }

        StartNextWave();
    }

    private Vector2 GetRandomVerticalPosition()
    {
        float spawnX = -GameManager.Instance.cameraWorldBounds.x - 1;

        float minY = -GameManager.Instance.cameraWorldBounds.y + 1;
        float maxY = GameManager.Instance.cameraWorldBounds.y - 1;
        return new Vector2(spawnX, UnityEngine.Random.Range(minY, maxY));
    }

    public IEnumerator SpawnEnemies(float secondsPerEnemy)
    {
        WaitForSeconds wait = new WaitForSeconds(secondsPerEnemy);

        while (true)
        {
            Instantiate(enemyPrefab, GetRandomVerticalPosition(), Quaternion.identity);
            numSpawnedEnemies++;
            yield return wait;
        }
    }
}
