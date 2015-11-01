using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cone : Offensive {

    public Fire coneEffect;
    public GameObject rotatedTopObject;
    
    public float rotateSpeed;

    public GameObject targettedEnemy;

    void Awake()
    {
        coneEffect.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Gamesystem.gameState != GameState.PAUSED)
        {
            if (targettedEnemy)
            {
                float distance = Vector3.Distance(transform.position, targettedEnemy.transform.position);
                if (distance < attackRadius)
                {
                    GameObject hitP = (transform.GetComponent<StructureController>() != null) ? transform.GetComponent<StructureController>().hitPoint : transform.GetComponent<EnemyController>().hitPoint;
                    rotatedTopObject.transform.LookAt(new Vector3(targettedEnemy.transform.position.x, rotatedTopObject.transform.position.y, targettedEnemy.transform.position.z));
                    Debug.DrawLine(transform.position, targettedEnemy.transform.position);
                }
                else
                {
                    targettedEnemy = null;
                }
            }
            else
            {
                coneEffect.gameObject.SetActive(false);
            }
        }
    }

    public override bool SearchAndAttack()
    {
        if (targettedEnemy == null)
        {
            GameObject enemy = CheckForEnemy();
            AttackEnemy(enemy);

            if (enemy != null)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public override void AttackTarget(GameObject target)
    {
        //DoNothing();
    }

    GameObject CheckForEnemy()
    {
        List<GameObject> allEnemies = (gameObject.GetComponent<StructureController>() != null) ? EnemyController.enemies : StructureController.structures;
        List<GameObject> enemiesInRadius = new List<GameObject>();

        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] != null)
            {
                float distance = Vector3.Distance(transform.position, allEnemies[i].transform.position);
                if (distance < attackRadius)
                {
                    enemiesInRadius.Add(allEnemies[i]);
                }
            }
        }

        if (enemiesInRadius.Count > 0)
        {
            return enemiesInRadius[Random.Range(0, enemiesInRadius.Count)];
        }
        else
        {
            return null;
        }
    }

    void AttackEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            coneEffect.gameObject.SetActive(true);
            targettedEnemy = (enemy.GetComponent<StructureController>() != null) ? enemy.GetComponent<StructureController>().hitPoint : enemy.GetComponent<EnemyController>().hitPoint;
            coneEffect.targettedTag = enemy.tag;
        }
    }
}
