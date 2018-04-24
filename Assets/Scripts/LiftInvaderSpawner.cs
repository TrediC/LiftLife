using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftInvaderSpawner : MonoBehaviour {

    [Header("Enemy obj")]
    [Space(2)]
    public GameObject Invader;
    public GameObject[] ListOfSpawnPoints;

    /*  NOT IN USE
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

    IEnumerator SpawnInvaders(int WaveCount, int EnemysInWave, float TimeBeforeStart, float TimeBetweenEnemySpawn)
    {
        yield return new WaitForSeconds(TimeBeforeStart);
        for (int i = 0; i < WaveCount; i++)
        {
            for(int x = 0; x < EnemysInWave; x++)
            {
                int randomSpawn = UnityEngine.Random.Range(0, ListOfSpawnPoints.Length);
                Instantiate(Invader, ListOfSpawnPoints[randomSpawn].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(TimeBetweenEnemySpawn);
            }
        }
    }

    public void Spawn(int WaveCount, int EnemysInWave, float TimeBeforeStart, float TimeBetweenEnemySpawn)
    {
        StartCoroutine(SpawnInvaders(WaveCount, EnemysInWave, TimeBeforeStart, TimeBetweenEnemySpawn));
    }
}
