using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
    
    public static List<GameObject> enemies = new List<GameObject>();

    public GameObject hitPoint;
    public ParticleSystem particleOnDestroy;

    public float addedYPosition;
    public float rewardWealth;
    public float damage;

    void Start()
    {
        enemies.Add(gameObject);
    }
    
}
