using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    [SerializeField]
    private float _health;

    public bool isDead;
    public bool destroyOnDead = true;

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            _health = _health - damage;

            if (_health <= 0 && !isDead)
            {
                isDead = true;

                if (gameObject.GetComponent<EnemyController>() != null)
                {
                    EnemyController.enemies.Remove(gameObject);
                    Instantiate(gameObject.GetComponent<EnemyController>().particleOnDestroy, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(new Vector2(270, 0)));
                    Gamesystem.wealth += GetComponent<EnemyController>().rewardWealth;
                }
                else
                    StructureController.structures.Remove(gameObject);

                if (destroyOnDead)
                    Destroy(gameObject);
            }
        }
    }

    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
}
