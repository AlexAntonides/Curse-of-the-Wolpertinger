using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Projectile : MonoBehaviour {

    public float damage = 1;
    public float bulletSpeed;
    public float rotationSpeed;

    public GameObject owner;

    [HideInInspector]
    public GameObject target;
    public GameObject hitPoint;
    public GameObject spawnOnReachedLocation;

    [HideInInspector]
    public Vector3 point = Vector3.zero;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Update()
    {
        if (Gamesystem.gameState != GameState.PAUSED)
        {
            if (target != null && hitPoint != null)
            {
                UpdateMovement();
            }
            else if (point == Vector3.zero)
            {
                Destroy(gameObject);
            }
            else if (point != Vector3.zero)
            {
                UpdateMovementToPoint();
            }
        }
    }

    void UpdateMovement()
    {
        Debug.DrawLine(transform.position, hitPoint.transform.position);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hitPoint.transform.position - transform.position), rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    void UpdateMovementToPoint()
    {
        Debug.DrawLine(transform.position, point);

        transform.LookAt(point);
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, point);
        if (distance < 1f)
        {
            Destroy(gameObject);
            Instantiate(spawnOnReachedLocation, new Vector3(point.x, point.y + 0.25f, point.z), Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hitPoint != null)
        {
            if (other.gameObject == hitPoint)
            {
                target.GetComponent<Health>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
