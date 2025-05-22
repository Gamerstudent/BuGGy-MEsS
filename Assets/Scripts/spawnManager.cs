using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    //public float spawnDelay = 0.5f;
    // if you have time modify the script to use the pooling method for now just use this.

    //public void StartWave(int enemyIndex, int enemyCount) 
    //{
    //    StartCoroutine(SpawnWave(enemyIndex, enemyCount));
    //}
    // bruh adding delay between spawns is much more complex.

    //private IEnumerator SpawnWave(int enemyIndex, int enemyCount) 
    //{
    //    for (int i = 0; i < enemyCount; i++) 
    //    {
    //        SpawnEnemy(enemyIndex);
    //        yield return new WaitForSeconds(spawnDelay);
    //    }
    //}
    public GameObject SpawnEnemy(int enemyIndex) 
    {
        int pointIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoints[pointIndex].position, Quaternion.identity);
        return enemy;
    }
}
