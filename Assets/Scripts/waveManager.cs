using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;


public class waveManager : MonoBehaviour
{
    public spawnManager spawner;
    public float timeBetweenWaves = 10f;
    public int currentWave = 0;
    private int enemiesAlive;


    [System.Serializable]
    public class Wave 
    {
        public int[] enemies;
        public int[] counts;
    }

    public Wave[] waves;

    void Start()
    {
        SpawnWaves();

    }

    void SpawnWaves() 
    {
        if (currentWave >= waves.Length) 
        {
            SceneManager.LoadSceneAsync(2);
            return;
        }

        Wave wave = waves[currentWave];
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            int enemyType = wave.enemies[i];
            int count = wave.counts[i];
            for (int j = 0; j < wave.counts[i]; j++)
            {
                GameObject enemy = spawner.SpawnEnemy(enemyType);
                enemiesAlive++;
                enemy.GetComponent<Enemy>().OnDeath = OnEnemyDeath;

                
            }
        }

        currentWave++;
            
     
        
        

    }
    void OnEnemyDeath() 
    {
        enemiesAlive--;

        if (enemiesAlive <= 0) 
        {
            SpawnWaves();
        }
    }

}
