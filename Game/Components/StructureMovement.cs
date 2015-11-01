using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(StructureController))]
public class StructureMovement : MonoBehaviour {

    public static bool buttonsSpawned = false;

    public enum Events
    {
        IDLE = 0,
        SELECTED = 1,
        MOVING = 2,
        UPGRADE = 3
    }

    public Events _currentEvents = Events.IDLE;

    public List<Gridtile> openList = new List<Gridtile>();

    public bool addedSideExtra = false;

    public float movementRange;

    private Gridsystem _gSystem;
    private StructureController _sController;

    void Start()
    {
        _gSystem = GameObject.FindObjectOfType<Gridsystem>();
        _sController = gameObject.GetComponent<StructureController>();
    }

    void Update()
    {
        if (Gamesystem.gameState != GameState.PAUSED)
        {
            if (transform.position.z >= 15)
                GetComponent<StructureController>().hasUpMoved = true;

            if (Gamesystem.gameState == GameState.OUT_OF_WAVE && !_sController.hasUpMoved)
                UpdateEvents();
        }   
    }

    void UpdateEvents()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_currentEvents == Events.SELECTED)
                {
                    if (hit.transform.name == Constants.NAME_MOVE_BUTTON)
                    {
                        _currentEvents = Events.MOVING;
                        _gSystem.ShowAllTiles(0);
                        _gSystem.ShowAllTiles(1);
                        UpdateTiles();
                    }
                    else if (hit.transform.name == Constants.NAME_UPGRADE_BUTTON)
                    {
                        _currentEvents = Events.UPGRADE;
                        UpgradeStructure();
                    }
                    else
                    {
                        _currentEvents = Events.IDLE;
                    }

                    Destroy(GameObject.FindGameObjectWithTag(Constants.TAG_SELECTED_BUTTONS));
                    buttonsSpawned = false;
                    
                    _sController.EnableAllRays();
                    Gridsystem.EnableAllRays();
                }
                else if (_currentEvents == Events.MOVING)
                {
                    if (hit.transform.name == Constants.NAME_TILE)
                    {
                        if (openList.Contains(hit.transform.GetComponent<Gridtile>()))
                        {
                            Gridsystem.getGridTile(transform.position).occupied = false;
                            transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.4f, hit.transform.position.z);
                            hit.transform.GetComponent<Gridtile>().occupied = true;

                            Reset();

                            _gSystem.HideAllTiles(0);
                            _gSystem.HideAllTiles(1);

                            _sController.hasUpMoved = true;
                            _currentEvents = Events.IDLE;
                        }
                        else
                        {
                            _currentEvents = Events.IDLE;

                            Reset();

                            _gSystem.HideAllTiles(0);
                            _gSystem.HideAllTiles(1);
                        }
                    }
                    else
                    {
                        _currentEvents = Events.IDLE;

                        Reset();

                        _gSystem.HideAllTiles(0);
                        _gSystem.HideAllTiles(1);
                    }
                }
                else if (_currentEvents == Events.IDLE)
                {
                    if (hit.transform == _sController.hitPoint.transform)
                    {
                        _currentEvents = Events.SELECTED;
                        ShowMiniMenu();
                        _sController.DisableAllRays();
                        Gridsystem.DisableAllRays();
                    }
                }
            }
        }
    }

    private void UpgradeStructure()
    {
        if (_currentEvents == Events.UPGRADE && Gamesystem.gameState == GameState.OUT_OF_WAVE)
        {
            _currentEvents = Events.IDLE;
            
            if (_sController.upgradedObject != null)
            {
                Gamesystem.wealth -= _sController.cost;

                GameObject newStructure = Instantiate(_sController.upgradedObject, transform.position, transform.rotation) as GameObject;
                newStructure.GetComponent<StructureController>().hasUpMoved = true;
                StructureController.structures.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void ShowMiniMenu()
    {
        if (!buttonsSpawned)
        {
            GameObject parent = Instantiate(Gamesystem.upgradeMovementButtons, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), Quaternion.Euler(Vector3.zero)) as GameObject;

            if (_sController.upgradedObject == null || Gamesystem.wealth < _sController.cost)
            {
                parent.transform.FindChild(Constants.NAME_UPGRADE_BUTTON).gameObject.SetActive(false);
            }
            
            buttonsSpawned = true;
        }
    }

    private void UpdateTiles()
    {
        for (int i = 0; i < movementRange + 1; i++)
        {
            List<Gridtile> tiles = Gridsystem.GetGridTiles();

            if (tiles.Count > 0)
            {
                Vector3 tilePosition = new Vector3(transform.position.x, tiles[0].transform.position.y, transform.position.z + i);
                Vector3 tileSidePositionX = new Vector3(transform.position.x + 1, tiles[0].transform.position.y, transform.position.z + 1);
                Vector3 tileSidePositionZ = new Vector3(transform.position.x - 1, tiles[0].transform.position.y, transform.position.z + 1);

                for (int j = 0; j < tiles.Count - 1; j++)
                {
                    if (tiles[j].transform.position == tilePosition || tiles[j].transform.position == tileSidePositionX || tiles[j].transform.position == tileSidePositionZ)
                    {
                        if (tiles[j].type == Gridtile.TileType.FRIENDLY || tiles[j].type == Gridtile.TileType.ENEMY)
                        {
                            if (tiles[j].occupied == false)
                            {
                                openList.Add(tiles[j]);
                            }
                            else
                            {
                                if (tiles[j].transform.position == tilePosition)
                                {
                                    Vector3 addedTilePosition = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z + 1);
                                    if (Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.FRIENDLY || Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.ENEMY)
                                    {
                                        if (tiles[j].occupied == false)
                                        {
                                            openList.Add(Gridsystem.getGridTile(addedTilePosition));
                                        }
                                    }
                                }
                                else if (tiles[j].transform.position == tileSidePositionX)
                                {
                                    Vector3 addedTilePosition = new Vector3(tilePosition.x + 1, tilePosition.y, tilePosition.z + 1);
                                    if (Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.FRIENDLY || Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.ENEMY)
                                    {
                                        if (tiles[j].occupied == false)
                                        {
                                            openList.Add(Gridsystem.getGridTile(addedTilePosition));
                                        }
                                    }
                                }
                                else if (tiles[j].transform.position == tileSidePositionZ)
                                {
                                    Vector3 addedTilePosition = new Vector3(tilePosition.x - 1, tilePosition.y, tilePosition.z + 1);
                                    if (Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.FRIENDLY || Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.ENEMY)
                                    {
                                        if (tiles[j].occupied == false)
                                        {
                                            openList.Add(Gridsystem.getGridTile(addedTilePosition));
                                        }
                                    }
                                }
                            }
                        }
                        else if(tiles[j].type == Gridtile.TileType.PATH)
                        {
                            if (tiles[j].transform.position == tilePosition)
                            {
                                Vector3 addedTilePosition = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z + 1);
                                if (Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.FRIENDLY || Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.ENEMY)
                                {
                                    if (tiles[j].occupied == false)
                                    {
                                        openList.Add(Gridsystem.getGridTile(addedTilePosition));
                                    }
                                }
                            } 
                            if (addedSideExtra)
                            {
                                if (tiles[j].transform.position == tileSidePositionX)
                                {
                                    Vector3 addedTilePosition = new Vector3(tilePosition.x + 1, tilePosition.y, tilePosition.z + 1);
                                    if (Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.FRIENDLY || Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.ENEMY)
                                    {
                                        if (tiles[j].occupied == false)
                                        {
                                            openList.Add(Gridsystem.getGridTile(addedTilePosition));
                                        }
                                    }
                                }
                                else if (tiles[j].transform.position == tileSidePositionZ)
                                {
                                    Vector3 addedTilePosition = new Vector3(tilePosition.x - 1, tilePosition.y, tilePosition.z + 1);
                                    if (Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.FRIENDLY || Gridsystem.getGridTile(addedTilePosition).type == Gridtile.TileType.ENEMY)
                                    {
                                        if (tiles[j].occupied == false)
                                        {
                                            openList.Add(Gridsystem.getGridTile(addedTilePosition));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int k = 0; k < openList.Count; k++)
        {
            openList[k].GetComponent<Renderer>().material = openList[k].movementMaterial;
            openList[k].ChangeTransparency(0.50f);
        }
    }

    private void Reset()
    {
        for (int i = 0; i < openList.Count; i++)
        {
            openList[i].GetComponent<Renderer>().material = openList[i].startMaterial;
        }

        openList.Clear();
    }
}
