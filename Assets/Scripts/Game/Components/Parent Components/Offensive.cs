using UnityEngine;
using System.Collections;

public abstract class Offensive : MonoBehaviour {

    [SerializeField]
    public float cooldown;

    [SerializeField]
    public float attackRadius;

    public abstract bool SearchAndAttack();
    public abstract void AttackTarget(GameObject target);

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * attackRadius);
    }
}
