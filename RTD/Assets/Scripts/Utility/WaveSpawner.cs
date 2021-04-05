using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefeb;
    public Transform SpawnPoint;
    public float TimeBetweenWaves = 2.0f;

    private float CountDown = 1f;
    private int WaveNumber = 1;

    private void Update()
    {
        if(CountDown <=0f)
        {
            SpawnWaves();
            CountDown = TimeBetweenWaves;
        }
        CountDown -= Time.deltaTime;
    }

    private void SpawnWaves()
    {
        for (int i = 0; i < WaveNumber; i++)
        {
            SpawnEnemy();
        }
        WaveNumber++;
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefeb, SpawnPoint.position, SpawnPoint.rotation);
    }
}
