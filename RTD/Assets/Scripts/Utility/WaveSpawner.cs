using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    public VoidDelGameObject SpawnDelegate = null;
    public Transform EnemyPrefeb;
    public Transform SpawnPoint = null;
    public float TimeBetweenWaves = 1.0f;
    public Transform EnemyPoket = null;

    private float Count = 0f;
    private int WaveNumber = 0;

    string SpawnPath;

    private void Start()
    {
        // Set Reference
        if (SpawnPoint == null) SpawnPoint = GameObject.Find("SpawnPoint").transform;
        if (EnemyPoket == null) EnemyPoket = GameObject.Find("Enemies").transform;
    }
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
        GameObject obj = Instantiate(Resources.Load(SpawnPath), SpawnPoint.position, SpawnPoint.rotation) as GameObject;
        //obj = Instantiate(EnemyPrefeb, SpawnPoint.position, SpawnPoint.rotation);
        obj.GetComponent<Moving>().DestroySpawnDelegate += GetComponent<GamePlay>().MinusLife;
        obj.transform.parent = EnemyPoket;
        SpawnDelegate?.Invoke(obj);
    }
    public IEnumerator StartSpawnWaves(string path, int endCount, UnityAction done)
    {
        Count = 0f;
        WaveNumber = 0;
        SpawnPath = path;
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
