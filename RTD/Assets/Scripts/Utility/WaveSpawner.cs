using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    public Transform EnemyPrefeb;
    public Transform SpawnPoint;
    public float TimeBetweenWaves = 1.0f;
    public Transform EnemyPoket;

    private float Count = 0f;
    private int WaveNumber = 0;

    private void Update()
    {
        //if(Count <= 0f)
        //{
        //    SpawnWaves();
        //    Count = TimeBetweenWaves;
        //}
        //Count -= Time.deltaTime;
    }
    public void Init()
    {
        Count = 0f;
        WaveNumber = 0;
        foreach (Transform child in EnemyPoket)
        {
            Destroy(child.gameObject);
        }
    }
    private void SpawnWaves()
    {
        SpawnEnemy();
        WaveNumber++;
    }

    private void SpawnEnemy()
    {
        Transform obj = Instantiate(EnemyPrefeb, SpawnPoint.position, SpawnPoint.rotation);
        obj.GetComponent<Moving>().DestroySpawnDelegate += GetComponent<GamePlay>().MinusLife;
        obj.parent = EnemyPoket;
    }
    public IEnumerator StartSpawnWaves(int endCount, UnityAction done)
    {
        Count = 0f;
        WaveNumber = 0;
        while (WaveNumber < endCount)
        {
            Count += Time.deltaTime;
            if (Count >= TimeBetweenWaves * WaveNumber)
            {
                SpawnWaves();
            }
            yield return null;
        }
        done?.Invoke();
    }
    public int GetCountEnemies()
    {
        return EnemyPoket.childCount;
    }
}
