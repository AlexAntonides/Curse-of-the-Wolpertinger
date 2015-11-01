using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wavesystem : MonoBehaviour {
    public static uint currentWave = 1;
    public static uint nextWave = currentWave + 1;
    public static uint waveCluster = 1;

    public EnemyController[] enemyTypes;
    public int[] amountEnemies;

    public bool waveSpawned = false;

    [HideInInspector]
    public List<EnemyController> currentEnemies = new List<EnemyController>();
    private List<EnemyController> _nextEnemies = new List<EnemyController>();

    private Transform spawnLocation;
    
    public void NextWave()
    {
        if(Gamesystem.gameState == GameState.OUT_OF_WAVE && waveSpawned == false)
        {
            if (spawnLocation == null)
                spawnLocation = GameObject.FindGameObjectWithTag(Constants.TAG_STARTTILE).transform;

            int totalMoved = 0;
            for(int i = 0; i < StructureController.structures.Count; i++)
            {
                if (StructureController.structures[i].GetComponent<StructureController>().hasUpMoved)
                {
                    totalMoved++;
                }
            }

            if (totalMoved == StructureController.structures.Count)
            {
                StartWave();
            }
            else
            {
                //There are some structures who haven't moved yet.
            }
        }
    }

    private void StartWave()
    {
        Gamesystem.gameState = GameState.IN_WAVE;

        currentEnemies = GetWave(currentWave);
        _nextEnemies = GetWave(nextWave);
        
        SpawnWave(currentEnemies);

        currentWave++;
        nextWave = currentWave + 1;

        if (currentWave > 5)
        {
            currentWave = 1;
            nextWave = currentWave + 1;
            waveCluster++;
        }

        if (nextWave > 5)
            nextWave = 1;
    }

    public List<EnemyController> GetWave(uint wave)
    {
        List<EnemyController> waveObjects = new List<EnemyController>();
        
        for (int i = 0; i < amountEnemies[wave - 1] * waveCluster; i++)
        {
            waveObjects.Add(enemyTypes[wave - 1]);
        }

        return waveObjects;
    }

    public void UpdateStats(GameObject enemy)
    {
        enemy.GetComponent<EnemyController>().damage = enemy.GetComponent<EnemyController>().damage * waveCluster;
        enemy.GetComponent<EnemyController>().rewardWealth = enemy.GetComponent<EnemyController>().rewardWealth * waveCluster;
        enemy.GetComponent<Health>().health = enemy.GetComponent<Health>().health * waveCluster;
        enemy.GetComponent<AStarMovement>().movementSpeed = enemy.GetComponent<AStarMovement>().movementSpeed * waveCluster;
    }

    void SpawnWave(List<EnemyController> wave)
    {
        for (int i = 0; i < wave.Count; i++)
        {
            StartCoroutine(DelayInstantiate(wave[i].gameObject, 2f * i, i, wave.Count));
        }
    }

    IEnumerator DelayInstantiate(GameObject enemy, float seconds, int i, int count)
    {
        yield return new WaitForSeconds(seconds);
        GameObject spawnedEnemy = Instantiate(enemy, new Vector3(spawnLocation.position.x, spawnLocation.position.y + enemy.GetComponent<EnemyController>().addedYPosition, spawnLocation.position.z), Quaternion.Euler(new Vector3(0,180,0))) as GameObject;
        UpdateStats(spawnedEnemy);

        if (i == count - 1)
        {
            waveSpawned = true;
        }
    }
}
