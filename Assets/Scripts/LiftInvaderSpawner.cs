using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftInvaderSpawner : MonoBehaviour {

    [Header("Enemy obj")]
    [Space(2)]
    public GameObject Invader;
    public GameObject[] ListOfSpawnPoints;

    [Header("Enemy Wave Settings")]
    public int iWaveCount = 2;
    public int iEnemysInWave = 2;
    public float fTimeBeforeStart = 2;
    public float fTimeBetweenWaves = 3.0f;
    public float fTimeBetweenEnemySpawns = 2.0f;

    private float timer;

	void Update ()
    {
        if(timer < Time.time)
        {
            StartCoroutine(SpawnInvaders());
            timer += fTimeBetweenWaves + Time.time;
        }
    }

    IEnumerator SpawnInvaders()
    {
        yield return new WaitForSeconds(fTimeBeforeStart);
        for (int i = 0; i < iWaveCount; i++)
        {
            int randomSpawn = UnityEngine.Random.Range(0, iWaveCount);
            SpawnFuntion(randomSpawn);
            yield return new WaitForSeconds(fTimeBetweenEnemySpawns);
        }
    }

    void SpawnFuntion(int value)
    {
        Instantiate(Invader, ListOfSpawnPoints[value].transform.position, Quaternion.identity);
    }
}
