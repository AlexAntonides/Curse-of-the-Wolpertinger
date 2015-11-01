using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Ranged : Offensive
{
    public Projectile projectile;
    public GameObject shootLocation;
    
    public float damage;

    public int maxTargettedEnemies;

    public override bool SearchAndAttack()
    {
        GameObject[] enemies = CheckForEnemies();
        AttackEnemies(enemies);

        if (enemies.Length > 0)
            return true;
        else
            return false;
    }

    public override void AttackTarget(GameObject target)
    {
        GameObject[] fakeArray = {target};
        AttackEnemies(fakeArray);
    }

    GameObject[] CheckForEnemies()
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

        return enemiesInRadius.ToArray();
    }

    void AttackEnemies(GameObject[] enemies)
    {
        if (enemies != null)
        {
            int attackedEnemies = (enemies.Length >= maxTargettedEnemies) ? maxTargettedEnemies : enemies.Length;

            for (int i = 0; i < attackedEnemies; i++)
            {
                if (enemies[i] != null)
                {
                    Health healthComp = enemies[i].GetComponent<Health>();
                    if (healthComp != null && healthComp.isDead == false)
                    {
                        GameObject proj = (GameObject)Instantiate(projectile.gameObject, shootLocation.transform.position, shootLocation.transform.rotation);
                        Projectile spProj = proj.GetComponent<Projectile>();
                        spProj.owner = gameObject;
                        spProj.target = enemies[i];
                        if (enemies[i].GetComponent<EnemyController>() != null)
                        {
                            spProj.hitPoint = enemies[i].GetComponent<EnemyController>().hitPoint;
                        }
                        else if (enemies[i].GetComponent<StructureController>() != null)
                        {
                            spProj.hitPoint = enemies[i].GetComponent<StructureController>().hitPoint;
                        }
                        else if (enemies[i].GetComponent<BaseController>() != null)
                        {
                            spProj.hitPoint = enemies[i].GetComponent<BaseController>().hitLocation;
                        }
                        spProj.damage = damage;
                    }
                }
            }
        }
    }
}
