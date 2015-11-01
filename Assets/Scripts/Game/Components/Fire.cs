using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Fire : MonoBehaviour {

    public float damagePerSecond;
    public GameObject owner;

    [HideInInspector]
    public string targettedTag;
    
    public bool afterTimeDestroy = false;
    public float destroyAfterTime = 0;

    public float perSecond = 1f;

    private float _timer;
    private float _deadTimer;

    void Update()
    {
        if (Gamesystem.gameState != GameState.PAUSED)
        {
            _timer += Time.deltaTime;
            
            if (afterTimeDestroy)
            {
                _deadTimer += Time.deltaTime;

                if (_deadTimer > destroyAfterTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<StructureController>() != null || other.GetComponentInParent<EnemyController>() != null)
        {
            if (_timer >= perSecond)
            {
                _timer = 0;

                if (owner != null)
                {
                    if (other.GetComponentInParent<StructureController>() != null && owner.GetComponent<StructureController>() == null)
                    {
                        if (other.GetComponentInParent<StructureController>().tag == targettedTag)
                        {
                            HitEnemy(other.gameObject);
                        }
                    }
                    else if (other.GetComponentInParent<EnemyController>() != null && owner.GetComponent<EnemyController>() == null)
                    {
                        if (other.GetComponentInParent<EnemyController>().tag == targettedTag)
                        {
                            HitEnemy(other.gameObject);
                        }
                    }
                }
                else
                {
                    if (other.GetComponentInParent<EnemyController>() != null)
                    {
                        HitEnemy(other.gameObject);
                    }
                }
            }
        }
    }

    void HitEnemy(GameObject other)
    {
        other.GetComponentInParent<Health>().TakeDamage(damagePerSecond);
    }
}
