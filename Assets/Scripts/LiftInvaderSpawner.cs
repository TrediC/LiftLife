using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftInvaderSpawner : MonoBehaviour {

    [Header("Enemy obj")]
    [Space(2)]
    public GameObject Invader;
    public GameObject[] ListOfSpawnPoints;

    /*
    [Header("Enemy Wave Settings")]
    [Space(5)]
    public int iWaveCount = 2;
    public int iEnemysInWave = 2;
    public float fTimeBeforeStart = 2;
    public float fTimeBetweenWaves = 3.0f;
    public float fTimeBetweenEnemySpawns = 2.0f;
    */

    public bool canSpawnEnemys = true;

    void Update()
    {
        if (!canSpawnEnemys)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnInvaders(int WaveCount, int EnemysInWave, float TimeBeforeStart, float TimeBetweenWaves, float TimeBetweenEnemySpawn)
    {
        int iWaveCount = WaveCount;
        int iEnemysInWave = EnemysInWave;
        float fTimeBeforeStart = TimeBeforeStart;
        float fTimeBetweenWaves = TimeBetweenWaves;
        float fTimeBetweenEnemySpawn = TimeBetweenEnemySpawn;

        yield return new WaitForSeconds(TimeBeforeStart);
        for (int i = 0; i < iWaveCount; i++)
        {
            int randomSpawn = UnityEngine.Random.Range(0, iWaveCount);
            Instantiate(Invader, ListOfSpawnPoints[randomSpawn].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(TimeBetweenEnemySpawn);
        }
        yield return new WaitForSeconds(TimeBetweenWaves);
        SpawnInvaders(iWaveCount, iEnemysInWave, fTimeBeforeStart, fTimeBetweenWaves, fTimeBetweenEnemySpawn);
    }

    public void Spawn(int WaveCount, int EnemysInWave, float TimeBeforeStart, float TimeBetweenWaves, float TimeBetweenEnemySpawn)
    {
        StartCoroutine(SpawnInvaders(WaveCount, EnemysInWave, TimeBeforeStart, TimeBetweenWaves, TimeBetweenEnemySpawn));
    }
}
