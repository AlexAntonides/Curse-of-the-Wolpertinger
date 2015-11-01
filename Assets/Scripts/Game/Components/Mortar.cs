using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mortar : Offensive {

    public Projectile projectile;
    public GameObject shootLocation;
    public ParticleSystem spawnOnFire;

    public float damage;

    public override bool SearchAndAttack()
    {
        AttackTarget(CheckForEnemy());
        return false;
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

    public override void AttackTarget(GameObject target)
    {
        if (target != null)
        {
            GameObject proj = (GameObject)Instantiate(projectile.gameObject, shootLocation.transform.position, shootLocation.transform.rotation);
            Projectile spProj = proj.GetComponent<Projectile>();
            spProj.owner = gameObject;
            spProj.point = target.transform.position;
            spProj.damage = damage;

            Instantiate(spawnOnFire, shootLocation.transform.position, shootLocation.transform.rotation);
        }
    }
}
