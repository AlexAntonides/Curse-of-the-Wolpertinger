using UnityEngine;
using System.Collections;

public class Defense : MonoBehaviour {

    public GameObject structureToSpawn;
    public GameObject spawnLocation;

    public float defenseRange;

    private bool structureSpawned = false;

    void Update()
    {
        if (!structureSpawned && Gamesystem.gameState == GameState.OUT_OF_WAVE)
        {
            for (int i = 0; i < StructureController.structures.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, StructureController.structures[i].transform.position);

                if (distance < defenseRange)
                {
                    SpawnStructure();
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, defenseRange);
    }

    private void SpawnStructure()
    {
        Instantiate(structureToSpawn, spawnLocation.transform.position, spawnLocation.transform.rotation);
        structureSpawned = true;
    }
}
