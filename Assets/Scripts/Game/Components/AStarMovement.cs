using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class AStarMovement : MonoBehaviour {

    public Vector3 tilePosition;
    public float movementSpeed = 1.3f;

    [SerializeField]
    private bool walking = false;

    [SerializeField]
    private List<Gridtile> movementPath = new List<Gridtile>();

    private Animator _animationComponent;
    private const string BOOL_WALKING = "Walking";

    void Start()
    {
        tilePosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 1), Mathf.Round(transform.position.z));
        FollowPath(AStar.Search(GameObject.FindGameObjectWithTag(Constants.TAG_STARTTILE).GetComponent<Gridtile>(), GameObject.FindGameObjectWithTag(Constants.TAG_ENDTTILE).GetComponent<Gridtile>()));
        _animationComponent = GetComponent<Animator>();
        _animationComponent.SetBool(BOOL_WALKING, true);
    }

    void Update()
    {
        if (Gamesystem.gameState != GameState.PAUSED)
        {
            tilePosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 1), Mathf.Round(transform.position.z));
            CheckMovementPath();
        }
    }

    public void CheckMovementPath()
    {
        if (movementPath.Count > 0 && walking == true)
        {
            float value = 0.4f;

            Gridtile gTile = movementPath[0];
            Vector3 gTilePosition = gTile.transform.position;

            Vector3 targettedPosition = new Vector3(gTilePosition.x, gTilePosition.y + transform.GetComponent<EnemyController>().addedYPosition, gTilePosition.z);
            Vector3 currentPosition = new Vector3(Mathf.Clamp(transform.position.x, transform.position.x - value, transform.position.x + value), Mathf.Clamp(transform.position.y, transform.position.y - value, transform.position.y + value), Mathf.Clamp(transform.position.z, transform.position.z - value, transform.position.z + value));
            
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targettedPosition.x, targettedPosition.y, targettedPosition.z), movementSpeed * Time.deltaTime);
            transform.LookAt(new Vector3(gTilePosition.x, transform.position.y, gTilePosition.z));

            if (currentPosition == targettedPosition)
            {
                movementPath.RemoveAt(0);
            }

            if (movementPath.Count == 0)
            {
                GameObject.Find(Constants.NAME_FRIENDLY_BASE).GetComponent<Health>().TakeDamage(gameObject.GetComponent<EnemyController>().damage);
                gameObject.GetComponent<Health>().TakeDamage(1000);
            }
        }
    }

    public void Walk()
    {
        walking = true;
    }

    public void Stop()
    {
        walking = false;
    }
    
    public void FollowPath(List<Gridtile> path)
    {
        movementPath = path;
    }
}
