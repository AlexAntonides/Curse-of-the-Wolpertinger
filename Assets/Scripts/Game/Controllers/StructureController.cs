using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Health))]
public class StructureController : MonoBehaviour {
    public static List<GameObject> structures = new List<GameObject>();

    public enum Events
    {
         READY_TO_ATTACK = 0
    }

    public Events _currentEvent;

    public GameObject upgradedObject;
    public GameObject hitPoint;
    public ParticleSystem particleOnSpawn;
    private GameObject enemyBase;

    public uint cost;

    public bool hasUpMoved = true;
    public bool hasAttacked = true;

    //private Health _healthComp;
    private Offensive _offensiveComp;

    private float timer;

    void Start()
    {
        structures.Add(gameObject);
        Instantiate(particleOnSpawn, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(new Vector2(270, 0)));

        //_healthComp = GetComponent<Health>();
        _offensiveComp = GetComponent<Offensive>();
        enemyBase = GameObject.Find(Constants.NAME_WOLPERTINGER_BASE);
    }

    void Update()
    {
        if (Gamesystem.gameState != GameState.PAUSED)
        {
            CheckAttackTimer();
            CheckBaseAttack();
        }
    }

    private void CheckAttackTimer()
    {
        timer += Time.deltaTime;
        if (_currentEvent == Events.READY_TO_ATTACK && Gamesystem.gameState == GameState.IN_WAVE)
        {
            if (timer > _offensiveComp.cooldown)
            {
                _offensiveComp.SearchAndAttack();
                timer = 0;
            }
        }
    }

    private void CheckBaseAttack()
    {
        if (hasAttacked == false && Gamesystem.gameState == GameState.OUT_OF_WAVE)
        {
            float radius = GetComponent<Offensive>().attackRadius;
            float distance = Vector3.Distance(transform.position, enemyBase.transform.position);

            if (distance < radius)
            {
                _offensiveComp.AttackTarget(enemyBase);
            }

            hasAttacked = true;
        }
    }

    public void DisableAllRays()
    {
        for (int i = 0; i < structures.Count; i++)
        {
            structures[i].layer = 2;
        }
    }

    public void EnableAllRays()
    {
        for (int i = 0; i < structures.Count; i++)
        {
            structures[i].layer = 0;
        }
    }
}
