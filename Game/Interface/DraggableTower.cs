using UnityEngine;
using System.Collections;

public class DraggableTower : MonoBehaviour {

    [SerializeField]
    private Animator animatedObject;
    [SerializeField]
    private GameObject spawnedObject;

    [SerializeField]
    private uint cost;

    private Vector3 _startLocation;
    private Quaternion _startRotation;
    private Vector3 _currentPosition;

    private Transform _currentTile;
    private float _distance;

    private Gridsystem _grid;
    private const string BOOL_ACTIVATE = "Activate";

    private enum Events
    {
        IDLE = 0,
        ON_HOVER = 1,
        DRAGGING = 2,
        ON_TILE = 3,
        SPAWN = 4
    }

    private Events _currentEvent = Events.IDLE;
    
    void Start()
    {
        _startLocation = transform.position;
        _startRotation = transform.rotation;

        _grid = GameObject.FindObjectOfType<Gridsystem>();
    }

    void Update()
    {
        CheckEvents();
    }

    void CheckEvents()
    {
        if (Gamesystem.gameState == GameState.OUT_OF_WAVE)
        {
            if (_currentEvent == Events.IDLE)
            {
                if (gameObject.layer != 0 || transform.position != _startLocation || transform.rotation != _startRotation)
                {
                    gameObject.layer = 0;
                    transform.position = _startLocation;
                    transform.rotation = _startRotation;
                }
            }
            else if (_currentEvent == Events.ON_HOVER)
            {
                // Show menu and play animation.
                if (animatedObject.GetBool(BOOL_ACTIVATE) != true)
                {
                    animatedObject.SetBool(BOOL_ACTIVATE, true);
                }
            }
            else if (_currentEvent == Events.SPAWN)
            {
                SpawnCurrentStructure();
                _currentEvent = Events.IDLE;
            }
            else if (_currentEvent == Events.DRAGGING || _currentEvent == Events.ON_TILE)
            {
                CheckDragOrOnTile();
            }
        }
    }
    
    void OnMouseDown()
    {
        if (Gamesystem.gameState == GameState.OUT_OF_WAVE)
        {
            if (Gamesystem.wealth >= cost)
            {
                _grid.ShowAllTiles();

                if (_currentEvent != Events.DRAGGING)
                {
                    _currentEvent = Events.DRAGGING;
                }
            }
            else
            {
                // No money.
            }
        }
    }

    void OnMouseUp()
    {
        _grid.HideAllTiles();

        if (_currentEvent == Events.ON_TILE)
        {
            _currentEvent = Events.SPAWN;
        }
        else
        {
            _currentEvent = Events.IDLE;
        }
    }

    void OnMouseEnter()
    {
        if (_currentEvent == Events.IDLE)
        {
            _currentEvent = Events.ON_HOVER;
        }
    }
    
    void OnMouseExit()
    {
        if (_currentEvent == Events.ON_HOVER)
        {
            _currentEvent = Events.IDLE;
            animatedObject.SetBool(BOOL_ACTIVATE, false);
        }
    }

    void CheckDragOrOnTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.white);

        if (gameObject.layer != 2) { gameObject.layer = 2; }

        if (_currentEvent == Events.DRAGGING)
        {
            Vector3 rayPoint = new Vector3(ray.GetPoint(_distance).x, transform.position.y, ray.GetPoint(_distance).z);

            _distance = Vector3.Distance(transform.position, Camera.main.transform.position);

            transform.position = rayPoint;
            transform.rotation = _startRotation;
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "Tile" && hit.transform.GetComponent<Gridtile>().type == Gridtile.TileType.FRIENDLY && hit.transform.GetComponent<Gridtile>().occupied == false)
            {
                _currentTile = hit.transform;
                _currentEvent = Events.ON_TILE;
                CheckOnTile();
            }
            else
            {
                _currentEvent = Events.DRAGGING;
            }
        }
    }

    void CheckOnTile()
    {
        if (_currentEvent == Events.ON_TILE)
        {
            Vector3 position = new Vector3(_currentTile.position.x, _currentTile.transform.position.y + 0.4f, _currentTile.transform.position.z);
            transform.position = position;
            transform.rotation = _currentTile.rotation;
        }
    }

    void SpawnCurrentStructure()
    {
        Vector3 position = new Vector3(_currentTile.position.x, _currentTile.position.y + 0.4f, _currentTile.position.z);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        GameObject structure = Instantiate(spawnedObject, position, rotation) as GameObject;
        StructureController comp = structure.GetComponent<StructureController>();
        comp.cost = cost;
        comp.hasUpMoved = true;
        _currentTile.GetComponent<Gridtile>().occupied = true;

        Gamesystem.wealth -= cost;
    }
}
